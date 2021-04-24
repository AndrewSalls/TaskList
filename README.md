# TaskList
Loads, saves, and enables the editing, sorting, and filtering of user-defined tasks.
The code automatically creates a file (ObjectiveLog.json) if said file doesn't already exist in the project, and stores any future created tasks in the fiile.
Each task contains a name, description, created, edited, and "due" date, a persists and repeats flag, and a type and importance.
Name and description are self explanatory, and the created and edited dates are just in case that information is later useful to the user.
The due date is the time where the task is supposed to be done. Opening the task manager after this date will automatically delete tasks whose due date has already passed.
This can be prevented with the persists flag. The repeats flag will increase the due date by 24 hours until the due date is a time after the current time, for tasks that need to be done daily.
The type and importance options are categories to make sorting and filtering the tasks possible.
