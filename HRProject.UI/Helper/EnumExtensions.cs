using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HRProject.UI.Helper
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());

            if (field != null)
            {
                var displayAttribute = field.GetCustomAttribute<DisplayAttribute>();

                if (displayAttribute != null)
                {
                    return displayAttribute.Name;
                }
            }

            return value.ToString();
        }
    }
}
