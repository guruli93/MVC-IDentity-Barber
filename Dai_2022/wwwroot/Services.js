document.getElementById('servicesLink').addEventListener('click', function (e) {
    e.preventDefault();
    var dropdown = document.getElementById('servicesDropdown');

    if (dropdown.classList.contains('show')) {
        // იშლება ნელა
        dropdown.classList.remove('show');
    } else {
        // გამოჩენა ნელა
        dropdown.classList.add('show');
    }
});
