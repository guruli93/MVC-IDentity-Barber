
        function updateSlots(reservedTimes) {
            document.querySelectorAll('.time-slot').forEach(button => {
                button.classList.remove('reserved', 'selected', 'free');
                button.classList.add('free');
            });

            document.querySelectorAll('.time-slot').forEach(button => {
                const time = button.getAttribute('data-time');
                if (reservedTimes.includes(time)) {
                    button.classList.add('reserved');
                } else {
                    button.classList.add('free');
                }
            });
        }

        flatpickr("#date", {
            dateFormat: "Y-m-d",
            inline: true,
            onChange: function (selectedDates, dateStr, instance) {
                if (selectedDates.length === 0) {
                    console.error('No date selected.');
                    return;
                }

                const selectedDate = selectedDates[0];
                const formattedDate = moment(selectedDate).tz("Asia/Tbilisi").format('YYYY-MM-DD');

                document.getElementById('selected-date').innerText = formattedDate;

                $.getJSON('/Booking/GetBookings', { date: formattedDate })
                    .done(function (reservedTimes) {
                        updateSlots(reservedTimes);
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        console.error('Failed to fetch reserved times:', textStatus, errorThrown);
                    });
            }
        });

        document.querySelectorAll('.time-slot').forEach(button => {
            button.addEventListener('click', function () {
                if (this.classList.contains('reserved')) {
                    return;
                }

                document.querySelectorAll('.time-slot').forEach(btn => btn.classList.remove('selected'));
                this.classList.add('selected');
                document.getElementById('selected-time').value = this.getAttribute('data-time');
            });
        });

        document.getElementById('pay-button').addEventListener('click', function () {
            const formData = {
                SelectedDate: document.getElementById('date').value,
                SelectedTime: document.getElementById('selected-time').value
            };

            $.ajax({
                url: '/Booking/BookAppointment',
                type: 'POST',
                data: formData,
                success: function (response) {
                    $('#paymentModal').modal('hide');
                    document.getElementById('booking-date').textContent = formData.SelectedDate;
                    document.getElementById('payment-success').style.display = 'block';

                    let countdown = 5;
                    const countdownElement = document.getElementById('countdown');
                    const interval = setInterval(() => {
                        countdown -= 1;
                        countdownElement.textContent = countdown;
                        if (countdown <= 0) {
                            clearInterval(interval);
                            window.location.href = "/Services/ShowRezervation";
                        }
                    }, 1000);

                    const selectedDateElement = document.querySelector('.flatpickr-day.selected');
                    if (selectedDateElement) {
                        selectedDateElement.classList.add('highlight');
                    }
                },
                error: function () {
                    console.error('Payment request failed.');
                }
            });
        });
    
 