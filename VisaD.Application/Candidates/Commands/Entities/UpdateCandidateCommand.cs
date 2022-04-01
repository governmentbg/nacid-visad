using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Candidates.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Candidates.Register;

namespace VisaD.Application.Candidates.Commands.Entities
{
	public class UpdateCandidateCommand : IRequest<Unit>
	{
		public CandidateDto Model { get; set; }
		public int PartId { get; set; }

		public class Handler : IRequestHandler<UpdateCandidateCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
			{
				var part = await context.Set<CandidatePart>()
					.Include(e => e.Entity)
						.ThenInclude(x => x.OtherNationalities)
					.Include(e => e.Entity)
						.ThenInclude(x => x.CandidatePassportDocument)
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				part.Entity.Update(request.Model.FirstName, request.Model.LastName, request.Model.BirthDate, request.Model.BirthPlace, request.Model.Nationality.Id, request.Model.PassportNumber, request.Model.PassportValidUntil,
					request.Model.Country.Id, request.Model.Phone, request.Model.Mail, request.Model.ImgFile.Key, request.Model.ImgFile.Hash, request.Model.ImgFile.Size,
					request.Model.ImgFile.Name, request.Model.ImgFile.MimeType, request.Model.ImgFile.DbId, request.Model.OtherNames,
					request.Model.FirstNameCyrillic, request.Model.LastNameCyrillic, request.Model.OtherNamesCyrillic);

				part.Entity.UpdateFile(request.Model.Document.AttachedFile.Key, request.Model.Document.AttachedFile.Hash, request.Model.Document.AttachedFile.Size,
					request.Model.Document.AttachedFile.Name, request.Model.Document.AttachedFile.MimeType, request.Model.Document.AttachedFile.DbId);

				var nationalitiesForAdd = request.Model.OtherNationalities.Where(cn => !part.Entity.OtherNationalities.Any(x => x.NationalityId == cn.Id)).ToList();
				if (nationalitiesForAdd.Any())
				{
					foreach (var nationality in nationalitiesForAdd)
					{
						part.Entity.AddNationality(nationality.Id);
					}
				}

				var nationalitiesForRemove = part.Entity.OtherNationalities.Where(x => !request.Model.OtherNationalities.Any(y => y.Id == x.NationalityId)).ToList();
				if (nationalitiesForRemove.Any())
				{
					foreach (var nationalityForRemove in nationalitiesForRemove)
					{
						part.Entity.RemoveNationality(nationalityForRemove.NationalityId);
					}
				}

				await context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
