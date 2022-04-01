using QRCoder;
using System;

namespace VisaD.Application.Common.Services
{
	public class QrCodeService
	{
		public string Create(string url)
		{
			var qrCodeGenerator = new QRCodeGenerator();
			var qrCodeData = qrCodeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
			var pngByteQRCode = new PngByteQRCode(qrCodeData);
			var qrCodeAsPngByteArr = pngByteQRCode.GetGraphic(20);
			var base64Image = Convert.ToBase64String(qrCodeAsPngByteArr);

			return base64Image;
		}
	}
}
