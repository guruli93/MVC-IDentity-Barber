document.addEventListener('DOMContentLoaded', () => {
    const timeSlotsContainer = document.getElementById('time-slots');
    const dateInput = document.getElementById('date');
    const selectedDateDisplay = document.getElementById('selected-date');
    const reservationsTableBody = document.querySelector('#reservations-table tbody');

    // Initialize Flatpickr for date selection
    flatpickr("#date", {
        onChange: function (selectedDates, dateStr, instance) {
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
                reserveTime(date, time);
            } else {
                alert('Please select a date first.');
            }
        }
    });

    function reserveTime(date, time) {
        fetch('/reservations/add', {
            method: 'POST',
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
