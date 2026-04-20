using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class GridService : IGridService
    {
        private readonly Configuration _config;
        public string PathGridResources { get; set; }

        public GridService()
        {
            _config = SmartClientEnvironmentUtils.GetConfiguration();
        }


        public List<GridDTO> Get(GridFilterDTO gridFilter)
        {
            if (!File.Exists(PathGridResources))
                throw new FileNotFoundException("grid.json Not Found");

            string json = File.ReadAllText(PathGridResources);
            GridsDTO grids = JsonConvert.DeserializeObject<GridsDTO>(json);

            if (!string.IsNullOrEmpty(gridFilter.Id))
            {
                return grids.Grids.Where(g => g.Id == gridFilter.Id).ToList();
            }

            if (gridFilter.MaximumQuantity > 0)
            {
                return grids.Grids.Where(g => g.Elements.Count <= gridFilter.MaximumQuantity).ToList();
            }

            return grids.Grids;
        }

        public List<GridDTO> GetForPlayback(GridFilterDTO gridFilter)
        {
            if (!File.Exists(PathGridResources))
                throw new FileNotFoundException("grid.json Not Found");

            string json = File.ReadAllText(PathGridResources);
            GridsDTO grids = JsonConvert.DeserializeObject<GridsDTO>(json);

            if (Boolean.Parse(_config.AppSettings.Settings["ShowOnlyGrid"].Value))
            {
                GridsDTO showGrid = new GridsDTO();
                showGrid.Grids = new List<GridDTO>();
                showGrid.Grids.Add(grids.Grids.ElementAt(0));
                showGrid.Grids.Add(grids.Grids.ElementAt(1));
                showGrid.Grids.Add(grids.Grids.ElementAt(4));
                showGrid.Grids.Add(grids.Grids.ElementAt(7));
                showGrid.Grids.Add(grids.Grids.ElementAt(9));

                grids = showGrid;
            }
            if (!string.IsNullOrEmpty(gridFilter.Id))
            {
                return grids.Grids.Where(g => g.Id == gridFilter.Id).ToList();
            }

            if (gridFilter.MaximumQuantity > 0)
            {
                return grids.Grids.Where(g => g.Elements.Count <= gridFilter.MaximumQuantity).ToList();
            }

            return grids.Grids;
        }

        public List<GridDTO> Get(GridFilterDTO gridFilter, bool gridNext)
        {
            if (!File.Exists(PathGridResources))
                throw new FileNotFoundException("grid.json Not Found");

            string json = File.ReadAllText(PathGridResources);
            GridsDTO grids = JsonConvert.DeserializeObject<GridsDTO>(json);

            if (!string.IsNullOrEmpty(gridFilter.Id))
            {
                var gr = new List<GridDTO>();
                if (gridNext)
                {
                    int f = int.Parse(gridFilter.Id);
                    gr = grids.Grids.Where(g => g.Elements.Count >= f).ToList();
                }
                else
                    grids.Grids.Where(g => g.Id == gridFilter.Id).ToList();

                return gr;
            }

            return grids.Grids;
        }

        public List<GridDTO> GetForPlayback(GridFilterDTO gridFilter, bool gridNext)
        {
            if (!File.Exists(PathGridResources))
                throw new FileNotFoundException("grid.json Not Found");

            string json = File.ReadAllText(PathGridResources);
            GridsDTO grids = JsonConvert.DeserializeObject<GridsDTO>(json);

            if (Boolean.Parse(_config.AppSettings.Settings["ShowOnlyGrid"].Value))
            {
                GridsDTO showGrid = new GridsDTO();
                showGrid.Grids = new List<GridDTO>();
                showGrid.Grids.Add(grids.Grids.ElementAt(0));
                showGrid.Grids.Add(grids.Grids.ElementAt(0));
                showGrid.Grids.Add(grids.Grids.ElementAt(1));
                showGrid.Grids.Add(grids.Grids.ElementAt(4));
                showGrid.Grids.Add(grids.Grids.ElementAt(7));
                showGrid.Grids.Add(grids.Grids.ElementAt(9));

                grids = showGrid;
            }
            if (gridNext)
            {
                var gr = new List<GridDTO>();
                int f = (int.Parse(gridFilter.Id) > gridFilter.MaximumQuantity ? gridFilter.MaximumQuantity : int.Parse(gridFilter.Id));
                gr = grids.Grids.Where(g => g.Elements.Count >= f && g.Elements.Count <= gridFilter.MaximumQuantity).ToList();
                return gr;
            }
            else if (gridFilter.MaximumQuantity > 0)
            {
                return grids.Grids.Where(g => g.Elements.Count <= gridFilter.MaximumQuantity).ToList();
            }

            return grids.Grids;
        }
    }
}
