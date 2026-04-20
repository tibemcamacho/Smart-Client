using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Drivers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Elipgo.SmartClient.ViewModels
{
    public class GuardViewModel : IGenericViewModel
    {
        public long UserId { get; set; }
        public string UserIdGuid { get; set; }
        public string Token { get; set; }
        public long EntityId { get; set; }
        public CatalogDTO Catalog { get; set; }
        public ResourceManager Resource { get; set; }
        public IDriverLive Driver { get; set; }
        public List<OptionItemDTO<int>> GetTime()
        {
            return new int[] { 1, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 }.Select
                (
                    p => new OptionItemDTO<int>
                    {
                        Name = p.ToString(),
                        Key = p
                    }
                ).ToList();
        }

        public List<OptionItemDTO<int>> GetUnitTime()
        {
            return Enum.GetValues(typeof(UnitTime)).Cast<UnitTime>().Select
                (
                    p => new OptionItemDTO<int>
                    {
                        Name = p.GetType()
                                .GetMember(p.ToString())
                                .First()
                                .GetCustomAttribute<DisplayAttribute>().Name,
                        Key = (int)p
                    }
                ).ToList();
        }

        public List<OptionItemDTO<int>> GetSpeed()
        {
            return new int[] { 1, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 }.Select
              (
                  p => new OptionItemDTO<int>
                  {
                      Name = p.ToString(),
                      Key = p
                  }
              ).ToList();
        }

        public int GetTime(int time, int unitTime)
        {
            return time * GetCoef(unitTime);
        }

        private int GetCoef(int unitTime)
        {
            switch ((UnitTime)unitTime)
            {
                case UnitTime.Seg:
                    return 1;
                case UnitTime.Min:
                    return 60;
                case UnitTime.Hs:
                    return 3600;
                default:
                    return 1;
            }
        }

        public int GetUnitTime(int time)
        {
            if (time < 60)
                return (int)UnitTime.Seg;
            if (time < 3600)
                return (int)UnitTime.Min;

            return (int)UnitTime.Hs;
        }

        public int GetTime(int time)
        {
            var unit = GetUnitTime(time);

            switch ((UnitTime)unit)
            {
                case UnitTime.Seg:
                    return time;
                case UnitTime.Min:
                    return time / 60;
                case UnitTime.Hs:
                    return time / 3600;
                default:
                    return time;
            }
        }
    }
}
