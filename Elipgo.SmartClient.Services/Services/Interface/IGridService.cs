using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface IGridService
    {
        string PathGridResources { get; set; }

        List<GridDTO> Get(GridFilterDTO gridFilter);
        List<GridDTO> GetForPlayback(GridFilterDTO gridFilter);
        List<GridDTO> Get(GridFilterDTO gridFilter, bool gridNext);
        List<GridDTO> GetForPlayback(GridFilterDTO gridFilter, bool gridNext);
    }
}
