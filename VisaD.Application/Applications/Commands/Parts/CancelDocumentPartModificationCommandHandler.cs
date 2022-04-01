using Microsoft.EntityFrameworkCore;
using System.Linq;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Register.Commands;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Register;

namespace VisaD.Application.Applications.Commands.Parts
{
	public class CancelDocumentPartModificationCommandHandler : CancelPartModificationCommandHandler<ApplicationCommit, DocumentPart, Document>
	{
		public CancelDocumentPartModificationCommandHandler(IAppDbContext context)
			: base(context)
		{
		}

		protected override IQueryable<DocumentPart> LoadPart()
		{
			return context.Set<DocumentPart>()
				.Include(e => e.Entity)
					.ThenInclude(e => e.Files);
		}
	}
}
