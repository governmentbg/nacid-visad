using System;
using VisaD.Data.Applications.Enums;
using VisaD.Data.Common.Interfaces;
using VisaD.Data.Common.Models;

namespace VisaD.Data.Applications.Register
{
	public class ApplicationLot : Lot<ApplicationCommit>, IAuditable
	{
		public ApplicationLotResult Result { get; set; }

		public DateTime CreateDate { get; set; }
		public int CreatorUserId { get; set; }

		public ApplicationLotResult AddResult(ApplicationLotResultType type, string note, string certificateNumber, string accessCode, int? regulationId)
		{
			var result = new ApplicationLotResult(type, note, certificateNumber, accessCode, regulationId);
			this.Result = result;

			return result;
		}

		public void UpdateResult(ApplicationLotResultType type, string note, string certificateNumber, string accessCode, int regulationId)
		{
			this.Result.Update(type, note, certificateNumber, accessCode, regulationId);
		}
	}
}
