# Payment Service TODOs

## Responsibilities
- [ ] Process payments.
- [ ] **Validation**: Before payment, check if the user exists (communicate with `UserService`).
- [ ] **Validation**: Before payment, check if the booking exists (communicate with `BookingService`).
- [ ] Update payment status (Pending, Completed, Failed).

## Implementation Notes
- Requires JWT Authentication.
- Validate that the token was issued by `UserService`.
- Ensure idempotency in payment processing.
