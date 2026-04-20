using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class BlueprintElementDTO
    {
        public int Id { get; set; }
        public int VMapId { get; set; }
        public int DeviceFeatureId { get; set; }
        public string DeviceFeatureName { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Rotation { get; set; }
        public string ObjectType { get; set; }
        public int ParentId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPtz { get; set; }
        public int? FeatureIconsId { get; set; }
        public IOPortState PortEstate { get; set; } = IOPortState.Offline;
    }
}
