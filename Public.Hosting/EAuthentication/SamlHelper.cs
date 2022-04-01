using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Web;
using System.Xml;

namespace Public.Hosting.EAuthentication
{
	public class SamlHelper
	{
		internal static EAuthLoginDataDto ParseEAuthResponse(Stream SamlResponse)
		{
			var eAuthLoginDataDto = new EAuthLoginDataDto();
			if (SamlResponse == null)
			{
				throw new ArgumentNullException("SamlResponse");
			}

			XmlDocument doc = new XmlDocument();
			try
			{
				doc.Load(SamlResponse);
			}
			catch
			{
				eAuthLoginDataDto.ResponseStatus = EAuthResponseStatus.InvalidResponseXML;
				return eAuthLoginDataDto;
			}

			var responseElement = doc.DocumentElement;
			DecryptResponse(doc);

			var samlNS = new XmlNamespaceManager(doc.NameTable);
			samlNS.AddNamespace("saml2", "urn:oasis:names:tc:SAML:2.0:assertion");
			samlNS.AddNamespace("saml2p", "urn:oasis:names:tc:SAML:2.0:protocol");
			samlNS.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			var statusCode = responseElement.SelectSingleNode("//saml2p:Status/saml2p:StatusCode", samlNS);
			if (statusCode != null)
			{
				var statusCodeValue = statusCode.Attributes["Value"].Value;
				var innerStatusCode = statusCode.SelectSingleNode("saml2p:StatusCode", samlNS);
				if (innerStatusCode != null)
				{
					statusCodeValue = innerStatusCode.Attributes["Value"].Value;
				}
				var statusMessage = responseElement.SelectSingleNode("//saml2p:Status/saml2p:StatusMessage", samlNS);
				eAuthLoginDataDto.ResponseStatusMessage = statusMessage != null ? HttpUtility.HtmlDecode(statusMessage.InnerText) : string.Empty;
				eAuthLoginDataDto.ResponseStatus = GetResponseStatusFromCode(statusCodeValue, eAuthLoginDataDto.ResponseStatusMessage);
			}

			if (eAuthLoginDataDto.ResponseStatus != EAuthResponseStatus.Success)
			{
				return eAuthLoginDataDto;
			}

			var attributes = responseElement.SelectSingleNode("//saml2:EncryptedAssertion/saml2:Assertion/saml2:AttributeStatement", samlNS);
			if (attributes != null)
			{
				var peronIdentifier = attributes.SelectSingleNode("saml2:Attribute[@Name='urn:egov:bg:eauth:2.0:attributes:personIdentifier']/saml2:AttributeValue", samlNS);
				if (peronIdentifier != null) eAuthLoginDataDto.Egn = peronIdentifier.InnerText;

				var name = attributes.SelectSingleNode("saml2:Attribute[@Name='urn:egov:bg:eauth:2.0:attributes:personName']/saml2:AttributeValue", samlNS);
				if (name != null) eAuthLoginDataDto.Name = name.InnerText;
			}

			return eAuthLoginDataDto;
		}
		private static EAuthResponseStatus GetResponseStatusFromCode(string statusCode, string statusMessage)
		{
			switch (statusCode)
			{
				case "urn:oasis:names:tc:SAML:2.0:status:AuthnFailed":
					if (statusMessage.Trim().ToLower() == "отказан от потребител")
						return EAuthResponseStatus.CanceledByUser;
					else if (statusMessage.Trim().ToLower() == "not_detected_qes")
						return EAuthResponseStatus.NotDetectedQES;
					else
						return EAuthResponseStatus.AuthenticationFailed;
				case "urn:oasis:names:tc:SAML:2.0:status:Success":
					return EAuthResponseStatus.Success;
			}

			return EAuthResponseStatus.AuthenticationFailed;
		}

		private static void DecryptResponse(XmlDocument xml)
		{
			var samlNS = new XmlNamespaceManager(xml.NameTable);
			samlNS.AddNamespace("saml2", "urn:oasis:names:tc:SAML:2.0:assertion");
			samlNS.AddNamespace("saml2p", "urn:oasis:names:tc:SAML:2.0:protocol");
			samlNS.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			samlNS.AddNamespace("xenc", "http://www.w3.org/2001/04/xmlenc#");
			var encryptedNode = xml.SelectSingleNode("//saml2:EncryptedAssertion/xenc:EncryptedData", samlNS) as XmlElement;

			var encryptedXml = new EncryptedXml(xml);
			var encryptedData = new EncryptedData();
			encryptedData.LoadXml(encryptedNode);

			var cert = GetFromFile("device.pfx", "12345");

			var privateKey = cert.GetRSAPrivateKey();
			var cipherNode = xml.SelectSingleNode("//saml2:EncryptedAssertion/xenc:EncryptedData/ds:KeyInfo/xenc:EncryptedKey/xenc:CipherData/xenc:CipherValue", samlNS);
			var cipher = cipherNode.InnerText;
			var cipherBytes = Convert.FromBase64String(cipher);
			byte[] decryptedPrivateKey = null;
			if (privateKey != null)
			{
				decryptedPrivateKey = privateKey.Decrypt(cipherBytes, RSAEncryptionPadding.OaepSHA1);
			}
			AesManaged aes = new AesManaged {
				Mode = CipherMode.CBC,
				KeySize = 128,
				Padding = PaddingMode.None,
				Key = decryptedPrivateKey
			};

			var decryptedData = encryptedXml.DecryptData(encryptedData, aes);
			encryptedXml.ReplaceData(encryptedNode, decryptedData);
		}

		public static string GenerateXmlMetadata(string fileName, string certificatePass)
		{
			var cert = GetFromFile(fileName, certificatePass);
			var xmlResponse = string.Format(@"<EntityDescriptor xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""urn:oasis:names:tc:SAML:2.0:metadata"" entityID=""https://nacid.bg"" cacheDuration=""PT5M"">
	            <SPSSODescriptor protocolSupportEnumeration=""urn:oasis:names:tc:SAML:2.0:protocol"">
		            <KeyDescriptor use=""encryption"">
			            <KeyInfo xmlns=""http://www.w3.org/2000/09/xmldsig#"">
			            <X509Data>
				            <X509Certificate>{0}</X509Certificate>
			            </X509Data>
			            </KeyInfo>
		            </KeyDescriptor>
	            </SPSSODescriptor>
            </EntityDescriptor>", Convert.ToBase64String(cert.RawData));
			return xmlResponse;
		}

		private static X509Certificate2 GetFromFile(string fileName, string certificatePass)
		{
			var location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
			var content = File.ReadAllBytes(location);
			var cert = new X509Certificate2(content, certificatePass,
				X509KeyStorageFlags.MachineKeySet
				| X509KeyStorageFlags.PersistKeySet
				| X509KeyStorageFlags.Exportable);
			return cert;
		}
	}
}
