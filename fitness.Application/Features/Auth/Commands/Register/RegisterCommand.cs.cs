using fitness.Domain.Enums;
using MediatR;

namespace fitness.Application.Features.Auth.Commands.Register;

public class RegisterCommand : IRequest<string>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string Address { get; set; } = string.Empty;
}