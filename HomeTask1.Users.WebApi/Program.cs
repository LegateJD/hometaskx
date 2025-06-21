using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using HomeTask1.Users.Domain;
using HomeTask1.Users.Infrastructure;
using HomeTask1.Users.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.WebHost.UseUrls("http://0.0.0.0:80");
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddTransient<IProjectServiceClient, ProjectServiceClient>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ISubscriptionService, SubscriptionService>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddHttpClient<IProjectServiceClient, ProjectServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:ProjectService"]);
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    var retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetry(3, retryAttempt => 
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    retryPolicy.Execute(() =>
    {
        dbContext.Database.EnsureCreated();
    });
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
