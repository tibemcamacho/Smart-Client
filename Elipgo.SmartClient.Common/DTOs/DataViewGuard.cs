namespace Elipgo.SmartClient.Common.DTOs
{
    public class DataViewGuard
    {
        public int Id { get; set; }
        public PresetDTO Preset { get; set; }
        public string Name { get => Preset.Name; }
        public int Speed { get; set; }
        public int Time { get; set; }
        public int UnitTime { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
    }
}


