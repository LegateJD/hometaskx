## 🧩 Microservices Example Application

This repository contains an example application composed of two microservices.

---

## 🚀 Running the Application

You can start the application using Docker Compose:

```bash
docker-compose up
```
> The application uses port **5000** for the **User Service** and port **5001** for the **Project Service** by default.
> You can access them at:
>
> * [http://localhost:5000](http://localhost:5000) (User Service)
> * [http://localhost:5001](http://localhost:5001) (Project Service)

---


## 🧪 Unit Testing

Unit tests are located in the `HomeTask1.Users.WebApi.Tests` project.

Run the tests using your preferred .NET test runner (e.g., `dotnet test` or Rider’s test runner).


## 🧪 Integration Testing

Integration tests are located in the `HomeTask1.Projects.WebApi.IntegrationTests` project.

* The tests use the Testcontainers library to spin up isolated containers for dependent services (e.g., databases, APIs).
* Before running the tests, make sure all Docker images are built:

```bash
docker-compose build
```

Run the tests using your preferred .NET test runner (e.g., `dotnet test` or Rider’s test runner).

---

## 📚 Swagger UI

You can access the Swagger documentation at:

* [http://localhost:5000/swagger](http://localhost:5000/swagger)
* [http://localhost:5001/swagger](http://localhost:5001/swagger)
