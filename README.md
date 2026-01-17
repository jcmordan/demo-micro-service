# demo-micro-service

# Booking Application - Microservices Architecture

This project is a microservices-based version of the Booking Application. It is divided into four independent services, each with specific responsibilities.

## Microservices Overview

### 1. User Service (`UserService.Api`)
- **Responsibility**: Handle user registration and authentication.
- **Role**: Identity Provider. Issues JWT tokens that all other services trust.

### 2. Booking Service (`BookingService.Api`)
- **Responsibility**: Handle room management and bookings.
- **Validation**: Ensures rooms exist and coordinates with User Service to validate users.

### 3. Notification Service (`NotificationService.Api`)
- **Responsibility**: Handle system notifications.
- **Validation**: Coordinates with User Service to ensure the recipient user exists.

### 4. Payment Service (`PaymentService.Api`)
- **Responsibility**: Handle payment processing.
- **Validation**: Coordinates with User Service (user existence) and Booking Service (booking existence).

## Security
All services support JWT Authentication. Each service is configured to validate tokens issued by the `UserService`.

## Getting Started
Each service contains a `TODO.md` file with detailed implementation requirements for students.