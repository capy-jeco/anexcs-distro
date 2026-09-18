using FluentValidation;

namespace Anexcs.Distro.Application.Tenants.Commands.CreateTenant;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.InitialDomain)
            .NotEmpty()
            .WithMessage("Initial domain is required.");
    }
}