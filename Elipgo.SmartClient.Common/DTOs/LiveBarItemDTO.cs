using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class LiveBarItemDTO
    {
        public LiveBarItemDTO(LiveBarButtom type)
        {
            ButtomType = type;
        }

        public LiveBarButtom ButtomType { get; set; }
    }

    public enum LiveBarButtom
    {
        refresh,
        groups,
        carousel,
        scenes,
        grids,
        removeGrids,
        saveGroups,
        saveEscene,
        removeAllAlarms,
        activeCarousel,
        preset,
        guard
    }
}
