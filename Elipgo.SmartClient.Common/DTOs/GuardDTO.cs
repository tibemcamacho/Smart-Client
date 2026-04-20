using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class GuardDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isActivated { get; set; }
        public GuardTourDTO[] GuardTours { get; set; }
    }

    public class ActivateGuardDTO
    {
        public int Id { get; set; }
        public bool isActivated { get; set; }

    }

    public class GuardTourDTO {
        public int PresetId { get; set; }
        public int Time { get; set; }
        public int Speed { get; set; }
    }

    public class GuardForCreationDTO {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isActivated { get; set; }
        public int TimeBetweenSequences { get; set; }
        public GuardTourForCreationDTO[] GuardTours { get; set; }
    }

    public class GuardTourForCreationDTO: GuardTourDTO {

        public WaitTimeViewType WaitTimeViewType { get; set; }
        public int ViewOrder { get; set; }
    }

    public enum WaitTimeViewType { Seconds, Minutes, Hours  }

}
