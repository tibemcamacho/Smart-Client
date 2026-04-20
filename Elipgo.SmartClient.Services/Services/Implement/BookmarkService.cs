using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class BookmarkService : IBookmarkService
    {
        public async Task<bool> AddBookmark(BookmarkGroupDTO bookmarkGroup, string token)
        {
            try
            {
                var result = await Client.Client.Instance.PostAsync<BookmarkGroupDTO>(bookmarkGroup, "/v1/bookmarkgroups", token);
                return result != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteBookmark(List<int> bookmarkGroupId, string token)
        {
            try
            {
                //var result = Client.Client.Instance.DeleteAsync<BookmarkGroupDTO>($"/v1/bookmarkgroups/{bookmarkGroupId}", token);
                Boolean result = await Client.Client.Instance.PutAsync<Boolean>(bookmarkGroupId, "/v1/bookmarkgroups/delete", token);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log($"DeleteBookmark error: {ex.Message}", LogPriority.Warning);
                return false;
            }
        }

        public async Task<IList<BookmarkGroupDTO>> Get(BookmarkFilterDTO filter, string token)
        {
            var result = await Client.Client.Instance.PostAsync<List<BookmarkGroupDTO>>(filter, "/v1/bookmarkgroups/page", token);

            return result;
        }
    }
}
