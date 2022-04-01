using System;
using System.Collections.Generic;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications.Diplomas;
using VisaD.Data.Nomenclatures;

namespace VisaD.Application.Applications.Dtos
{
	public class DiplomaFileDto
	{
		public int Id { get; set; }
		public string DiplomaNumber { get; set; }

		public DateTime IssuedDate { get; set; }

		public NomenclatureDto<Country> Country { get; set; }
		public string City { get; set; }

		public string OrganizationName { get; set; }

		public DiplomaTypeNomenclatureDto Type { get; set; }

		public DiplomaDocumentFile DiplomaDocumentFile { get; set; }

		public List<DiplomaDocumentFile> AttachedFiles { get; set; } = new List<DiplomaDocumentFile>();
	}
}
