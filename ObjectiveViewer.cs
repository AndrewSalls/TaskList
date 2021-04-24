using System;
using System.Windows.Forms;
using System.Drawing;
using TaskList.Properties;

namespace TaskList
{
    public class ObjectiveViewer : Form
    {
        private readonly ObjectiveList _objectiveList;
        private ToolStripButton save;
        private Form _subWindow;
        private Point _pastLoc;

        public static void Main(string[] _)
        {
            ObjectiveViewer window = new ObjectiveViewer("ObjectiveLog.json");
            Application.Run(window);
        }

        public ObjectiveViewer(string fileOutput)
        {
            _objectiveList = new ObjectiveList(fileOutput, EditObjectiveDisplay);
            _subWindow = null;
            save = null;
            Hide();
            InitDisplay();
            _pastLoc = Location;
            FormClosing += ObjectiveViewer_FormClosing;
            Show();
        }

        private void InitDisplay()
        {
            BackColor = Color.LightGoldenrodYellow;
            Font = new Font(FontFamily.GenericSansSerif, 12);
            Controls.Add(_objectiveList);
            Controls.Add(InitMenu());
            Text = "Objective List";
            Icon = Resources.mainIcon;

            ClientSize = new Size(800, 600);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            CenterToScreen();
            LocationChanged += ObjectiveViewer_LocationChanged;
            BringToFront();
            Focus();
        }

        private void ObjectiveViewer_LocationChanged(object sender, EventArgs e)
        {
            if (_subWindow != null)
            {
                _subWindow.Location = new Point(((Form)sender).Location.X - _pastLoc.X + _subWindow.Location.X, ((Form)sender).Location.Y - _pastLoc.Y + _subWindow.Location.Y);
            }

            _pastLoc = ((Form)sender).Location;
        }

        private ToolStrip InitMenu()
        {
            ToolStrip output = new ToolStrip
            {
                Dock = DockStyle.Top
            };

            save = new ToolStripButton
            {
                Text = "Save",
                TextAlign = ContentAlignment.MiddleCenter
            };
            save.Click += Save_Click;
            output.Items.Add(save);

            output.Items.Add(CreateFilterDropdown());
            output.Items.Add(CreateSortDropdown());
            output.Items.Add(CreatePriorityDropdown());

            ToolStripButton AtoZ = new ToolStripButton
            {
                Text = "Ascending",
                TextAlign = ContentAlignment.MiddleCenter,
                CheckOnClick = true,
                CheckState = CheckState.Unchecked
            };
            AtoZ.Click += (o, e) =>
            {
                if (AtoZ.Checked)
                    AtoZ.Text = "Descending";
                else
                    AtoZ.Text = "Ascending";

                _objectiveList.SetSortDirection(AtoZ.Checked);
            };
            output.Items.Add(AtoZ);

            ToolStripButton create = new ToolStripButton
            {
                Text = "New Objective",
                TextAlign = ContentAlignment.MiddleCenter
            };
            create.Click += (o, e) => AddObjectiveDisplay();
            output.Items.Add(create);

            output.Text = "Menu";
            output.Font = new Font(Font.SystemFontName, 12);
            return output;
        }

        private void Save_Click(object sender, EventArgs e) {
            save.Checked = false;
            save.Enabled = false;
            _objectiveList.SaveObjectives();
            save.Enabled = true;
        }

