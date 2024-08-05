// JavaScript
document.addEventListener('DOMContentLoaded', () => {
    const container = document.querySelector('.product-container');
    const items = document.querySelectorAll('.product-card').length;
    const itemWidth = document.querySelector('.product-card').offsetWidth + 20; // margin + padding
    let currentIndex = 0;

    function moveSlide() {
        currentIndex = (currentIndex + 1) % items;
        container.style.transform = `translateX(-${currentIndex * itemWidth}px)`;
    }

    // ავტომატური გადასვლა
    setInterval(moveSlide, 5000);

    // ღილაკების ფუნქციონალი
    document.querySelector('.carousel-button.left').addEventListener('click', () => {
        currentIndex = (currentIndex - 1 + items) % items;
        container.style.transform = `translateX(-${currentIndex * itemWidth}px)`;
    });

    document.querySelector('.carousel-button.right').addEventListener('click', () => {
        moveSlide();
    });
});
