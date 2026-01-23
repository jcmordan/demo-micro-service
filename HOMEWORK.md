# 📚 Homework: Migrating Monolith to Microservices

## 🎯 Objective
The goal of this assignment is to refactor a monolithic application into a decoupled microservices architecture. You will extract business logic and infrastructure layers from the existing monolith and implement them into the provided microservices skeleton.

## 🏗 Project Context
The system consists of three main repositories:
- **Monolith (Source)**: [demo-monolitic](https://github.com/jcmordan/demo-monolitic) - Contains the full implementation in a single API.
- **Microservices (Target)**: [demo-micro-service](https://github.com/jcmordan/demo-micro-service) - Contains the `UserService` (Auth) and API skeletons for other services.
- **Frontend**: [demo-web](https://github.com/jcmordan/demo-web) - A React application that can interact with either backend.

## 📋 Task Details
As a student, you must:
1. **Analyze** the assigned feature in the `demo-monolitic` repository.
2. **Implement the Core/Domain Layer** in the corresponding microservice:
   - Define Entities (clean of monolithic dependencies).
   - Define DTOs for API communication.
   - Define Service Interfaces.
   - Implement the business logic (Services).
3. **Implement the Infrastructure Layer**:
   - Create a dedicated `DbContext`.
   - Implement Repositories.
4. **Wire up the API**:
   - Register dependencies in `Program.cs`.
   - Implement/Update Controllers to use the new Services.
   - Ensure JWT Authentication is working (trusted from `UserService`).

## 👥 Teams and Assignments

| Team | Assigned Service | Repository Folder |
| :--- | :--- | :--- |
| **Eduardo / Darlin** | 📅 Booking | `/BookingService` |
| **Adrian & Enrique** | 🏨 Room | `/RoomService` |
| **Jonas & Joaquin** | 💳 Payment | `/PaymentService` |
| **Luis & Idaris** | 🔔 Notification | `/NotificationService` |

## 🏁 Verification Goal
The ultimate success criteria is demonstrating that the **demo-web** application continues to function perfectly when pointed at your microservice backend, just as it did with the monolith. This proves that your API contract remains consistent.

## 💡 Technical Tips
- Use **In-Memory Database** for simplicity during development.
- Refer to `UserService` in the microservices repository as a template for the 3-layer architecture (Core, Infrastructure, Api).
- Check the `launchSettings.json` in each API project to manage local ports.

## ⚠️ Important Notes
- **Keep it Simple**: Avoid overcomplicating the implementation. No complex validations are required for this exercise.
- **Use of AI**: Avoid the overuse of AI for code generation. The goal of this assignment is for you to understand the foundations of microservices architecture. The best way to learn is to implement the solution yourself.

---
**Happy Coding! 🚀**
