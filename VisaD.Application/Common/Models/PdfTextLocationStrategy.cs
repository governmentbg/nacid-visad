using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System.Collections.Generic;

namespace VisaD.Application.Common.Models
{
	public class PdfTextLocationStrategy : LocationTextExtractionStrategy
	{
		public List<PdfTextChunk> ObjectResult = new List<PdfTextChunk>();

		public override void EventOccurred(IEventData data, EventType type)
		{
			if (!type.Equals(EventType.RENDER_TEXT))
				return;

			TextRenderInfo renderInfo = (TextRenderInfo)data;

			string curFont = renderInfo.GetFont().GetFontProgram().ToString();

			float curFontSize = renderInfo.GetFontSize();

			IList<TextRenderInfo> text = renderInfo.GetCharacterRenderInfos();
			foreach (TextRenderInfo t in text)
			{
				string letter = t.GetText();
				Vector letterStart = t.GetBaseline().GetStartPoint();
				Vector letterEnd = t.GetAscentLine().GetEndPoint();
				Rectangle letterRect = new Rectangle(letterStart.Get(0), letterStart.Get(1), letterEnd.Get(0) - letterStart.Get(0), letterEnd.Get(1) - letterStart.Get(1));

				if (letter != " " && !letter.Contains(" "))
				{
					var chunk = new PdfTextChunk();
					chunk.Text = letter;
					chunk.Rect = letterRect;
					chunk.FontFamily = curFont;
					chunk.FontSize = (int)curFontSize;
					chunk.SpaceWidth = t.GetSingleSpaceWidth() / 2f;

					ObjectResult.Add(chunk);
				}
			}
		}
	}
}
