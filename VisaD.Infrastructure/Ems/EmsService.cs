using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisaD.Infrastructure.Ems.Models;

namespace VisaD.Infrastructure.Ems
{
	public class EmsService
	{
		private readonly HttpClient httpClient;

		public EmsService(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public async Task<Guid> SubmitApplicationAsync(string applicationContent)
		{
			var response = await httpClient.PostAsync("api/Portal/Application", new StringContent(applicationContent, Encoding.UTF8, "application/json"));

			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			var output = JsonConvert.DeserializeObject<EmsDocGuidOutput>(responseContent);
			return output.DocumentGuid;
		}

		public async Task<T> GetEmsStructuredDataWithInformationAsync<T>(int docId)
			where T: class
		{
			var response = await httpClient.GetAsync($"api/ElectronicDocument/byDocId/{docId}/ApplicationInformation");
			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			var data = JsonConvert.DeserializeObject<T>(responseContent);
			return data;
		}

		public async Task<EmsDocStatusResponse> GetEmsApplicationStatus(Guid docGuid)
		{
			var response = await httpClient.GetAsync($"api/Portal/Application/Status/{docGuid}");
			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			var output = JsonConvert.DeserializeObject<EmsDocStatusResponse>(responseContent);
			return output;
		}

		public async Task<T> GetCase<T>(string docNumber, string accessCode)
			where T : class
		{
			var response = await httpClient.GetAsync($"api/Portal/Case?documentNumber={docNumber}&accessCode={accessCode}");
			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<T>(responseContent);
			return result;
		}
	}
}
