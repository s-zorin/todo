var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
define(["require", "exports", "typescript-collections", "focus-trap"], function (require, exports, typescript_collections_1, focus_trap_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.hideModal = exports.showModal = void 0;
    focus_trap_1 = __importDefault(focus_trap_1);
    let traps = new typescript_collections_1.Dictionary();
    function showModal(element) {
        if (element) {
            element.style.display = "block";
            let trap = traps.getValue(element);
            if (!trap) {
                trap = focus_trap_1.default(element);
                traps.setValue(element, trap);
            }
            trap.activate();
        }
    }
    exports.showModal = showModal;
    function hideModal(element) {
        if (element) {
            element.style.display = "none";
            let trap = traps.getValue(element);
            if (trap) {
                trap.deactivate();
            }
        }
    }
    exports.hideModal = hideModal;
});
//# sourceMappingURL=modal.js.map