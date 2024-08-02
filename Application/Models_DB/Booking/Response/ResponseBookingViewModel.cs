namespace Application
{
    public class ResponseBookingViewModel
    {
        public int Id;
        public DateTime SelectedDate { get; set; }
        public string SelectedTime { get; set; }
        public List<string> ReservedTimes { get; set; } = new List<string>();
    }
}