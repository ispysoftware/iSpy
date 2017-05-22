using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public sealed class AutoCompleteTextbox : TextBox
    {
        #region Constructors

        // the constructor
        public AutoCompleteTextbox()
        {
            MinTypedCharacters = 2;
            AutoCompleteList = new List<TextEntry>();

            _listBox = new ListBox
                      {
                          Name = "SuggestionListBox",
                          Font = Font,
                          Visible = true
                      };
            MaxDropDownItems = 20;
            RowHeight = GetStringHeight("H");
            _panel = new Panel
                    {
                        Visible = false,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        ClientSize = new Size(1, 1),
                        Name = "SuggestionPanel",
                        Padding = new Padding(0, 0, 0, 0),
                        Margin = new Padding(0, 0, 0, 0),
                        BackColor = Color.Transparent,
                        ForeColor = Color.Transparent,
                        Text = ""
                    };
            _panel.PerformLayout();
            if (!_panel.Controls.Contains(_listBox))
            {
                _panel.Controls.Add(_listBox);
            }

            _listBox.Dock = DockStyle.Fill;
            _listBox.SelectionMode = SelectionMode.One;
            _listBox.KeyDown += ListBoxKeyDown;
            _listBox.MouseClick += ListBoxMouseClick;
            _listBox.MouseDoubleClick += ListBoxMouseDoubleClick;

            CurrentAutoCompleteList = new List<string>();
            _listBox.DataSource = CurrentAutoCompleteList;
            _oldText = Text;
        }
        #endregion Constructors

        #region Fields

        private readonly ListBox _listBox;

        private string _oldText;

        private readonly Panel _panel;

        private int _maxDropDownItems = 16;
        private int _maxHeight = 100;

        #endregion Fields

        #region Properties

        [Browsable(true)]
        public int MaxDropDownItems
        {
            get { return _maxDropDownItems; }
            set
            {
                _maxDropDownItems = value;
                CalculateMaxPanelHeight();
            }
        }

        private int RowHeight { get; }


        public List<TextEntry> AutoCompleteList;


        public class TextEntry
        {
            public readonly string Text;
            public readonly string TextUc;

            public TextEntry(string text)
            {
                Text = text;
                TextUc = text.ToUpper().Replace("-", "").Replace(" ", "");
            }

            public override string ToString()
            {
                return Text;
            }

            public bool Contains(string[] s)
            {
                return s.Aggregate(true, (current, t) => current && TextUc.Contains(t));
            }
        }


        [Browsable(true)]
        public int MinTypedCharacters { get; set; }

        public int SelectedIndex
        {
            get { return _listBox.SelectedIndex; }
            set
            {
                if (_listBox.Items.Count != 0)
                {
                    _listBox.SelectedIndex = value;
                }
            }
        }

        private List<string> CurrentAutoCompleteList { get; }

        private Form ParentForm => Parent.FindForm();

        #endregion Properties

        #region Methods

        private void CalculateMaxPanelHeight()
        {
            var measureString = "H\n";
            for (var counter = 1; counter < _maxDropDownItems; counter++)
            {
                measureString += "H\n";
            }
            _maxHeight = GetStringHeight(measureString);
        }


        private int GetStringHeight(string measureString)
        {
            using (var e = _listBox.CreateGraphics())
            {
                var stringFont = Font;
                var stringSize = e.MeasureString(measureString, stringFont);
                return (int)stringSize.Height;
            }
        }

        public void HideSuggestionListBox()
        {
            if (ParentForm != null)
            {
                _panel.Hide();
                if (ParentForm.Controls.Contains(_panel))
                {
                    ParentForm.Controls.Remove(_panel);
                }
            }
        }

        private bool _handlersSet;

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (!_handlersSet)
            {
                _panel.Font = Font;
                _listBox.Font = Font;
                _panel.Refresh();
                _panel.PerformLayout();
                ParentForm.ResizeBegin += StartResize;
                ParentForm.ResizeEnd += EndResize;
                _handlersSet = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs args)
        {
            if (args.KeyCode == Keys.Up)
            {
                MoveSelectionInListBox(SelectedIndex - 1);
                args.Handled = true;
            }
            else if (args.KeyCode == Keys.Down)
            {
                MoveSelectionInListBox(SelectedIndex + 1);
                args.Handled = true;
            }
            else if (args.KeyCode == Keys.PageUp)
            {
                MoveSelectionInListBox(SelectedIndex - 10);
                args.Handled = true;
            }
            else if (args.KeyCode == Keys.PageDown)
            {
                MoveSelectionInListBox(SelectedIndex + 10);
                args.Handled = true;
            }
            else if (args.KeyCode == Keys.Enter)
            {
                SelectItem();
                args.Handled = true;
            }
            else
            {
                base.OnKeyDown(args);
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (!_panel.ContainsFocus)
            {
                base.OnLostFocus(e);
                HideSuggestionListBox();
            }
        }

        protected override void OnTextChanged(EventArgs args)
        {
            if (!DesignMode)
                ShowSuggests();
            base.OnTextChanged(args);
            _oldText = Text;
        }


        private void ListBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            SelectItem();
            e.Handled = true;
        }

        private void ListBoxMouseClick(object sender, MouseEventArgs e)
        {
            SelectItem();
        }

        private void ListBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectItem();
        }

        private void MoveSelectionInListBox(int index)
        {
            if (index <= -1)
            {
                SelectedIndex = 0;
            }
            else
            {
                if (index > _listBox.Items.Count - 1)
                {
                    SelectedIndex = _listBox.Items.Count - 1;
                }
                else
                {
                    SelectedIndex = index;
                }
            }
        }

        private void SelectItem()
        {
            if ((_listBox.Items.Count > 0) && (SelectedIndex > -1))
            {
                Text = _listBox.SelectedItem.ToString();
                HideSuggestionListBox();
            }
        }

        private void ShowSuggests()
        {
            if (Text.Length >= MinTypedCharacters)
            {
                _panel.SuspendLayout();
                if ((Text.Length > 0) && (_oldText == Text.Substring(0, Text.Length - 1)))
                {
                    UpdateCurrentAutoCompleteList();
                }
                else if ((_oldText.Length > 0) && (Text == _oldText.Substring(0, _oldText.Length - 1)))
                {
                    UpdateCurrentAutoCompleteList();
                }
                else
                {
                    UpdateCurrentAutoCompleteList();
                }

                if ((CurrentAutoCompleteList != null) && CurrentAutoCompleteList.Count > 0)
                {
                    _panel.Show();
                    _panel.BringToFront();
                    Focus();
                }
                else
                {
                    HideSuggestionListBox();
                }
                _panel.ResumeLayout(true);
            }
            else
            {
                HideSuggestionListBox();
            }
        }

        private void UpdateCurrentAutoCompleteList()
        {
            CurrentAutoCompleteList.Clear();
            var words = Text.Replace("-","").ToUpper().Trim().Split(' ');
            foreach (var te in AutoCompleteList)
            {
                if (te.Contains(words))
                {
                    CurrentAutoCompleteList.Add(te.Text);
                }
            }

            UpdateListBoxItems();
        }

        private void UpdateListBoxItems()
        {
            if (ParentForm != null)
            {
                _panel.Width = Width;
                SetPanelHeight(_panel);
                SetPanelPosition(_panel);
                if (!ParentForm.Controls.Contains(_panel))
                {
                    ParentForm.Controls.Add(_panel);
                }
                ((CurrencyManager) _listBox.BindingContext[CurrentAutoCompleteList]).Refresh();
            }
        }


        private void SetPanelPosition(Panel panel)
        {
            var availableBelow = 0;
            var p = GetLocationRelativeToForm(this);


            var availableAbove = p.Y;
            availableBelow += ParentForm.Height - p.Y - Height;


            if (availableBelow > panel.Height)
            {
                panel.Location = p + new Size(0, Height);
            }
            else if (availableAbove > panel.Height)
            {
                panel.Location = new Point(p.X, p.Y - panel.Height + (_maxDropDownItems - panel.Height/RowHeight)/3);
            }
            else if (availableBelow > availableAbove)
            {
                panel.Height = availableBelow;
                panel.Location = p + new Size(0, Height);
            }
            else
            {
                panel.Height = availableAbove;
                panel.Location = new Point(p.X, p.Y - panel.Height + (_maxDropDownItems - panel.Height/RowHeight)/3);
            }
        }

        private Point GetLocationRelativeToForm(Control c)
        {
            var findForm = c.FindForm();
            if (findForm != null)
            {
                var locationOnForm = findForm.PointToClient(c.Parent.PointToScreen(c.Location));


                return locationOnForm;
            }
            return Point.Empty;
        }


        private void SetPanelHeight(Panel pnl)
        {
            var currentList = "H\n";


            if (CurrentAutoCompleteList.Count < _maxDropDownItems)
            {
                if (CurrentAutoCompleteList.Count > 0)
                {
                    for (var counter = 0; counter < CurrentAutoCompleteList.Count; counter += 1)
                    {
                        currentList += CurrentAutoCompleteList[counter] + "\n";
                    }
                    var listHeight = GetStringHeight(currentList);

                    pnl.Height = listHeight == 0 ? 0 : Math.Min(_maxHeight,listHeight);
                }
            }
            else
            {
                pnl.Height = _maxHeight;
            }
        }

        #endregion Methods

        #region Other

        private bool _resizing;

        private void StartResize(object sender, EventArgs e)
        {
            if (_panel.Visible)
            {
                _resizing = true;
                _panel.Visible = false;
            }
        }


        private void EndResize(object sender, EventArgs e)
        {
            if (_resizing)
            {
                _resizing = false;
                _panel.Width = Width;
                SetPanelHeight(_panel);
                SetPanelPosition(_panel);
                _panel.Visible = true;
            }
        }


        public new void Dispose()
        {
            ParentForm.ResizeBegin -= StartResize;
            ParentForm.ResizeEnd -= EndResize;
            base.Dispose(true);
        }

        #endregion Other
    }
}