namespace Entities
{
    public class History
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int EventTypeId { get; set; }
        public EventType EventType { get; set; }
    }
}
