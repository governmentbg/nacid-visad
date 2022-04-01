using iText.Kernel.Geom;

namespace VisaD.Application.Common.Models
{
	public class PdfTextChunk
	{
		public string Text { get; set; }
		public Rectangle Rect { get; set; }
		public string FontFamily { get; set; }
		public int FontSize { get; set; }
		public float SpaceWidth { get; set; }
	}
}
