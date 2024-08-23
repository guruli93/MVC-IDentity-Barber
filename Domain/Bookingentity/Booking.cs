
namespace Domain.Bookingentity;

public class Booking
{
    public int Id { get; set; }
    public DateTime SelectedDate { get; set; }
    public string SelectedTime { get; set; }
    public List<string> ReservedTimes { get; set; } = new List<string>();
}
