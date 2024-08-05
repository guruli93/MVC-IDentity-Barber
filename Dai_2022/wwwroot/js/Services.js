document.getElementById('servicesLink').addEventListener('click', function (e) {
    e.preventDefault();
    var dropdown = document.getElementById('servicesDropdown');

    if (dropdown.classList.contains('show')) {
       
        dropdown.classList.remove('show');
    } else {
        
        dropdown.classList.add('show');
    }
});
