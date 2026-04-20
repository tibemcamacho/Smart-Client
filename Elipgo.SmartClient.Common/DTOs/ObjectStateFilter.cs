using System.Collections.Generic;
using Elipgo.SmartClient.Common.Enum;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class ObjectStateFilter
    {
        public ElementSidebar Type { get; set; }
        public List<CheckElementDTO> options { get; set; }
    }

}
