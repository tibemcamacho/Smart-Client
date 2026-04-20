using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class BookmarkFilterDTO
    {
        public int UserId { get; set; }
        public string NamePrefix { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DvfId { get; set; }
        public int[] Status { get; set; }
        public bool IsDeleted { get; set; }
        public int Page { get; set; }
    }
}
