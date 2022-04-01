using FileStorageNetCore.Models;
using VisaD.Application.Nomenclatures.Dtos;

namespace VisaD.Application.Applications.Dtos
{
	public class ApplicationFileDto
	{
		public int Id { get; set; }

		public ApplicationFileTypeNomenclatureDto Type { get; set; }

		public AttachedFile AttachedFile { get; set; }

		public string FileDescription { get; set; }
	}
}