        private ToolStripDropDownButton CreateFilterDropdown()
        {
            ToolStripDropDownButton tFilter = new ToolStripDropDownButton();
            ToolStripDropDown tFilterDrop = new ToolStripDropDown();
            ToolStripButton[] tFilterOptions = new ToolStripButton[10];

            tFilterOptions[0] = new ToolStripButton{ Text = "All", Checked = true };
            tFilterOptions[0].Click += (o, e) => _objectiveList.SetFilterTo(Objective.ObjectiveType.ALL);
            tFilterOptions[1] = new ToolStripButton { Text = "Homework" };
            tFilterOptions[1].Click += (o, e) => _objectiveList.SetFilterTo(Objective.ObjectiveType.HOMEWORK);
            tFilterOptions[2] = new ToolStripButton { Text = "Job" };
            tFilterOptions[2].Click += (o, e) => _objectiveList.SetFilterTo(Objective.ObjectiveType.JOB);
            tFilterOptions[3] = new ToolStripButton { Text = "Club" };
            tFilterOptions[3].Click += (o, e) => _objectiveList.SetFilterTo(Objective.ObjectiveType.CLUB);
            tFilterOptions[4] = new ToolStripButton { Text = "Project" };
            tFilterOptions[4].Click += (o, e) => _objectiveList.SetFilterTo(Objective.ObjectiveType.PROJECT_IDEA);
            tFilterOptions[5] = new ToolStripButton { Text = "Personal" };
            tFilterOptions[5].Click += (o, e) => _objectiveList.SetFilterTo(Objective.ObjectiveType.PERSONAL);
            tFilterOptions[6] = new ToolStripButton { Text = "Free Time" };
            tFilterOptions[6].Click += (o, e) => _objectiveList.SetFilterTo(Objective.ObjectiveType.FREE_TIME);
            tFilterOptions[7] = new ToolStripButton { Text = "Note" };
            tFilterOptions[7].Click += (o, e) => _objectiveList.SetFilterTo(Objective.ObjectiveType.NOTE);
            tFilterOptions[8] = new ToolStripButton { Text = "Reminder" };
            tFilterOptions[8].Click += (o, e) => _objectiveList.SetFilterTo(Objective.ObjectiveType.REMINDER);
            tFilterOptions[9] = new ToolStripButton { Text = "Other" };
            tFilterOptions[9].Click += (o, e) => _objectiveList.SetFilterTo(Objective.ObjectiveType.OTHER);

            tFilterDrop.Items.AddRange(tFilterOptions);

            tFilter.Text = "Type Filter";
            tFilter.TextAlign = ContentAlignment.MiddleCenter;
            tFilter.DropDown = tFilterDrop;
            tFilter.DropDownDirection = ToolStripDropDownDirection.BelowRight;

            tFilter.DropDownItemClicked += (o, e) => {
                foreach (ToolStripItem i in ((ToolStripDropDownButton)o).DropDownItems)
                    ((ToolStripButton)i).Checked = e.ClickedItem == i;
            };

            return tFilter;
        }

        private ToolStripDropDownButton CreateSortDropdown()
        {
            ToolStripDropDownButton oSort = new ToolStripDropDownButton();
            ToolStripDropDown oSortDrop = new ToolStripDropDown();
            ToolStripButton[] oSortOptions = new ToolStripButton[5];

            oSortOptions[0] = new ToolStripButton { Text = "Default", Checked = true };
            oSortOptions[0].Click += (o, e) => _objectiveList.SetSortTo(ObjectiveList.SortType.DEFAULT);
            oSortOptions[1] = new ToolStripButton { Text = "Date Created" };
            oSortOptions[1].Click += (o, e) => _objectiveList.SetSortTo(ObjectiveList.SortType.DATE_CREATED);
            oSortOptions[2] = new ToolStripButton { Text = "Date Edited" };
            oSortOptions[2].Click += (o, e) => _objectiveList.SetSortTo(ObjectiveList.SortType.DATE_EDITED);
            oSortOptions[3] = new ToolStripButton { Text = "Date Due" };
            oSortOptions[3].Click += (o, e) => _objectiveList.SetSortTo(ObjectiveList.SortType.DATE_DUE);
            oSortOptions[4] = new ToolStripButton { Text = "Alphabetical" };
            oSortOptions[4].Click += (o, e) => _objectiveList.SetSortTo(ObjectiveList.SortType.ALPHABETICAL);

            oSortDrop.Items.AddRange(oSortOptions);

            oSort.Text = "Sort";
            oSort.TextAlign = ContentAlignment.MiddleCenter;
            oSort.DropDown = oSortDrop;
            oSort.DropDownDirection = ToolStripDropDownDirection.BelowRight;

            oSort.DropDownItemClicked += (o, e) => {
                foreach (ToolStripItem i in ((ToolStripDropDownButton)o).DropDownItems)
                    ((ToolStripButton)i).Checked = e.ClickedItem == i;
            };

            return oSort;
        }

