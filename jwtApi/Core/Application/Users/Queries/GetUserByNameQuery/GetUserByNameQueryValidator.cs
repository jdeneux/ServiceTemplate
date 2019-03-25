using FluentValidation;
using System;

namespace jwtApi.Core.Application.Users.Queries.GetUserByNameQuery
{
    public class GetUserByNameQueryValidator : AbstractValidator<GetUserByNameQuery>
    {
        public GetUserByNameQueryValidator()
        {
            RuleFor(v => v.Username).NotNull().NotEmpty();
        }
    }
}
