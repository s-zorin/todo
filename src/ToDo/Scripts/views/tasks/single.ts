const deleteButton = document.getElementById("delete-button") as HTMLButtonElement;
const modalCancelButton = document.getElementById("modal-cancel-button") as HTMLButtonElement;
const modal = document.getElementById("delete-confirmation-modal") as HTMLElement;

if (deleteButton) {
    deleteButton.onclick = showModal;
}

if (modalCancelButton) {
    modalCancelButton.onclick = hideModal;
}

if (modal) {
    modal.onclick = function (event) {
        if (event.target === modal) {
            hideModal();
        }
    }
}

function showModal() {
    if (modal) {
        modal.style.display = "block";
    }
}

function hideModal() {
    if (modal) {
        modal.style.display = "none";
    }
}

