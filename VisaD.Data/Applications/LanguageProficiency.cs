using System;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Nomenclatures;

namespace VisaD.Data.Applications
{
	public class LanguageProficiency : IEntity, IAuditable
	{
		public int Id { get; private set; }

		public int TrainingId { get; private set; }
		public virtual Training Training { get; private set; }

		public int LanguageId { get; private set; }
		public Language Language { get; private set; }

		public int ReadingId { get; set; }

		public LanguageDegree Reading { get; set; }


		public int WritingId { get; set; }

		public LanguageDegree Writing { get; set; }

		public int SpeakingId { get; set; }

		public LanguageDegree Speaking { get; set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }

		private LanguageProficiency()
		{

		}

		public LanguageProficiency(int languageId, int readingId, int writingId, int speakingId)
		{
			this.LanguageId = languageId;
			this.ReadingId = readingId;
			this.WritingId = writingId;
			this.SpeakingId = speakingId;
		}

		public void Update(int languageId, int readingId, int writingId, int speakingId)
		{
			this.LanguageId = languageId;
			this.ReadingId = readingId;
			this.WritingId = writingId;
			this.SpeakingId = speakingId;
		}
	}
}
