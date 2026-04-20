using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class GridsDTO
    {
        public List<GridDTO> Grids { get; set; }
    }

    public class GridDTO
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public List<ContainerDTO> Elements { get; set; }
    }

    public class ContainerDTO
    {
        public int ContainerId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
