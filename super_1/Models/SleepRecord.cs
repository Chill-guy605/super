namespace super_1.Models
{
    public class SleepRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime SleepStart { get; set; }
        public DateTime? SleepEnd { get; set; }

        public User User { get; set; }
    }
}
