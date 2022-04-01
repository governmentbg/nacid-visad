using System;
using System.Collections.Generic;
using System.Linq;
using VisaD.Data.Common.Interfaces;

namespace VisaD.Data.Applications
{
	public class Training : IEntity, IAuditable
	{
		public int Id { get; private set; }

		private HashSet<LanguageProficiency> _proficiencies = new HashSet<LanguageProficiency>();
		public IReadOnlyCollection<LanguageProficiency> Proficiencies => _proficiencies.ToList().AsReadOnly();

		public string LanguageDepartment { get; private set; }
		public int? LanguageTrainingDuration { get; private set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }

		public TrainingLanguageDocumentFile TrainingLanguageDocument { get; set; }

		public Training(string languageDepartment, int? languageTrainingDuration)
		{
			this.LanguageDepartment = languageDepartment;
			this.LanguageTrainingDuration = languageTrainingDuration;
		}

		public Training(Training training)
			: this(training.LanguageDepartment, training.LanguageTrainingDuration)
		{
			if (training.TrainingLanguageDocument != null)
			{
				this.AddFile(training.TrainingLanguageDocument.Key, training.TrainingLanguageDocument.Hash, training.TrainingLanguageDocument.Size, training.TrainingLanguageDocument.Name,
					training.TrainingLanguageDocument.MimeType, training.TrainingLanguageDocument.DbId);
			}

			foreach (var item in training.Proficiencies)
			{
				AddProficiency(item.LanguageId, item.ReadingId, item.WritingId, item.SpeakingId);
			}
		}

		public void Update(string languageDepartment, int? languageTrainingDuration)
		{
			this.LanguageDepartment = languageDepartment;
			this.LanguageTrainingDuration = languageTrainingDuration;
		}

		public LanguageProficiency AddProficiency(int languageId, int readingId, int writingId, int speakingId)
		{
			var proficiency = new LanguageProficiency(languageId, readingId, writingId, speakingId);
			this._proficiencies.Add(proficiency);

			return proficiency;
		}

		public LanguageProficiency UpdateProficiency(int id, int languageId, int readingId, int writingId, int speakingId)
		{
			var proficiency = this.Proficiencies.Single(e => e.Id == id);
			proficiency.Update(languageId, readingId, writingId, speakingId);

			return proficiency;
		}

		public void RemoveProficiency(int id)
		{
			var proficiency = this._proficiencies.Single(e => e.Id == id);
			this._proficiencies.Remove(proficiency);
		}

		public void UpdateFile(Guid key, string hash, long size, string name, string mimeType, int dbId)
		{
			this.TrainingLanguageDocument.Update(key, hash, size, name, mimeType, dbId);
		}

		public TrainingLanguageDocumentFile AddFile(Guid key, string hash, long size, string name, string mimeType, int dbId)
		{
			var document = new TrainingLanguageDocumentFile(key, hash, size, name, mimeType, dbId);
			this.TrainingLanguageDocument = document;

			return document;
		}
	}
}
