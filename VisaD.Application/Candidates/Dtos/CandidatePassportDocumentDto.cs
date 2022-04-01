using FileStorageNetCore.Models;
using VisaD.Application.Nomenclatures.Dtos;

namespace VisaD.Application.Candidates.Dtos
{
	public class CandidatePassportDocumentDto
	{
		public int Id { get; set; }

		public AttachedFile AttachedFile { get; set; }
	}
}