        private ToolStripDropDownButton CreatePriorityDropdown()
        {
            ToolStripDropDownButton pSort = new ToolStripDropDownButton();
            ToolStripDropDown pSortDrop = new ToolStripDropDown();
            ToolStripButton[] pSortOptions = new ToolStripButton[6];

            pSortOptions[0] = new ToolStripButton { Text = "All", Checked = true };
            pSortOptions[0].Click += (o, e) => _objectiveList.SetPriorityTo(Objective.Priority.ALL);
            pSortOptions[1] = new ToolStripButton { Text = "Critical" };
            pSortOptions[1].Click += (o, e) => _objectiveList.SetPriorityTo(Objective.Priority.CRITICAL);
            pSortOptions[2] = new ToolStripButton { Text = "High" };
            pSortOptions[2].Click += (o, e) => _objectiveList.SetPriorityTo(Objective.Priority.HIGH);
            pSortOptions[3] = new ToolStripButton { Text = "Medium" };
            pSortOptions[3].Click += (o, e) => _objectiveList.SetPriorityTo(Objective.Priority.MEDIUM);
            pSortOptions[4] = new ToolStripButton { Text = "Low" };
            pSortOptions[4].Click += (o, e) => _objectiveList.SetPriorityTo(Objective.Priority.LOW);
            pSortOptions[5] = new ToolStripButton { Text = "None" };
            pSortOptions[5].Click += (o, e) => _objectiveList.SetPriorityTo(Objective.Priority.NONE);

            pSortDrop.Items.AddRange(pSortOptions);

            pSort.Text = "Priority";
            pSort.TextAlign = ContentAlignment.MiddleCenter;
            pSort.DropDown = pSortDrop;
            pSort.DropDownDirection = ToolStripDropDownDirection.BelowRight;

            pSort.DropDownItemClicked += (o, e) => {
                foreach (ToolStripItem i in ((ToolStripDropDownButton)o).DropDownItems)
                    ((ToolStripButton)i).Checked = e.ClickedItem == i;
            };

            return pSort;
        }

        private void ObjectiveViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_subWindow != null)
                _subWindow.Dispose();

