namespace Elipgo.SmartClient.Common.DTOs
{
    public enum SubType
    {
        Default = 0,
        Preset = 1,
        Guard = 2,
    }
    public class SceneElementEntity
    {
        public int Id { get; set; }
        public int SceneId { get; set; }
        public int ObjectId { get; set; }
        public string ObjectType { get; set; }
        public int Action { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public SubType ObjectSubType { get; set; }
        public string NameObjectSubType { get; set; }
        public int? ObjectSubId { get; set; }
    }
}
