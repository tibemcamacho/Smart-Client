using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System.Collections.Generic;
using System.Resources;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.ViewModels
{
    public class BookmarkViewModel : IGenericViewModel
    {
        public long UserId { get; set; }
        public string UserIdGuid { get; set; }
        public string Token { get; set; }
        public long EntityId { get; set; }
        public CatalogDTO Catalog { get; set; }
        public ResourceManager Resource { get; set; }

        private IBookmarkService _bookmarkService = Locator.Current.GetService<IBookmarkService>();

        public async Task<bool> AddBookmark(BookmarkGroupDTO bookmark)
        {
            return await _bookmarkService.AddBookmark(bookmark, Token);
        }

        public async Task<bool> DeleteBookmark(List<int> bookmarkId)
        {
            return await _bookmarkService.DeleteBookmark(bookmarkId, Token);
        }

        public async Task<CameraDTO> GetCamera(int elementId)
        {
            return await Vmon5Service.GetCamera(elementId, Token);
        }
    }
}
