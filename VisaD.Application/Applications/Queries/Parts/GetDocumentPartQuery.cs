using FileStorageNetCore.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisaD.Application.Applications.Dtos;
using VisaD.Application.Common.Dtos;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Queries.Parts
{
	public class GetDocumentPartQuery : IRequest<PartDto<DocumentDto>>
	{
		public int PartId { get; set; }

		public class Handler : IRequestHandler<GetDocumentPartQuery, PartDto<DocumentDto>>
		{
			private readonly IAppDbContext context;

			public Handler(IAppDbContext context)
			{
				this.context = context;
			}

			public async Task<PartDto<DocumentDto>> Handle(GetDocumentPartQuery request, CancellationToken cancellationToken)
			{
				var result = await context.Set<DocumentPart>()
					.AsNoTracking()
					.Select(e => new PartDto<DocumentDto> {
						Id = e.Id,
						Entity = new DocumentDto {
							Description = e.Entity.Description,
							AreIdenticalFiles = e.Entity.AreIdenticalFiles,
							Files = e.Entity.Files
									.Select(af => new ApplicationFileDto {
										Id = af.Id,
										FileDescription = af.FileDescription,
										Type = af.Type != null
											? new ApplicationFileTypeNomenclatureDto {
												Id = af.Type.Id,
												Name = af.Type.Name,
												HasDate = af.Type.HasDate,
												Description = af.Type.Description
											}
											: null,
										AttachedFile = new AttachedFile {
											Key = af.Key,
											Hash = af.Hash,
											Size = af.Size,
											Name = af.Name,
											MimeType = af.MimeType,
											DbId = af.DbId
										}
									})
						},
						State = e.State
					})
					.SingleOrDefaultAsync(e => e.Id == request.PartId, cancellationToken);

				return result;
			}
		}
	}
}
