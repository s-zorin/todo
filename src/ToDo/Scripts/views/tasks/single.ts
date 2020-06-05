import * as modal from "../shared/components/modal";

const deleteButton = document.getElementById("delete-button") as HTMLButtonElement;
const modalCancelButton = document.getElementById("modal-cancel-button") as HTMLButtonElement;
const modalElement = document.getElementById("delete-confirmation-modal") as HTMLElement;

if (deleteButton) {
    deleteButton.onclick = showModal;
}

if (modalCancelButton) { 
    modalCancelButton.onclick = hideModal;
}

if (modalElement) {
    modalElement.onclick = hideModal;
}

function showModal() {
    modal.showModal(modalElement);
}

function hideModal(): void
function hideModal(event?: MouseEvent): void {
    if (event) {
        if (event.target != event.currentTarget) {
            return;
        }
    }

    modal.hideModal(modalElement);
}