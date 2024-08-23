
using Domain.Bookingentity;

namespace Application
{
    public interface ICommandBookingRepository
    {
       
        Task<bool> AddAsync(Booking entity);
        Task<bool> UpdateAsync(Booking entity);
        Task<bool> DeleteAsync(int Id);
    }

}
