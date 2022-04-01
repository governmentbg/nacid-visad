using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.DomainValidation;
using VisaD.Application.DomainValidation.Enums;
using VisaD.Data.Nomenclatures;
using VisaD.Data.Users;
using VisaD.Data.Users.Enums;

namespace VisaD.Application.Users.Commands
{
    public class CreateUserCommand : IRequest<int>
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }
        public string RoleAlias { get; set; }
        public Institution Institution { get; set; }
        public string Position { get; set; }
        

        public class Handler : IRequestHandler<CreateUserCommand, int>
        {
            private readonly IAppDbContext context;
            private readonly DomainValidationService validation;

            public Handler(
                IAppDbContext context, 
                DomainValidationService validation)
            {
                this.context = context;
                this.validation = validation;
            }

            public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                bool isEmailTaken = await context.Set<User>()
                    .AnyAsync(e => e.Email.Trim().ToLower() == request.Email.Trim().ToLower(), cancellationToken);

                if (isEmailTaken)
                {
                    this.validation.ThrowErrorMessage(UserErrorCode.User_EmailTaken);
                }

                request.Username = request.Email;

                var user = new User(request.Username, request.FirstName, request.MiddleName, request.LastName, request.Email, request.Phone, request.RoleId, request.Position, request.Institution?.Id);

                this.context.Set<User>().Add(user);
                await this.context.SaveChangesAsync(cancellationToken);

                return user.Id;
            }
        }
    }
}
