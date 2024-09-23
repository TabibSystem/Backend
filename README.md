# Tabib

Tabib is a healthcare management system built using **ASP.NET Core**. It provides functionalities for managing **clinics**, **doctors**, **patients**, and **appointments**, along with real-time chat and prescription management.

<div align="center">

  <img src="13rC-_1_.png" alt="Project Structure" width="400" height="300"/>

</div>

## System Architecture

The **Tabib** system is built using ASP.NET Core with Identity for user management, handling roles such as **Admin**, **Doctor**, and **Patient**. It follows **Clean Architecture** principles to keep the system well-structured, scalable, and easy to maintain.

### Key Benefits of Clean Architecture for **Tabib**:

- **Separation of Concerns**: Each layer focuses on a specific responsibility, making it easier to manage changes. For example, changes in the database won’t affect the core business logic.
    
- **Testability**: By isolating business logic, it becomes easier to write unit tests for core features like **appointment booking** or **prescription management** without relying on the database or external systems.
    
- **Scalability**: The system is designed to grow, allowing the easy addition of new features like **telemedicine** or **AI-based recommendations**.

The **Tabib** project is structured as follows:

- **Domain Layer**: Handles business logic for core features like **appointments**, **clinics**, **patients**, **prescriptions**, and **medical records**.

- **Application Layer**: Contains services for handling operations like booking appointments, managing clinics, and creating prescriptions, along with interfaces for repository access.

- **Infrastructure Layer**: Manages database interactions with **Entity Framework Core**, handles user authentication with **ASP.NET Core Identity**, and integrates **JWT** for secure API access.

- **Presentation Layer**: The API acts as the **Presentation Layer**. It provides endpoints for user interaction, enabling patients and doctors to interact with the system through the API.
-

## API Documentation

**Swagger** and XML comments provide API documentation. Key endpoints:

- `POST /api/account/register`: Register a new user (Doctor, Patient, or Assistant).
- `GET /api/appointments`: Get user appointments (filterable by status and date).
- `POST /api/appointments/book`: Book an appointment slot.
- `GET /api/clinics/{id}`: Get clinic details and available doctors.
- `POST /api/chats`: Initiate real-time chats.

## Database Design

The database structure in **Tabib** is designed using **Entity Framework Core**, ensuring efficient data management and relationships 

## Integration

- **Identity with JWT Authentication**: Ensures secure user management and API access.
- **SignalR for Real-Time Communication**: Facilitates real-time chat between doctors and patients for improved user engagement.

## Future Enhancements

Planned features include:

- **Telemedicine**: Video consultations between doctors and patients.
- **Push Notifications**: Alerts for upcoming appointments, chat messages, and medical reminders.
- **Mobile Apps**: Android and iOS apps for easy access to key features.

## Backend Testing

The system uses **xUnit** for testing:

- **Unit Testing**: Core functionalities (appointment booking, registration, authentication) are tested using **xUnit** with mock dependencies to ensure business logic correctness.
### Public API Access

Use this link to test and interact with the API. 
[[http://tabib.runasp.net/swagger/index.html]]
