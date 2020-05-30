const deleteButton = document.getElementById("delete-button");
const modalCancelButton = document.getElementById("modal-cancel-button");
const modal = document.getElementById("delete-confirmation-modal");
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
    };
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
//# sourceMappingURL=single.js.map