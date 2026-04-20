using System.Collections.Generic;
using System.Linq;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class BookmarkGroupByPageDTO
    {
        public PageDTO Page { get; set; }
        public IList<BookmarkGroupDTO> Bookmarks { get; set; }
    }
}