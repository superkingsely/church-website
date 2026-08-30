

public sealed record CreateMemberCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber);