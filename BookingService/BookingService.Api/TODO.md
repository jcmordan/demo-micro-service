# Booking Service TODOs

## Responsibilities
- [ ] Manage rooms (CRUD).
- [ ] Handle bookings.
- [ ] **Validation**: Before booking, check if the room exists.
- [ ] **Validation**: Before booking, check if the user exists (communicate with `UserService`).
- [ ] Implement check-in/check-out logic.

## Implementation Notes
- Requires JWT Authentication.
- Validate that the token was issued by `UserService`.
- May need to implement inter-service communication (REST or Message Broker).
