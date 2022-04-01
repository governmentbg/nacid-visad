using System.Collections.Generic;
using VisaD.Data.Applications;
using VisaD.Data.Applications.Diplomas;
using VisaD.Data.Applications.Enums;

namespace VisaD.Application.Applications.Dtos
{
	public class DiplomaDto
	{
		public int Id { get; set; }

		public IEnumerable<DiplomaFileDto> DiplomaFiles { get; set; } = new List<DiplomaFileDto>();

		public DiplomaDocumentFile RectorDecisionDocumentFile { get; set; }

		public DiplomaDocumentFile NacidRecommendation { get; set; }

		public string Description { get; set; }

		public Diploma ToModel()
		{
			var diploma = new Diploma();
			diploma.Description = this.Description;

			//Init list to add attached files from diploma dto
			var diplomaDocumentFiles = new List<DiplomaDocumentFile>();

			//Add attached files from diploma dto
			if (this.RectorDecisionDocumentFile != null)
			{
				this.RectorDecisionDocumentFile.Type = DiplomaDocumentType.RectorDecision;
				diplomaDocumentFiles.Add(this.RectorDecisionDocumentFile);
			}

			if (this.NacidRecommendation != null)
			{
				this.NacidRecommendation.Type = DiplomaDocumentType.NacidRecommendation;
				diplomaDocumentFiles.Add(this.NacidRecommendation);
			}

			foreach (var attachedFile in diplomaDocumentFiles)
			{
				diploma.AddAttachedFile(attachedFile.Key, attachedFile.Hash, attachedFile.Size, attachedFile.Name, attachedFile.MimeType, attachedFile.DbId, attachedFile.Type);
			}

			foreach (var item in this.DiplomaFiles)
			{
				foreach (var file in item.AttachedFiles)
				{
					file.Type = DiplomaDocumentType.OtherDocument;
				}

				item.DiplomaDocumentFile.Type = DiplomaDocumentType.Diploma;
				item.AttachedFiles.Add(item.DiplomaDocumentFile);

				diploma.AddFile(item.DiplomaNumber, item.IssuedDate, item.Country.Id, item.City, item.OrganizationName, item.Type.Id, item.AttachedFiles);
			}

			return diploma;
		}
	}
}
