var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (Object.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
define(["require", "exports", "../shared/components/modal"], function (require, exports, modal) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    modal = __importStar(modal);
    const deleteButton = document.getElementById("delete-button");
    const modalCancelButton = document.getElementById("modal-cancel-button");
    const modalElement = document.getElementById("delete-confirmation-modal");
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
    function hideModal(event) {
        if (event) {
            if (event.target != event.currentTarget) {
                return;
            }
        }
        modal.hideModal(modalElement);
    }
});
//# sourceMappingURL=single.js.map