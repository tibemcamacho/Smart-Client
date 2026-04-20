namespace Elipgo.SmartClient.Common.DTOs
{
    public class OptionItemDTO<T>
    {
        public T Key { get; set; }
        public string Name { get; set; }
        public object Tag { get; set; }
        public object Item { get; set; }
    }

    public class OptionItemDTO
    {
        public int Key { get; set; }
        public string Name { get; set; }
    }
}
