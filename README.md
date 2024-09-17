**TO-DO Web Application**

This project is a TO-DO web application built with ASP.NET Core 6.0, Entity Framework Core, and SQLite. It allows multiple users to register, login, and manage their to-do activities. The application is containerized using Docker for easy deployment.

**Features**
- User Registration
- User Login
- Create, view, edit, and delete to-do items
- Mark to-do items as Done, Canceled, or Unmarked
- SQLite database integration
- Unit tests for application functionality
- Containerization using Docker

**Technologies Used**

- ASP.NET Core 6.0
- Blazor/Razor Pages for UI
- Entity Framework Core with SQLite
- Docker for containerization
- xUnit for unit testing
- Prerequisites
- .NET 6.0 SDK
- Docker
- Git

Run The Project
You will need the following tools:
•	Visual Studio 2022
•	.Net Core 8 or later
•	Docker Desktop
Installing
Follow these steps to get your development environment set up: (Before Run Start the Docker Desktop)
1.	Clone the repository
2.	Once Docker for Windows is installed, go to the Settings > Advanced option, from the Docker icon in the system tray, to configure the minimum amount of memory and CPU like so:
•	Memory: 4 GB
•	CPU: 2
3.	At the root directory of solution, select docker-compose and Set a startup project. Run docker-compose without debugging on visual studio. Or you can go to root directory which include docker-compose.yml files, run below command:
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
4.	Wait for docker compose all microservices. That’s it! (some microservices need extra time to work so please wait if not worked in first shut)
5.	Launch Shopping Web UI -> https://localhost:32825/ in your browser to view index page. 

![Screenshot 2024-09-17 185035](https://github.com/user-attachments/assets/42355b6c-b61c-4689-820b-9709582c80e2)

![Screenshot 2024-09-17 185059](https://github.com/user-attachments/assets/4afa6acf-8eb0-4fb6-8ffe-0a96de3da2d3)
