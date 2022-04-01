using System.ComponentModel;
using System.Reflection;
using VisaD.Application.Common.Interfaces;

namespace VisaD.Application.Common.Services
{
	public class EnumUtility : IEnumUtility
    {
        public string GetDescription(object value)
        {
            FieldInfo fieldInfo = value.GetType()
                .GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null
                && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static string GetDescriptionSt(object value)
        {
            FieldInfo fieldInfo = value.GetType()
                .GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null
                && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
