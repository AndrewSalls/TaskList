using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskList
{
    public class ObjectiveViewPanel : TableLayoutPanel
    {
        public Objective Displayed { get; set; }
        private readonly ObjectiveList.DeleteObjectivePanel_Delegate _delete;
        private readonly ObjectiveList.EditObjectivePanel_Delegate _edit;

        public ObjectiveViewPanel(Objective display, ObjectiveList.EditObjectivePanel_Delegate editDelegate, ObjectiveList.DeleteObjectivePanel_Delegate deleteDelegate)
        {
            Displayed = display;
            BackColor = Color.FromArgb(242, 242, 151);
            AutoScroll = false;
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            RowCount = 9;
            ColumnCount = 8;
            for(int i = 0; i < 8; i++)
            {
                RowStyles.Add(new RowStyle(SizeType.Percent, 12.5f));
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5f));
            }

            _edit = editDelegate;
            _delete = deleteDelegate;
            InitControls();
        }

        private void InitControls()
        {
            Label title = new Label
            {
                Dock = DockStyle.Fill,
                Text = Displayed.Name,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Controls.Add(title);
            SetCellPosition(title, new TableLayoutPanelCellPosition(0, 0));
            SetRowSpan(title, 1);
            SetColumnSpan(title, 8);

            LockedTextBox info = new LockedTextBox
            {
                MaxLength = 1200,
                Text = Displayed.Description,
                TextAlign = HorizontalAlignment.Left,
                Dock = DockStyle.Fill,
                Multiline = true,
                WordWrap = true,
                AcceptsReturn = true,
                AcceptsTab = true,
                TabStop = false,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            Controls.Add(info);
            SetCellPosition(info, new TableLayoutPanelCellPosition(0, 1));
            SetRowSpan(info, 4);
            SetColumnSpan(info, 8);

            Label type = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Category: " + ConvertType(Displayed.Type),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Controls.Add(type);
            SetCellPosition(type, new TableLayoutPanelCellPosition(0, 5));
            SetRowSpan(type, 1);
            SetColumnSpan(type, 3);

            Label priority = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Priority: " + ConvertPriority(Displayed.Importance),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Controls.Add(priority);
            SetCellPosition(priority, new TableLayoutPanelCellPosition(5, 5));
            SetRowSpan(priority, 1);
            SetColumnSpan(priority, 3);

            Label cDate = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Created: " + Displayed.Created.ToString(),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Controls.Add(cDate);
            SetCellPosition(cDate, new TableLayoutPanelCellPosition(0, 6));
            SetRowSpan(cDate, 2);
            SetColumnSpan(cDate, 2);

            Label eDate = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Edited: " + Displayed.LastEdited.ToString(),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Controls.Add(eDate);
            SetCellPosition(eDate, new TableLayoutPanelCellPosition(3, 6));
            SetRowSpan(eDate, 2);
            SetColumnSpan(eDate, 2);

            Label dDate = new Label()
            {
                Dock = DockStyle.Fill,
                Text = "Due: " + Displayed.Created.ToString(),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Controls.Add(dDate);
            SetCellPosition(dDate, new TableLayoutPanelCellPosition(6, 6));
            SetRowSpan(dDate, 2);
            SetColumnSpan(dDate, 2);

            Button edit = new Button()
            {
                BackColor = Color.FromArgb(236, 236, 85),
                Dock = DockStyle.Fill,
                Text = "Edit",
                TextAlign = ContentAlignment.MiddleCenter
            };
            edit.Click += (o, e) => {
                _edit.Invoke(Displayed, false);
                Update();
            };

            Controls.Add(edit);
            SetCellPosition(edit, new TableLayoutPanelCellPosition(0, 8));
            SetRowSpan(edit, 1);
            SetColumnSpan(edit, 3);

            Button delete = new Button
            {
                BackColor = Color.FromArgb(236, 236, 85),
                Dock = DockStyle.Fill,
                Text = "Delete",
                TextAlign = ContentAlignment.MiddleCenter
            };
            delete.Click += (o, e) => _delete.Invoke();

            Controls.Add(delete);
            SetCellPosition(delete, new TableLayoutPanelCellPosition(5, 8));
            SetRowSpan(delete, 1);
            SetColumnSpan(delete, 3);
        }

        public static string ConvertType(Objective.ObjectiveType type)
        {
            switch(type)
            {
                case Objective.ObjectiveType.HOMEWORK:
                    return "Homework";
                case Objective.ObjectiveType.PROJECT_IDEA:
                    return "Project";
                case Objective.ObjectiveType.CLUB:
                    return "Club";
                case Objective.ObjectiveType.PERSONAL:
                    return "Personal";
                case Objective.ObjectiveType.FREE_TIME:
                    return "Free Time";
                case Objective.ObjectiveType.JOB:
                    return "Job";
                case Objective.ObjectiveType.NOTE:
                    return "Note";
                case Objective.ObjectiveType.REMINDER:
                    return "Reminder";
                case Objective.ObjectiveType.OTHER:
                    return "Other";
                case Objective.ObjectiveType.ALL:
                default:
                    return "???";
            }
        }
        public static string ConvertPriority(Objective.Priority priority)
        {
            switch (priority)
            {
                case Objective.Priority.NONE:
                    return "None";
                case Objective.Priority.LOW:
                    return "Low";
                case Objective.Priority.MEDIUM:
                    return "Medium";
                case Objective.Priority.HIGH:
                    return "High";
                case Objective.Priority.CRITICAL:
                    return "Critical";
                case Objective.Priority.ALL:
                default:
                    return "???";
            }
        }

        private class LockedTextBox : TextBox
        {
            protected override void WndProc(ref Message m)
            {
                // Send WM_MOUSEWHEEL messages to the parent
                if (m.Msg == 0x20a) SendMessage(Parent.Handle, m.Msg, m.WParam, m.LParam);
                else base.WndProc(ref m);
            }
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        }
    }
}