            while (!save.Enabled) ;
            save.PerformClick();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S) && save.Enabled)
                save.PerformClick();

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void EditObjectiveDisplay(Objective edit, bool useDefaults = false)
        {
            Form subWindow = new Form
            {
                ClientSize = new Size(400, 400),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                ControlBox = false,
                ShowInTaskbar = false,
                TopMost = true,
                StartPosition = FormStartPosition.CenterScreen,
                BackColor = Color.FromArgb(252, 210, 153)
            };

            TableLayoutPanel editPanel = new TableLayoutPanel
            {
                RowCount = 8,
                ColumnCount = 8,
                Dock = DockStyle.Fill
            };
            for (int i = 0; i < 8; i++)
            {
                editPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5f));
                editPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
            }

            TextBox name = new TextBox
            {
                MaxLength = 100,
                Text = useDefaults ? "Enter name here" : edit.Name,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 232, 204)
            };
            name.GotFocus += (o, e) => {
                if (name.Text.Equals("Enter name here"))
                    name.Text = "";
            };
            name.LostFocus += (o, e) => {
                if (name.Text.Equals(""))
                    name.Text = "Enter name here";
            };
            editPanel.Controls.Add(name);
            editPanel.SetCellPosition(name, new TableLayoutPanelCellPosition(0, 0));
            editPanel.SetRowSpan(name, 1);
            editPanel.SetColumnSpan(name, 8);

            TextBox description = new TextBox
            {
                MaxLength = 1200,
                Text = useDefaults ? "Enter description here" : edit.Description,
                Multiline = true,
                WordWrap = true,
                AcceptsReturn = true,
                AcceptsTab = true,
                TextAlign = HorizontalAlignment.Left,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 232, 204)
            };
            description.GotFocus += (o, e) => {
                if (description.Text.Equals("Enter description here"))
                    description.Text = "";
            };
            description.LostFocus += (o, e) => {
                if (description.Text.Equals(""))
                    description.Text = "Enter description here";
            };
            editPanel.Controls.Add(description);
            editPanel.SetCellPosition(description, new TableLayoutPanelCellPosition(0, 1));
            editPanel.SetRowSpan(description, 3);
            editPanel.SetColumnSpan(description, 8);

            DateTimePicker dueDate = new DateTimePicker
            {
                Value = useDefaults ? DateTime.Now.AddDays(1) : edit.DueBy.Date,
                MinDate = DateTime.Now,
                Format = DateTimePickerFormat.Short,
                ShowUpDown = true,
                BackColor = Color.FromArgb(255, 232, 204)
            };
            editPanel.Controls.Add(dueDate);
            editPanel.SetCellPosition(dueDate, new TableLayoutPanelCellPosition(0, 4));
            editPanel.SetRowSpan(dueDate, 1);
            editPanel.SetColumnSpan(dueDate, 2);

            DateTimePicker dueTime = new DateTimePicker
            {
                Value = useDefaults ? DateTime.Now + TimeSpan.FromHours(1) : edit.DueBy,
                MinDate = DateTime.Now,
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                BackColor = Color.FromArgb(255, 232, 204)
            };
            editPanel.Controls.Add(dueTime);
            editPanel.SetCellPosition(dueTime, new TableLayoutPanelCellPosition(2, 4));
            editPanel.SetRowSpan(dueTime, 1);
            editPanel.SetColumnSpan(dueTime, 2);

            CheckBox persists = new CheckBox
            {
                Checked = !useDefaults && edit.Persist,
                Text = "Persists",
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(252, 210, 153)
            };
            editPanel.Controls.Add(persists);
            editPanel.SetCellPosition(persists, new TableLayoutPanelCellPosition(4, 4));
            editPanel.SetRowSpan(persists, 1);
            editPanel.SetColumnSpan(persists, 2);

            CheckBox repeats = new CheckBox
            {
                Checked = !useDefaults && edit.Repeat,
                Text = "Repeats",
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(252, 210, 153)
            };
            repeats.CheckedChanged += (o, e) => {
                if (repeats.Checked)
                    persists.Checked = true;
            };
            editPanel.Controls.Add(repeats);
            editPanel.SetCellPosition(repeats, new TableLayoutPanelCellPosition(6, 4));
            editPanel.SetRowSpan(repeats, 1);
            editPanel.SetColumnSpan(repeats, 2);

            ComboBox typeOptions = new ComboBox
            {
                Text = "Type",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 232, 204)
            };
            typeOptions.Items.AddRange(new string[] { "Homework", "Job", "Club", "Project", "Personal", "Free Time", "Note", "Reminder", "Other" });
            typeOptions.SelectedItem = useDefaults ? "" : ObjectiveViewPanel.ConvertType(edit.Type);
            editPanel.Controls.Add(typeOptions);
            editPanel.SetCellPosition(typeOptions, new TableLayoutPanelCellPosition(0, 5));
            editPanel.SetRowSpan(typeOptions, 1);
            editPanel.SetColumnSpan(typeOptions, 3);

            ComboBox priorityOptions = new ComboBox
            {
                Text = "Priority",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 232, 204)
            };
            priorityOptions.Items.AddRange(new string[] { "Critical", "High", "Medium", "Low", "None" });
            priorityOptions.SelectedItem = useDefaults ? "" : ObjectiveViewPanel.ConvertPriority(edit.Importance);
            editPanel.Controls.Add(priorityOptions);
            editPanel.SetCellPosition(priorityOptions, new TableLayoutPanelCellPosition(5, 5));
            editPanel.SetRowSpan(priorityOptions, 1);
            editPanel.SetColumnSpan(priorityOptions, 3);

            Button finished = new Button
            {
                Text = useDefaults ? "Create" : "Edit",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 252, 153)
            };
            finished.Click += (o, e) => {
                Objective newObj = new Objective();

                if (useDefaults)
                    newObj.Created = DateTime.Now;
                else
                    newObj.Created = edit.Created;

                newObj.LastEdited = DateTime.Now;
                newObj.DueBy = dueDate.Value.Date + dueTime.Value.TimeOfDay;

                newObj.Name = name.Text;
                newObj.Description = description.Text;
                newObj.Persist = persists.Checked;
                newObj.Repeat = repeats.Checked;

                if (typeOptions.SelectedItem == null)
                    newObj.Type = Objective.ObjectiveType.REMINDER;
                else
                    newObj.Type = TypeString((string)typeOptions.SelectedItem);

                if (typeOptions.SelectedItem == null)
                    newObj.Importance = Objective.Priority.LOW;
                else
                    newObj.Importance = PriorityString((string)priorityOptions.SelectedItem);

                if (useDefaults)
                    _objectiveList.AddObjective(newObj);
                else
                {
                    _objectiveList.DeleteObjective(edit);
                    _objectiveList.AddObjective(newObj);
                }

                subWindow.Close();
                _objectiveList.UpdateShownObjectives();
            };
            editPanel.Controls.Add(finished);
            editPanel.SetCellPosition(finished, new TableLayoutPanelCellPosition(0, 7));
            editPanel.SetRowSpan(finished, 1);
            editPanel.SetColumnSpan(finished, 3);

            Button cancel = new Button()
            {
                Text = "Cancel",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 161, 153)
            };
            cancel.Click += (o, e) => {
                subWindow.Dispose();
            };
            editPanel.Controls.Add(cancel);
            editPanel.SetCellPosition(cancel, new TableLayoutPanelCellPosition(5, 7));
            editPanel.SetRowSpan(cancel, 1);
            editPanel.SetColumnSpan(cancel, 3);

            subWindow.Controls.Add(editPanel);
            subWindow.Focus();
            _subWindow = subWindow;
            subWindow.Show();
        }
        public void AddObjectiveDisplay()
        {
            EditObjectiveDisplay(new Objective(), true);
        }

        private Objective.ObjectiveType TypeString(string str)
        {
            switch (str)
            {
                case "Homework":
                    return Objective.ObjectiveType.HOMEWORK;
                case "Job":
                    return Objective.ObjectiveType.JOB;
                case "Club":
                    return Objective.ObjectiveType.CLUB;
                case "Project":
                    return Objective.ObjectiveType.PROJECT_IDEA;
                case "Personal":
                    return Objective.ObjectiveType.PERSONAL;
                case "Free Time":
                    return Objective.ObjectiveType.FREE_TIME;
                case "Note":
                    return Objective.ObjectiveType.NOTE;
                case "Reminder":
                    return Objective.ObjectiveType.REMINDER;
                case "Other":
                    return Objective.ObjectiveType.OTHER;
                default:
                    return Objective.ObjectiveType.ALL;
            }
        }

        private Objective.Priority PriorityString(string str)
        {
            switch (str)
            {
                case "Critical":
                    return Objective.Priority.CRITICAL;
                case "High":
                    return Objective.Priority.HIGH;
                case "Medium":
                    return Objective.Priority.MEDIUM;
                case "Low":
                    return Objective.Priority.LOW;
                case "None":
                    return Objective.Priority.NONE;
                default:
                    return Objective.Priority.ALL;
            }
        }
    }
}
