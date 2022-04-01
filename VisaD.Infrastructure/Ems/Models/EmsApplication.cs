using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using VisaD.Data.Applications.Register;

namespace VisaD.Infrastructure.Ems.Models
{
    public class EmsApplication
    {
        public string ElectronicServiceUri { get; set; }
        public string ServiceCode { get; set; }
        public string ParentDocNumber { get; set; }
        public int PaymentTypeId { get; set; }
        public bool UseEmailForCorrespondence { get; set; }
        public EmsCorrespondent Applicant { get; set; }
        public JObject StructuredData { get; set; }
        public string ExternalNumber { get; set; }
        public DateTime? ExternalNumberDate { get; set; }
        public ICollection<ApplicationLotResultFile> AttachedFiles { get; set; }
        public bool SkipConfirmationDoc { get; set; }
        public bool SkipConfirmationEmail { get; set; }

    }
}
