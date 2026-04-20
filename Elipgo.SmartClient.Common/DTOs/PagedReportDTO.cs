using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class PagedReportDTO<T>
    {
        public int TotalRecords { get; set; }
        public IList<T> Report { get; set; }
    }
}
