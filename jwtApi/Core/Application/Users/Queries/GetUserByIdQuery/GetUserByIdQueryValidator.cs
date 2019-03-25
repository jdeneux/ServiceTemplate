using FluentValidation;

namespace jwtApi.Core.Application.Users.Queries.GetUserByIdQuery
{
    public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(v => v.Id).GreaterThanOrEqualTo(0);
        }
    }
}
