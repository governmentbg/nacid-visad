using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VisaD.Hosting.Infrastructure.Captcha.Models;
using VisaD.Hosting.Infrastructure.Configurations;

namespace VisaD.Hosting.Infrastructure.Captcha
{
	public class CaptchaService
	{
		private readonly ReCaptchaConfiguration captchaConfiguration;

		public CaptchaService(IOptions<ReCaptchaConfiguration> options)
		{
			this.captchaConfiguration = options.Value;
		}

		public async Task<bool> Verify(string captcha)
		{
			if (captcha == "undefined")
			{
				return false;
			}

			var body = new Dictionary<string, string>
			{
				{ "secret", this.captchaConfiguration.Secret },
				{ "response", captcha }
			};

			var content = new FormUrlEncodedContent(body);

			using (var client = new HttpClient())
			{
				var response = await client.PostAsync(this.captchaConfiguration.ApiUrl, content);

				if (response.IsSuccessStatusCode)
				{
					var resultJson = await response.Content.ReadAsStringAsync();
					var result = JsonConvert.DeserializeObject<CaptchaResponse>(resultJson);

					return result.Success;
				}
			}

			return false;
		}
	}
}
