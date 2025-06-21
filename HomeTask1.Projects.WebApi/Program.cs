using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using HomeTask1.Projects.Domain;
using HomeTask1.Projects.Infrastructure;
using HomeTask1.Projects.WebApi;
using HomeTask1.Projects.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IUserServiceClient, UserServiceClient>();
builder.Services.AddScoped<IUserSettingRepository, UserSettingRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserSettingService, UserSettingService>();
builder.Services.AddScoped<ProjectsDbContext>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.WebHost.UseUrls("http://0.0.0.0:80");
builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:UserService"]);
    client.Timeout = TimeSpan.FromSeconds(30);
});
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


var app = builder.Build();
MongoConfig.ConfigureMappings();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();

