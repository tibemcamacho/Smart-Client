namespace Elipgo.SmartClient.Common.DTOs
{
    public class CheckElementDTO
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }

        public bool Separator { get; set; }
        public System.Drawing.Bitmap Icon { get; set; }
        public System.Object Control { get; set; }
        public bool Visible { get; set; } = true;
    }

}
