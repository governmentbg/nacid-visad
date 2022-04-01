using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VisaD.Application.WordProcessing.Services
{
    public class WordProcessingService : IWordProcessingService
    {
		public MemoryStream PopulateTemplate(byte[] content, object data)
		{
			using (var stream = new MemoryStream())
			{
				stream.Write(content, 0, content.Length);

				using (WordprocessingDocument doc = WordprocessingDocument.Open(stream, true))
				{
					var sdtElements = doc.MainDocumentPart.Document.Body.Descendants<SdtElement>().ToList();
					foreach (var el in sdtElements)
					{
						var tag = el.SdtProperties.GetFirstChild<Tag>();
						if (tag == null)
							continue;

						var val = data.GetType().GetProperty(tag.Val).GetValue(data, null) as string;
						el.Descendants<Text>().First().Text = val;

						IEnumerable<OpenXmlElement> elements = null;
						if (el is SdtBlock)
							elements = (el as SdtBlock).SdtContentBlock.Elements();
						else if (el is SdtCell)
							elements = (el as SdtCell).SdtContentCell.Elements();
						else if (el is SdtRun)
							elements = (el as SdtRun).SdtContentRun.Elements();

						foreach (var innerEl in elements)
							el.InsertBeforeSelf(innerEl.CloneNode(true));
						el.Remove();
					}
				}

				return new MemoryStream(stream.ToArray());
			}
		}
	}
}
