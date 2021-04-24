using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TaskList
{
    public class ObjectiveList : TableLayoutPanel
    {
        public enum SortType
        {
            ALPHABETICAL, DATE_CREATED, DATE_EDITED, DATE_DUE, DEFAULT
        }

        private readonly List<ObjectiveViewPanel> _objectives;
        private readonly string _savePoint;

        private Objective.ObjectiveType _filterType;
        private SortType _sortType;
        private Objective.Priority _prioritySortType;
        private bool _aToz;

        private readonly EditObjectivePanel_Delegate _editDelegate;

        public ObjectiveList(string fileName, EditObjectivePanel_Delegate ed)
        {
            AutoScroll = true;
            RowCount = 0;
            Dock = DockStyle.Fill;
            ColumnCount = 3;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80f));
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10f));

            _savePoint = fileName;
            _filterType = Objective.ObjectiveType.ALL;
            _sortType = SortType.DEFAULT;
            _aToz = true;
            _prioritySortType = Objective.Priority.ALL;

            _objectives = FileManager.DeserializeData(_savePoint).Select(d => new ObjectiveViewPanel(d, ed, () => DeleteObjective(d))).ToList();
            _objectives.RemoveAll(p => !p.Displayed.Persist && p.Displayed.DueBy < DateTime.Now);
            _objectives.ForEach(o => {
                if (o.Displayed.Repeat)
                {
                    while (o.Displayed.DueBy < DateTime.Now)
                        o.Displayed.DueBy += TimeSpan.FromDays(1);
                }
            });
            
            _editDelegate = ed;
            UpdateShownObjectives();
        }

        public void SetFilterTo(Objective.ObjectiveType type)
        {
            _filterType = type;
            UpdateShownObjectives();
        }

        public void SetSortTo(SortType type)
        {
            _sortType = type;
            UpdateShownObjectives();
        }

        public void SetSortDirection(bool AtoZ)
        {
            _aToz = AtoZ;
            UpdateShownObjectives();
        }

        public void SetPriorityTo(Objective.Priority type)
        {
            _prioritySortType = type;
            UpdateShownObjectives();
        }

        public void SaveObjectives()
        {
            FileManager.SerializeData(_objectives.Select(o => o.Displayed).ToList(), _savePoint);
        }

        public void UpdateShownObjectives()
        {
            Controls.Clear();
            RowStyles.Clear();
            IEnumerable<ObjectiveViewPanel> toAdd = Sort(_objectives.Where(GetPriorityMethod()).Where(GetSortMethod()));
            RowCount = 0;
            foreach (ObjectiveViewPanel o in toAdd)
            {
                RowStyles.Add(new RowStyle(SizeType.Absolute, 300f));
                RowCount += 1;
                Controls.Add(o);
                SetCellPosition(o, new TableLayoutPanelCellPosition(1, RowCount - 1));
                SetRowSpan(o, 1);
                SetColumnSpan(o, 1);
            }
            if(Controls.Count == 1)
            {
                RowStyles.Add(new RowStyle(SizeType.Absolute, 273f));
                RowCount += 1;
            }
        }

        public void AddObjective(Objective o)
        {
            _objectives.Add(new ObjectiveViewPanel(o, _editDelegate, () => DeleteObjective(o)));
            UpdateShownObjectives();
        }
        public void DeleteObjective(Objective o)
        {
            _objectives.RemoveAll(p => p.Displayed.Equals(o));
            UpdateShownObjectives();
        }

        private Func<ObjectiveViewPanel, bool> GetSortMethod()
        {
            if (_filterType == Objective.ObjectiveType.ALL)
                return o => true;

            return o => o.Displayed.Type == _filterType;
        }

        private Func<ObjectiveViewPanel, bool> GetPriorityMethod()
        {
            switch(_prioritySortType)
            {
                case Objective.Priority.CRITICAL:
                    return o => o.Displayed.Importance == Objective.Priority.CRITICAL;
                case Objective.Priority.HIGH:
                    return o => o.Displayed.Importance == Objective.Priority.HIGH;
                case Objective.Priority.MEDIUM:
                    return o => o.Displayed.Importance == Objective.Priority.MEDIUM;
                case Objective.Priority.LOW:
                    return o => o.Displayed.Importance == Objective.Priority.LOW;
                case Objective.Priority.NONE:
                    return o => o.Displayed.Importance == Objective.Priority.NONE;
                case Objective.Priority.ALL:
                default:
                    return o => true;
            }
        }

        private IEnumerable<ObjectiveViewPanel> Sort(IEnumerable<ObjectiveViewPanel> _list)
        {
            if(_aToz)
            {
                switch(_sortType)
                {
                    case SortType.ALPHABETICAL:
                        return _list.OrderByDescending(o => o.Displayed.Name);
                    case SortType.DATE_CREATED:
                        return _list.OrderByDescending(o => o.Displayed.Created);
                    case SortType.DATE_EDITED:
                        return _list.OrderByDescending(o => o.Displayed.LastEdited);
                    case SortType.DATE_DUE:
                        return _list.OrderByDescending(o => o.Displayed.DueBy);
                    case SortType.DEFAULT:
                        return _list.Reverse();
                }
            }

            switch(_sortType)
            {
                case SortType.ALPHABETICAL:
                    return _list.OrderBy(o => o.Displayed.Name);
                case SortType.DATE_CREATED:
                    return _list.OrderBy(o => o.Displayed.Created);
                case SortType.DATE_EDITED:
                    return _list.OrderBy(o => o.Displayed.LastEdited);
                case SortType.DATE_DUE:
                    return _list.OrderBy(o => o.Displayed.DueBy);
                case SortType.DEFAULT:
                default:
                    return _list;
            }
        }

        public delegate void DeleteObjectivePanel_Delegate();
        public delegate void EditObjectivePanel_Delegate(Objective toEdit, bool isNewObjective);
    }
}
