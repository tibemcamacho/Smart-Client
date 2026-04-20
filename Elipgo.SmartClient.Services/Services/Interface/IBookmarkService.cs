using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface IBookmarkService
    {
        Task<bool> AddBookmark(BookmarkGroupDTO bookmarkGroup, string token);
        Task<bool> DeleteBookmark(List<int> bookmarkGroupId, string token);
        Task<IList<BookmarkGroupDTO>> Get(BookmarkFilterDTO filter, string token);
    }
}
