define(["require", "exports", "focus-trap"], function (require, exports, focusTrap) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var deleteButton = document.getElementById("delete-button");
    var modalCancelButton = document.getElementById("modal-cancel-button");
    var modal = document.getElementById("delete-confirmation-modal");
    var trap = focusTrap(modal, null);
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
            trap.activate();
        }
    }
    function hideModal() {
        if (modal) {
            modal.style.display = "none";
            trap.deactivate();
        }
    }
});
//# sourceMappingURL=single.js.map