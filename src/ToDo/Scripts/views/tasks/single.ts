import focusTrap = require("focus-trap");

const deleteButton = document.getElementById("delete-button") as HTMLButtonElement;
const modalCancelButton = document.getElementById("modal-cancel-button") as HTMLButtonElement;
const modal = document.getElementById("delete-confirmation-modal") as HTMLElement;
const trap = focusTrap(modal, null);

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
        trap.activate();
    }
}

function hideModal() {
    if (modal) {
        modal.style.display = "none";
        trap.deactivate();
    }
}

