function openImageModal(imageData, contentType) {
    var modal = document.getElementById('imageModal');
    var modalImage = document.getElementById('modalImage');

    // სურათის შევსება Base64 მონაცემებით
    modalImage.src = 'data:' + contentType + ';base64,' + imageData;

    // მოდალური ფანჯარის გაწვდვა
    modal.style.display = 'flex'; /* განთავსეთ მოდალური ფანჯარა ცენტრში */
}

function closeImageModal() {
    var modal = document.getElementById('imageModal');
    modal.style.display = 'none'; /* დახურეთ მოდალური ფანჯარა */
}

// დახურეთ მოდალური ფანჯარა ფონზე დაწკაპებით
window.onclick = function (event) {
    var modal = document.getElementById('imageModal');
    if (event.target == modal) {
        closeImageModal();
    }
}
