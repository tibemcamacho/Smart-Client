using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;

namespace Elipgo.SmartClient.Drivers
{
    public interface IMultiDownloadBookmarks
    {
        void AddQueue(List<BookmarkGroupDTO> bookmarkGroup);
        void DeleteQueue(BookmarkGroupDTO BookmarkGroupDTO);
        void ProcesQueue();
        void ClearQueue();
        void Dispose();
    }
}
