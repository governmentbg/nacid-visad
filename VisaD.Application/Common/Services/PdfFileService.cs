using HandlebarsDotNet;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Common.Models;

namespace VisaD.Application.Common.Services
{
	public class PdfFileService : IPdfFileService
	{
		//PUPPETEER VERSION 2.0.0 is the only working version!

		public async Task<MemoryStream> GeneratePdfFile<T>(T payload, byte[] content, bool closeStream = true)
		{
			var outputStream = new MemoryStream();
			string template = Encoding.UTF8.GetString(content, 0, content.Length);
			var templateFunc = Handlebars.Compile(template);

			var fileContent = templateFunc.Invoke(payload);
			await this.Convert(fileContent, outputStream, "\\VisaD-chromium");

			if(closeStream)
			{
				outputStream.Close();
				outputStream.Dispose();
			}

			return outputStream;
		}

		public async Task<byte[]> GenerateSignedPdfFile<T>(T payload, byte[] content, PdfSignFieldSettings signFieldSettings)
		{
			using (var pdfStream = await GeneratePdfFile(payload, content, false))
			{
				pdfStream.Position = 0;

				if (!string.IsNullOrWhiteSpace(signFieldSettings.FieldName))
				{
					var coordinates = GetTextCoordinates(pdfStream.ToArray(), signFieldSettings.FieldName);
					var resultPdf = CreateSignatureField(pdfStream.ToArray(), coordinates, signFieldSettings.Width, signFieldSettings.Height, signFieldSettings.Margin);
					return resultPdf.ToArray();
				}

				return pdfStream.ToArray();
			}
		}

		private List<PdfObjectAppearance> GetTextCoordinates(byte[] pdfContent, string keyword)
		{
			using (var pdfStream = new MemoryStream(pdfContent))
			using (var pdfReader = new PdfReader(pdfStream))
			using (var pdfDocument = new PdfDocument(pdfReader))
			{
				var coordinates = new List<PdfObjectAppearance>();
				FilteredEventListener listener = new FilteredEventListener();
				var strat = listener.AttachEventListener(new PdfTextLocationStrategy());
				PdfCanvasProcessor processor = new PdfCanvasProcessor(listener);

				Dictionary<int, string> pages = new Dictionary<int, string>();
				int previousStartIndex = 0;
				for (var i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
				{
					var sb = new StringBuilder();
					var page = pdfDocument.GetPage(i);
					processor.ProcessPageContent(page);
					foreach (var el in strat.ObjectResult)
					{
						sb.Append(el.Text);
					}

					string text = sb.ToString().Substring(previousStartIndex);
					pages.Add(i, text);
					previousStartIndex += text.Length - 1;
				}

				int beforeLength = 0;
				foreach (var page in pages)
				{
					int index = page.Value.IndexOf(keyword);
					while (index >= 0)
					{
						var appearance = GetAppearance(strat.ObjectResult, beforeLength + index, keyword.Length);
						appearance.Page = page.Key;
						appearance.Name = page.Value.Substring(index, keyword.Length);
						coordinates.Add(appearance);

						index = page.Value.IndexOf(keyword, index + 1);
					}

					beforeLength += page.Value.Length - 1;
				}

				return coordinates;
			}
		}

		private PdfObjectAppearance GetAppearance(List<PdfTextChunk> texts, int from, int to)
		{
			var lx = texts[from].Rect.GetLeft();
			var rx = texts[from + to - 1].Rect.GetRight();
			var ty = texts.GetRange(from, to).Max(e => e.Rect.GetTop());
			var by = texts.GetRange(from, to).Min(e => e.Rect.GetBottom());

			var signatureApperance = new PdfObjectAppearance {
				TextHeight = (int)(ty - by) + 1,
				TextWidth = (int)(rx - lx) + 1,
				LeftX = lx,
				BottomY = by
			};
			return signatureApperance;
		}

		private MemoryStream CreateSignatureField(byte[] pdfContent, List<PdfObjectAppearance> coordinates, int width, int heigth, int margin = 0)
		{
			using (var pdfStream = new MemoryStream())
			{
				pdfStream.Write(pdfContent);
				pdfStream.Position = 0;

				using (var reader = new iTextSharp.text.pdf.PdfReader(pdfContent))
				using (var stamp = new iTextSharp.text.pdf.PdfStamper(reader, pdfStream))
				{
					foreach (var item in coordinates)
					{
						var field = iTextSharp.text.pdf.PdfFormField.CreateSignature(stamp.Writer);
						field.SetWidget(new iTextSharp.text.Rectangle(item.LeftX - margin, item.BottomY + item.TextHeight - heigth,
							item.LeftX + width, item.BottomY + item.TextHeight + margin), iTextSharp.text.pdf.PdfAnnotation.HIGHLIGHT_OUTLINE);

						var image = iTextSharp.text.Image.GetInstance(new Bitmap(item.TextWidth, item.TextHeight + 10), iTextSharp.text.BaseColor.WHITE);
						image.SetAbsolutePosition(item.LeftX, item.BottomY - 10);
						stamp.GetOverContent(item.Page).AddImage(image, true);

						field.FieldName = item.Name;
						field.Flags = iTextSharp.text.pdf.PdfAnnotation.FLAGS_PRINT;
						stamp.AddAnnotation(field, item.Page);
					}

					return pdfStream;
				}
			}
		}
		private async Task<byte[]> Convert(string content, Stream outputStream, string browserFetcherPath)
		{
			var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions {
				Path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + browserFetcherPath
			});

			await browserFetcher.DownloadAsync(BrowserFetcher.DefaultRevision);

			using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions {
				Headless = true,
				ExecutablePath = browserFetcher.RevisionInfo(BrowserFetcher.DefaultRevision).ExecutablePath
			}))
			{
				var page = await browser.NewPageAsync();
				await page.SetContentAsync(content);

				var pdfStream = await page.PdfDataAsync();
				await outputStream.WriteAsync(pdfStream);
				return pdfStream;
			}
		}
	}
}
