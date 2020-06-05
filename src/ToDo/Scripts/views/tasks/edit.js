const taskNameInput = document.getElementById("Task_Name");
const taskDueDateInput = document.getElementById("Task_DueDate");
if (taskNameInput) {
    taskNameInput.required = true;
    taskNameInput.maxLength = 50;
}
if (taskDueDateInput) {
    taskDueDateInput.required = true;
    taskDueDateInput.min = getMinDate();
}
function getMinDate() {
    let now = new Date();
    return `${now.getFullYear()}-${pad(now.getMonth() + 1)}-${pad(now.getDay())}`;
}
function pad(n) {
    return n.toString().padStart(2, "0");
}
//# sourceMappingURL=edit.js.map