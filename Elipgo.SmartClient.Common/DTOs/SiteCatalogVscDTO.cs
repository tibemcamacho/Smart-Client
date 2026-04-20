using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class SiteCatalogVscDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CameraCatalogVscDTO> Cameras { get; set; }
    }

    public class CameraCatalogVscDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SourceCatalogVscDTO> Sources { get; set; }
    }

    public class SourceCatalogVscDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string VirtualPath { get; set; }
        public string HttpProtocol { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ProductCode { get; set; }

        public string GetVirtualPath()
        {
            if (string.IsNullOrEmpty(this.VirtualPath))
            {
                return "";
            }
            return string.Format(@"/{0}", this.VirtualPath);
        }

    }

}
