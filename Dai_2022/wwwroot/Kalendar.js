document.addEventListener('DOMContentLoaded', () =>
{
    const timeSlotsContainer = document.getElementById('time-slots');
    const dateInput = document.getElementById('date');
    const selectedDateDisplay = document.getElementById('selected-date');
    const reservationsTableBody = document.querySelector('#reservations-table tbody');


    const moment = require('moment-timezone');

 

     Initialize Flatpickr for date selection
    flatpickr("#date", {
        onChange: function (selectedDates, dateStr, instance) {
            // Log the selected date and its timestamp for debugging
            console.log('Flatpickr Selected Date:', dateStr);
            console.log('Flatpickr Timestamp:', selectedDates[0].getTime());

            selectedDateDisplay.textContent = dateStr;
        }
    });

    // Handle click on time slots
    timeSlotsContainer.addEventListener('click', function (event) {
        if (event.target.closest('.time-slot') && !event.target.closest('.time-slot').classList.contains('busy')) {
            const timeSlot = event.target.closest('.time-slot');
            const time = timeSlot.dataset.time;

            if (selectedDateDisplay.textContent !== 'Please select a date') {
                const date = selectedDateDisplay.textContent;
                console.log('Reserving Date:', date);  // Log the date being reserved
                reserveTime(date, time);
            } else {
                alert('Please select a date first.');
            }
        }
    });

    function reserveTime(date, time) {
        fetch('/reservations/add', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                date: date,
                time: time
            })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    addReservationToTable(data.reservation);
                } else {
                    alert('Failed to reserve the time slot.');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('An error occurred.');
            });
    }

    function addReservationToTable(reservation) {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${formatTime(reservation.time)}</td>
            <td>${reservation.dayOfWeek}</td>
            <td>${reservation.month} ${reservation.dayNumber}</td>
        `;

        reservationsTableBody.appendChild(row);
    }

    function formatTime(time) {
        const [hours, minutes] = time.split(':').map(Number);
        const period = hours >= 12 ? 'PM' : 'AM';
        const formattedHours = hours % 12 || 12;
        return `${formattedHours}:${minutes < 10 ? '0' : ''}${minutes} ${period}`;
    }
});
