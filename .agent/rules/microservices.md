# Microservices Architectural Rules

## Database per Service Principle
- Each microservice MUST have its own dedicated database and `DbContext`.
- A service's `DbContext` MUST ONLY contain entities that are owned by that specific service.
- Direct database access to another service's tables/entities is STRICTLY FORBIDDEN.
- Any data required from another service (e.g., Room details for a Booking) MUST be retrieved via an HTTP call to that service's API.
- In-memory relationship navigations (e.g., `booking.Room.Name`) are not possible for external entities. These must be handled by DTO mapping after fetching data from the external service.

## Centralized Configuration
- All service URLs MUST be managed via the centralized `.env` file (under the `ServiceUrls` section) and accessed through the `AddServiceUrl` extension method.
- Use `HttpClient` for inter-service communication, configured with the base URLs from the centralized `ServiceUrls` mapping.
