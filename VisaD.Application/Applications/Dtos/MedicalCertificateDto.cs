using FileStorageNetCore.Models;
using System;
using VisaD.Application.Nomenclatures.Dtos;
using VisaD.Data.Applications;

namespace VisaD.Application.Applications.Dtos
{
	public class MedicalCertificateDto
	{
		public int Id { get; set; }

		public AttachedFile File { get; set; }

		public DateTime IssuedDate { get; set; }

		public MedicalCertificate ToModel()
		{
			var medicalCertificate = new MedicalCertificate(this.File.Key, this.File.Hash, this.File.Size, this.File.Name, this.File.MimeType, this.File.DbId, this.IssuedDate);

			return medicalCertificate;
		}
	}
}
