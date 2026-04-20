namespace Elipgo.SmartClient.Common.DTOs
{
    public class ScenesEntity
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }
        public SceneElementEntity[] Elements { get; set; }
    }
}
