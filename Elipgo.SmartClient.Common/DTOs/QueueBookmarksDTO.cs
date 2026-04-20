using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class QueueBookmarksDTO
    {
        public VaultItemCardState statusQueue { get; set; }
        public BookmarkGroupDTO bookmarkGroupDTO { get; set; }
    }
}
