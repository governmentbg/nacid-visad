using FileStorageNetCore.Api;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using VisaD.Application.Common.Interfaces;
using VisaD.Application.Ems.Models;
using VisaD.Data.Applications.Register;
using VisaD.Data.Nomenclatures;
using VisaD.Infrastructure.Ems.Models;

namespace VisaD.Application.Ems.Converters
{
	public class EmsApplicationConverter : IEmsApplicationConverter
	{
		private readonly IAppDbContext context;
		private readonly BlobStorageService fileStorage;
		private readonly EmsConfiguration emsConfiguration;

		public EmsApplicationConverter(IAppDbContext context, IOptions<EmsConfiguration> emsOptions, BlobStorageService fileStorage)
		{
			this.context = context;
			this.fileStorage = fileStorage;
			this.emsConfiguration = emsOptions.Value;
		}

		public EmsApplication ToEmsApplication(string electornicServiceUri, ApplicantPart model, string regNumber, ApplicationLotResultFile file, bool hasParent)
		{
			var institution = this.context.Set<Institution>().Where(i => i.Id == model.Entity.InstitutionId).SingleOrDefault();

			var correspondent = new EmsCorrespondent {
				FirstName = model.Entity.FirstName,
				MiddleName = model.Entity.MiddleName,
				LastName = model.Entity.LastName,
				Name = institution.Name,

				CorrespondentContacts = new List<EmsCorrespondentContact>
				{
					new EmsCorrespondentContact
					{
						Name = model.Entity.FirstName + " " + model.Entity.MiddleName + " " + model.Entity.LastName,
						Email = model.Entity.Mail
					}
				}
			};

			ICollection<ApplicationLotResultFile> attachedFiles = new List<ApplicationLotResultFile>();

			if (file != null)
			{
				using (var client = new HttpClient())
				{
					var urlToken = this.emsConfiguration.EmsUrl + "/api/token";
					var keyValues = new List<KeyValuePair<string, string>> {
						new KeyValuePair<string, string>("username", this.emsConfiguration.SystemUserName),
						new KeyValuePair<string, string>("password", this.emsConfiguration.SystemUserPassword),
						new KeyValuePair<string, string>("grant_type", "password")
					};

					var obj = new FormUrlEncodedContent(keyValues);
					var responseToken = client.PostAsync(urlToken, obj).Result;
					if (responseToken.IsSuccessStatusCode)
					{
						var responseTokenJson = responseToken.Content.ReadAsStringAsync().Result;
						var responseTokenObj = JsonConvert.DeserializeObject<AuthResponse>(responseTokenJson);
						var authToken = "Bearer " + responseTokenObj.Access_token;

						client.DefaultRequestHeaders.Accept.Clear();
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						client.DefaultRequestHeaders.Add("Authorization", authToken);

						var fileBytes = this.fileStorage.GetBytes(file.Key, file.DbId);
						var fileAsStringContent = Convert.ToBase64String(fileBytes.Result);
						var fileDto = new FileRequestDto {
							Content = fileAsStringContent,
							Name = file.Name
						};

						var fileContent = JsonConvert.SerializeObject(fileDto);
						var buffer = Encoding.UTF8.GetBytes(fileContent);
						var byteContent = new ByteArrayContent(buffer);
						byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var fileStorageUrl = this.emsConfiguration.EmsUrl + "/api/docFile/postFile";
						var responseFile = client.PostAsync(fileStorageUrl, byteContent).Result;

						if (responseFile.IsSuccessStatusCode)
						{
							var responseFileJson = responseFile.Content.ReadAsStringAsync().Result;
							var responseFileObj = JsonConvert.DeserializeObject<FileResponseDto>(responseFileJson);

							var copiedFile = new ApplicationLotResultFile(responseFileObj.Key, file.Hash, file.Size, file.Name, file.MimeType, responseFileObj.DbId);

							attachedFiles.Add(copiedFile);
						}
					}
				}
			}

			var emsRegisterNumber = regNumber.Split('/');

			var application = new EmsApplication {
				ElectronicServiceUri = electornicServiceUri,
				UseEmailForCorrespondence = false,
				Applicant = correspondent,
				StructuredData = JObject.FromObject(model),
				ParentDocNumber = hasParent ? regNumber : null,
				ExternalNumber = hasParent ? null : emsRegisterNumber[0],
				ExternalNumberDate = hasParent ? null : DateTime.Now.Date,
				AttachedFiles = attachedFiles,
				SkipConfirmationDoc = hasParent ? true : false,
				SkipConfirmationEmail = true
			};

			return application;
		}
	}
}
