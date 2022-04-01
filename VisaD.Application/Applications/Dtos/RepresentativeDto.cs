using System;
using System.Collections.Generic;
using VisaD.Data.Applications;
using VisaD.Data.Applications.AttachedFiles;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Enums;

namespace VisaD.Application.Applications.Dtos
{
    public class RepresentativeDto
    {
        public bool HasRepresentative { get; set; }
        public RepresentativeType? Type { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string IdentificationCode { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }

        public string Note { get; set; }

        public RepresentativeDocumentFile ApplicationForCertificate { get; set; }

        public RepresentativeDocumentFile LetterOfAttorney { get; set; }

        public DateTime SubmissionDate { get; set; }

        public Representative ToModel()
		{
            var representativeDocuments = new List<RepresentativeDocumentFile>();
            this.ApplicationForCertificate.Type = RepresentativeDocumentType.ApplicationForCertificate;
            representativeDocuments.Add(this.ApplicationForCertificate);

            if (this.LetterOfAttorney != null)
			{
                this.LetterOfAttorney.Type = RepresentativeDocumentType.LetterOfAttorney;
                representativeDocuments.Add(this.LetterOfAttorney);
			}

            return new Representative(this.HasRepresentative, this.Type, this.FirstName, this.LastName, this.IdentificationCode, this.Mail, this.Phone,
                this.Note, this.SubmissionDate, representativeDocuments);
        }
    }
}
