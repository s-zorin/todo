const taskNameInput = document.getElementById("Task_Name") as HTMLInputElement;
const taskDueDateInput = document.getElementById("Task_DueDate") as HTMLInputElement;

if (taskNameInput) {
    taskNameInput.required = true;
    taskNameInput.maxLength = 50;
}

if (taskDueDateInput) {
    taskDueDateInput.required = true;
    taskDueDateInput.min = getMinDate();
}

function getMinDate(): string {
    let now = new Date();
    return `${now.getFullYear()}-${pad(now.getMonth() + 1)}-${pad(now.getDay())}`;
}

function pad(n: number): string {
    return n.toString().padStart(2, "0");
}