using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WinApi.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var field = enumType.GetField(enumValue.ToString());
            var attributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (!attributes.Any())
            {
                attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (!attributes.Any())
                {
                    return enumValue.ToString();
                }
                var enumDescription = ((DescriptionAttribute)attributes[0]).Description;
                return enumDescription;
            }
            var enumName = ((DisplayAttribute)attributes[0]).GetName();
            return enumName;
        }
    }
}