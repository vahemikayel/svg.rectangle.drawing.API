using FluentValidation;

namespace SVG.API.Application.Validations
{
    public class CRUDCommandValidator<TCommand> : AbstractValidator<TCommand>
        where TCommand : new()
    {
        public CRUDCommandValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
