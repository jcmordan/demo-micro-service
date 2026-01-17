# Notification Service TODOs

## Responsibilities
- [ ] Send notifications to users (Console or Email).
- [ ] **Validation**: Before sending, check if the user exists (communicate with `UserService`).
- [ ] Support different types of notifications (Booking confirmed, Payment received, etc.).

## Implementation Notes
- Requires JWT Authentication.
- Validate that the token was issued by `UserService`.
- Consider using an asynchronous approach (e.g., RabbitMQ or Kafka) for receiving notification requests.
