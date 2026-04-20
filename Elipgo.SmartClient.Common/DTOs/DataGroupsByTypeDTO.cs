using Elipgo.SmartClient.Common.Enum;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class DataGroupsByTypeDTO
    {
        public string Group { get; set; }

        public string Type { get; set; }

        public string  PropertyListName { get; set; }

        public string TypeName { get; set; }

        public ElementType ElementType { get; set; }
    }
}
