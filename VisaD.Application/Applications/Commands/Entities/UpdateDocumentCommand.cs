using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Entities
{
	public class UpdateDocumentCommand : IRequest<Unit>
	{
		public DocumentDto Model { get; set; }
		public int PartId { get; set; }

		public class Handler : IRequestHandler<UpdateDocumentCommand, Unit>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<Unit> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
			{
				var part = await context.Set<DocumentPart>()
					.Include(e => e.Entity)
						.ThenInclude(e => e.Files)
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				part.Entity.Description = request.Model.Description;

				var filesForAdd = request.Model.Files.Where(e => !part.Entity.Files.Select(t => t.Id).Contains(e.Id));
				foreach(var fileForAdd in filesForAdd)
				{
					part.Entity.AddFile(
						fileForAdd.Type?.Id,
						fileForAdd.AttachedFile.Key,
						fileForAdd.AttachedFile.Hash,
						fileForAdd.AttachedFile.Size,
						fileForAdd.AttachedFile.Name,
						fileForAdd.AttachedFile.MimeType,
						fileForAdd.AttachedFile.DbId,
						fileForAdd.FileDescription
					);
				}

				var filesForUpdate = request.Model.Files.Where(e => part.Entity.Files.Select(t => t.Id).Contains(e.Id));
				foreach(var fileForUpdate in filesForUpdate)
				{
					part.Entity.UpdateFile(
						fileForUpdate.Id,
						fileForUpdate.Type?.Id,
						fileForUpdate.AttachedFile.Key,
						fileForUpdate.AttachedFile.Hash,
						fileForUpdate.AttachedFile.Size,
						fileForUpdate.AttachedFile.Name,
						fileForUpdate.AttachedFile.MimeType,
						fileForUpdate.AttachedFile.DbId,
						fileForUpdate.FileDescription
					);
				}

				var filesForRemove = part.Entity.Files.Where(e => !request.Model.Files.Select(t => t.Id).Contains(e.Id));
				foreach(var fileForRemove in filesForRemove)
				{
					part.Entity.RemoveFile(fileForRemove.Id);
				}

				await context.SaveChangesAsync(cancellationToken);

				return Unit.Value;
			}
		}
	}
}
