import { Dictionary } from "typescript-collections";
import focusTrap, { FocusTrap } from "focus-trap"; 

let traps = new Dictionary<HTMLElement, FocusTrap>();

export function showModal(element: HTMLElement) {
    if (element) {
        element.style.display = "block";
        let trap = traps.getValue(element);
        if (!trap) {
            trap = focusTrap(element);
            traps.setValue(element, trap);
        }
        trap.activate();
    }
}

export function hideModal(element: HTMLElement) {
    if (element) {
        element.style.display = "none";
        let trap = traps.getValue(element);
        if (trap) {
            trap.deactivate();
        }
    }
}