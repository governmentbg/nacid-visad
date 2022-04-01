using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Diplomas;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Entities
{
	public class UpdateDiplomaCommand : IRequest<Unit>
	{
		public DiplomaDto Model { get; set; }

		public int PartId { get; set; }

		public class Handler : IRequestHandler<UpdateDiplomaCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(UpdateDiplomaCommand request, CancellationToken cancellationToken)
			{
				var part = await this.context.Set<DiplomaPart>()
					.Include(e => e.Entity)
						.ThenInclude(e => e.DiplomaFiles)
							.ThenInclude(e => e.DiplomaDocumentFiles)
					.Include(e => e.Entity)
						.ThenInclude(e => e.AttachedFiles)
					.SingleOrDefaultAsync(e => e.Id == request.PartId);

				part.Entity.Description = request.Model.Description;

				var filesForAdd = request.Model.DiplomaFiles.Where(e => !part.Entity.DiplomaFiles.Select(t => t.Id).Contains(e.Id));
				foreach (var diploma in filesForAdd)
				{
					foreach (var file in diploma.AttachedFiles)
					{
						file.Type = DiplomaDocumentType.OtherDocument;
					}

					diploma.DiplomaDocumentFile.Type = DiplomaDocumentType.Diploma;
					diploma.AttachedFiles.Add(diploma.DiplomaDocumentFile);

					part.Entity.AddFile(diploma.DiplomaNumber, diploma.IssuedDate, diploma.Country.Id, diploma.City, diploma.OrganizationName, diploma.Type.Id, diploma.AttachedFiles);
				}

				var existingNacidRecommendation = part.Entity.AttachedFiles
						.SingleOrDefault(e => e.DiplomaId == request.Model.Id && e.Type == DiplomaDocumentType.NacidRecommendation);

				if (existingNacidRecommendation != null && request.Model.NacidRecommendation == null)
				{
					this.context.Set<DiplomaDocumentFile>().Remove(existingNacidRecommendation);
				}
				else if (request.Model.NacidRecommendation != null)
				{
					if (existingNacidRecommendation == null)
					{
						request.Model.NacidRecommendation.Type = DiplomaDocumentType.NacidRecommendation;
						part.Entity.AddAttachedFile(request.Model.NacidRecommendation.Key, request.Model.NacidRecommendation.Hash, request.Model.NacidRecommendation.Size,
							request.Model.NacidRecommendation.Name, request.Model.NacidRecommendation.MimeType, request.Model.NacidRecommendation.DbId, request.Model.NacidRecommendation.Type);
					}
					else
					{
						part.Entity.UpdateAttachedFile(request.Model.NacidRecommendation.Key, request.Model.NacidRecommendation.Hash, request.Model.NacidRecommendation.Size,
							request.Model.NacidRecommendation.Name, request.Model.NacidRecommendation.MimeType, request.Model.NacidRecommendation.DbId, request.Model.NacidRecommendation.Type);
					}
				}

				if (request.Model.RectorDecisionDocumentFile != null)
				{
					part.Entity.UpdateAttachedFile(request.Model.RectorDecisionDocumentFile.Key, request.Model.RectorDecisionDocumentFile.Hash, request.Model.RectorDecisionDocumentFile.Size,
					request.Model.RectorDecisionDocumentFile.Name, request.Model.RectorDecisionDocumentFile.MimeType, request.Model.RectorDecisionDocumentFile.DbId, request.Model.RectorDecisionDocumentFile.Type);
				}

				var filesForUpdate = request.Model.DiplomaFiles.Where(e => part.Entity.DiplomaFiles.Select(t => t.Id).Contains(e.Id));
				foreach (var diploma in filesForUpdate)
				{
					if (diploma.Id == 0)
					{
						break;
					}

					var existingAttachedFile = part.Entity.DiplomaFiles
						.Where(x => x.Id == diploma.Id)
						.SelectMany(x => x.DiplomaDocumentFiles)
						.ToList();

					this.context.Set<DiplomaDocumentFile>().RemoveRange(existingAttachedFile);

					diploma.DiplomaDocumentFile.Type = DiplomaDocumentType.Diploma;
					diploma.AttachedFiles.Add(diploma.DiplomaDocumentFile);

					foreach (var attachedFile in diploma.AttachedFiles.Where(x => x.Type != DiplomaDocumentType.Diploma))
					{
						attachedFile.Type = DiplomaDocumentType.OtherDocument;
					}

					var diplomaFile = part.Entity.DiplomaFiles.SingleOrDefault(x => x.Id == diploma.Id);

					foreach (var attachedFile in diploma.AttachedFiles)
					{
						diplomaFile.AddDiplomaFile(attachedFile.Key, attachedFile.Hash, attachedFile.Size, attachedFile.Name, attachedFile.MimeType, attachedFile.DbId, attachedFile.Type);
					}

					part.Entity.UpdateFile(diploma.Id, diploma.DiplomaNumber, diploma.IssuedDate, diploma.Country.Id, diploma.City, diploma.OrganizationName, diploma.Type.Id);
				}

				var filesForRemove = part.Entity.DiplomaFiles.Where(e => !request.Model.DiplomaFiles.Select(t => t.Id).Contains(e.Id));
				foreach (var diploma in filesForRemove)
				{
					part.Entity.RemoveFile(diploma.Id);
				}

				await this.context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
