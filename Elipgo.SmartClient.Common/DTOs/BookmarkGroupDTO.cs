using System.Collections.Generic;
using System.Linq;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class BookmarkGroupDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FileFormat { get; set; }
        public bool IsDeleted { get; set; }
        public int Status { get; set; }
        public List<BookmarkGroupElementDTO> Elements { get; set; }

        // Extra
        public float TotalProgress
        {
            get
            {
                return (Elements.Sum(x => x.Progress) / (Elements.Count));
            }
        }
    }
}