using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.Helpers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class TranslationAttribute : Attribute
    {
        public string Name { get; }
        public Type ResourceType { get; }

        public TranslationAttribute(string name)
        {
            Name = name;
        }

        public TranslationAttribute(string name, Type resourceType)
        {
            Name = name;
            ResourceType = resourceType;
        }

        public string GetValue()
        {
            if (ResourceType == null) return Name;

            // Busca propiedad estática en el archivo de recursos
            var prop = ResourceType.GetProperty(Name,
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            return prop?.GetValue(null, null)?.ToString() ?? Name;
        }
    }
}
