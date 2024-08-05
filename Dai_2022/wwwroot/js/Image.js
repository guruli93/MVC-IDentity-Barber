function openImageModal(imageData, contentType) {
    var modal = document.getElementById('imageModal');
    var modalImage = document.getElementById('modalImage');

    // სურათის შევსება Base64 მონაცემებით
    modalImage.src = 'data:' + contentType + ';base64,' + imageData;

  
    modal.style.display = 'flex';
}

function closeImageModal() {
    var modal = document.getElementById('imageModal');
    modal.style.display = 'none';
}


window.onclick = function (event) {
    var modal = document.getElementById('imageModal');
    if (event.target == modal) {
        closeImageModal();
    }
}
