using System.Collections.Generic;
using VisaD.Data.Applications;

namespace VisaD.Application.Applications.Dtos
{
	public class TrainingDto
	{
		public string LanguageDepartment { get; set; }
		public int? LanguageTrainingDuration { get; set; }

		public IEnumerable<LanguageProficiencyDto> LanguageProficiencies { get; set; } = new List<LanguageProficiencyDto>();

		public TrainingLanguageDocumentFile TrainingLanguageDocumentFile { get; set; }

		public Training ToModel()
		{
			var training = new Training(this.LanguageDepartment, this.LanguageTrainingDuration);

			if (this.TrainingLanguageDocumentFile != null)
			{
				training.AddFile(this.TrainingLanguageDocumentFile.Key, this.TrainingLanguageDocumentFile.Hash, this.TrainingLanguageDocumentFile.Size,
					this.TrainingLanguageDocumentFile.Name, this.TrainingLanguageDocumentFile.MimeType, this.TrainingLanguageDocumentFile.DbId);
			}

			foreach (var item in this.LanguageProficiencies)
			{
				training.AddProficiency(item.Language.Id, item.Reading.Id, item.Writing.Id, item.Speaking.Id);
			}

			return training;
		}
	}
}
