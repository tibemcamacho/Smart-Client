using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class GridElement
    {
        public SidebarElementDTO SidebarElement { get; set; }
        public ElementGroupingMode GroupingMode { get; set; }
    }
}
