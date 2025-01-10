function showModal(selector) {
    const modalElement = document.querySelector(selector);
    if (modalElement) {
        const modalInstance = new bootstrap.Modal(modalElement);
        modalInstance.show();
    }
}

function hideModal(selector) {
    const modalElement = document.querySelector(selector);
    if (modalElement) {
        const modalInstance = bootstrap.Modal.getInstance(modalElement);
        if (modalInstance) {
            modalInstance.hide();
        }
    }
}