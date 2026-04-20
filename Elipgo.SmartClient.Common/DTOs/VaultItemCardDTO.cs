using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class VaultItemCardDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label1 { get; set; }
        public string Label2 { get; set; }
        public int Progress { get; set; }
        //public int Status { get; set; } = 0;
        public BookmarkGroupDTO BookmarkGroup { get; set; } = new BookmarkGroupDTO();

    }
}
