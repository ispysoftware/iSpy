using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace iSpyApplication.Utilities
{
    public partial class ScreenArea : Form
    {
        #region:::::::::::::::::::::::::::::::::::::::::::Form level declarations:::::::::::::::::::::::::::::::::::::::::::

        public enum CursPos
        {

            WithinSelectionArea = 0,
            OutsideSelectionArea,
            TopLine,
            BottomLine,
            LeftLine,
            RightLine,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight

        }

        public enum ClickAction
        {

            NoClick = 0,
            Dragging,
            Outside,
            TopSizing,
            BottomSizing,
            LeftSizing,
            TopLeftSizing,
            BottomLeftSizing,
            RightSizing,
            TopRightSizing,
            BottomRightSizing

        }

        private readonly Screen _screen;

        public Rectangle Area = Rectangle.Empty;

        public ClickAction CurrentAction;
        public bool LeftButtonDown;
        public bool RectangleDrawn;
        public bool ReadyToDrag;

        public Point ClickPoint;
        public Point CurrentTopLeft;
        public Point CurrentBottomRight;
        public Point DragClickRelative;

        public int RectangleHeight;
        public int RectangleWidth;

        readonly Graphics g;
        readonly Pen _myPen = new Pen(Color.Black, 2);
        //readonly Pen _eraserPen = new Pen(Color.FromArgb(224, 224, 224), 1);

        protected override void OnMouseClick(MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {

                return;

            }

            base.OnMouseClick(e);

        }

        #endregion

        #region:::::::::::::::::::::::::::::::::::::::::::Mouse Event Handlers & Drawing Initialization:::::::::::::::::::::::::::::::::::::::::::
        public ScreenArea(Screen s, Rectangle area)
        {
            _screen = s;
            InitializeComponent();
            Left = s.WorkingArea.Left;
            Top = s.WorkingArea.Top;
            WindowState = FormWindowState.Maximized;
            MouseDown += mouse_Click;
            MouseDoubleClick += mouse_DClick;
            MouseUp += mouse_Up;
            MouseMove += mouse_Move;
            KeyUp += key_press;
            g = CreateGraphics();
            _myPen.DashStyle = DashStyle.Dash;
            Area = area;

        }
        #endregion



        public void SaveSelection(bool showCursor)
        {
            Area = new Rectangle(CurrentTopLeft.X, CurrentTopLeft.Y, CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);
            if (Area.Width % 2 != 0)
                Area.Width = Area.Width- 1;
            if (Area.Height % 2 != 0)
                Area.Height = Area.Height - 1;
            Close();

        }



        public void key_press(object sender, KeyEventArgs e)
        {

            if (e.KeyCode.ToString() == "S" && (RectangleDrawn && (CursorPosition() == CursPos.WithinSelectionArea || CursorPosition() == CursPos.OutsideSelectionArea)))
            {

                SaveSelection(true);
            }
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }

        }

        #region:::::::::::::::::::::::::::::::::::::::::::Mouse Buttons:::::::::::::::::::::::::::::::::::::::::::
        private void mouse_DClick(object sender, MouseEventArgs e)
        {

            if (RectangleDrawn && (CursorPosition() == CursPos.WithinSelectionArea || CursorPosition() == CursPos.OutsideSelectionArea))
            {

                SaveSelection(false);

            }

        }

        private void mouse_Click(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {

                SetClickAction();
                LeftButtonDown = true;
                ClickPoint = new Point(Cursor.Position.X - _screen.WorkingArea.X, Cursor.Position.Y - _screen.WorkingArea.Y);

                if (RectangleDrawn)
                {

                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    DragClickRelative.X = Cursor.Position.X - _screen.WorkingArea.X - CurrentTopLeft.X;
                    DragClickRelative.Y = Cursor.Position.Y - _screen.WorkingArea.Y - CurrentTopLeft.Y;

                }

            }
        }

        private void mouse_Up(object sender, MouseEventArgs e)
        {

            RectangleDrawn = true;
            LeftButtonDown = false;
            CurrentAction = ClickAction.NoClick;

        }
        #endregion



        private void mouse_Move(object sender, MouseEventArgs e)
        {

            if (LeftButtonDown && !RectangleDrawn)
            {

                DrawSelection();

            }

            if (RectangleDrawn)
            {

                CursorPosition();

                if (CurrentAction == ClickAction.Dragging)
                {

                    DragSelection();

                }

                if (CurrentAction != ClickAction.Dragging && CurrentAction != ClickAction.Outside)
                {

                    ResizeSelection();

                }

            }

        }



        private CursPos CursorPosition()
        {
            int x = Cursor.Position.X - _screen.WorkingArea.X;
            int y = Cursor.Position.Y - _screen.WorkingArea.Y;

            if (((x > CurrentTopLeft.X - 10 && x < CurrentTopLeft.X + 10)) && ((y > CurrentTopLeft.Y + 10) && (y < CurrentBottomRight.Y - 10)))
            {

                this.Cursor = Cursors.SizeWE;
                return CursPos.LeftLine;

            }
            if (((x >= CurrentTopLeft.X - 10 && x <= CurrentTopLeft.X + 10)) && ((y >= CurrentTopLeft.Y - 10) && (y <= CurrentTopLeft.Y + 10)))
            {

                this.Cursor = Cursors.SizeNWSE;
                return CursPos.TopLeft;

            }
            if (((x >= CurrentTopLeft.X - 10 && x <= CurrentTopLeft.X + 10)) && ((y >= CurrentBottomRight.Y - 10) && (y <= CurrentBottomRight.Y + 10)))
            {

                this.Cursor = Cursors.SizeNESW;
                return CursPos.BottomLeft;

            }
            if (((x > CurrentBottomRight.X - 10 && x < CurrentBottomRight.X + 10)) && ((y > CurrentTopLeft.Y + 10) && (y < CurrentBottomRight.Y - 10)))
            {

                this.Cursor = Cursors.SizeWE;
                return CursPos.RightLine;

            }
            if (((x >= CurrentBottomRight.X - 10 && x <= CurrentBottomRight.X + 10)) && ((y >= CurrentTopLeft.Y - 10) && (y <= CurrentTopLeft.Y + 10)))
            {

                this.Cursor = Cursors.SizeNESW;
                return CursPos.TopRight;

            }
            if (((x >= CurrentBottomRight.X - 10 && x <= CurrentBottomRight.X + 10)) && ((y >= CurrentBottomRight.Y - 10) && (y <= CurrentBottomRight.Y + 10)))
            {

                this.Cursor = Cursors.SizeNWSE;
                return CursPos.BottomRight;

            }
            if (((y > CurrentTopLeft.Y - 10) && (y < CurrentTopLeft.Y + 10)) && ((x > CurrentTopLeft.X + 10 && x < CurrentBottomRight.X - 10)))
            {

                this.Cursor = Cursors.SizeNS;
                return CursPos.TopLine;

            }
            if (((y > CurrentBottomRight.Y - 10) && (y < CurrentBottomRight.Y + 10)) && ((x > CurrentTopLeft.X + 10 && x < CurrentBottomRight.X - 10)))
            {

                this.Cursor = Cursors.SizeNS;
                return CursPos.BottomLine;

            }
            if (
                (x >= CurrentTopLeft.X + 10 && x <= CurrentBottomRight.X - 10) && (y >= CurrentTopLeft.Y + 10 && y <= CurrentBottomRight.Y - 10))
            {

                this.Cursor = Cursors.Hand;
                return CursPos.WithinSelectionArea;

            }

            this.Cursor = Cursors.No;
            return CursPos.OutsideSelectionArea;
        }

        private void SetClickAction()
        {

            switch (CursorPosition())
            {
                case CursPos.BottomLine:
                    CurrentAction = ClickAction.BottomSizing;
                    break;
                case CursPos.TopLine:
                    CurrentAction = ClickAction.TopSizing;
                    break;
                case CursPos.LeftLine:
                    CurrentAction = ClickAction.LeftSizing;
                    break;
                case CursPos.TopLeft:
                    CurrentAction = ClickAction.TopLeftSizing;
                    break;
                case CursPos.BottomLeft:
                    CurrentAction = ClickAction.BottomLeftSizing;
                    break;
                case CursPos.RightLine:
                    CurrentAction = ClickAction.RightSizing;
                    break;
                case CursPos.TopRight:
                    CurrentAction = ClickAction.TopRightSizing;
                    break;
                case CursPos.BottomRight:
                    CurrentAction = ClickAction.BottomRightSizing;
                    break;
                case CursPos.WithinSelectionArea:
                    CurrentAction = ClickAction.Dragging;
                    break;
                case CursPos.OutsideSelectionArea:
                    CurrentAction = ClickAction.Outside;
                    break;
            }

        }

        private void ResizeSelection()
        {
            int x = Cursor.Position.X - _screen.WorkingArea.X;
            int y = Cursor.Position.Y - _screen.WorkingArea.Y;

            if (CurrentAction == ClickAction.LeftSizing)
            {

                if (x < CurrentBottomRight.X - 10)
                {

                    //Erase the previous rectangle
                    g.Clear(BackColor);//.DrawRectangle(_eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = x;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    g.DrawRectangle(_myPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

                }

            }
            if (CurrentAction == ClickAction.TopLeftSizing)
            {

                if (x < CurrentBottomRight.X - 10 && y < CurrentBottomRight.Y - 10)
                {

                    //Erase the previous rectangle
                    g.Clear(BackColor); //g.DrawRectangle(_eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = x;
                    CurrentTopLeft.Y = y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    g.DrawRectangle(_myPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

                }
            }
            if (CurrentAction == ClickAction.BottomLeftSizing)
            {

                if (x < CurrentBottomRight.X - 10 && y > CurrentTopLeft.Y + 10)
                {

                    //Erase the previous rectangle
                    g.Clear(BackColor); //g.DrawRectangle(_eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.X = x;
                    CurrentBottomRight.Y = y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    g.DrawRectangle(_myPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

                }

            }
            if (CurrentAction == ClickAction.RightSizing)
            {

                if (x > CurrentTopLeft.X + 10)
                {

                    //Erase the previous rectangle
                    g.Clear(BackColor); //g.DrawRectangle(_eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = x;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    g.DrawRectangle(_myPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

                }
            }
            if (CurrentAction == ClickAction.TopRightSizing)
            {

                if (x > CurrentTopLeft.X + 10 && y < CurrentBottomRight.Y - 10)
                {

                    //Erase the previous rectangle
                    g.Clear(BackColor); //g.DrawRectangle(_eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = x;
                    CurrentTopLeft.Y = y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    g.DrawRectangle(_myPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

                }
            }
            if (CurrentAction == ClickAction.BottomRightSizing)
            {

                if (x > CurrentTopLeft.X + 10 && y > CurrentTopLeft.Y + 10)
                {

                    //Erase the previous rectangle
                    g.Clear(BackColor); //g.DrawRectangle(_eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.X = x;
                    CurrentBottomRight.Y = y;
                    RectangleWidth = CurrentBottomRight.X - CurrentTopLeft.X;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    g.DrawRectangle(_myPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

                }
            }
            if (CurrentAction == ClickAction.TopSizing)
            {

                if (y < CurrentBottomRight.Y - 10)
                {

                    //Erase the previous rectangle
                    g.Clear(BackColor); //g.DrawRectangle(_eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentTopLeft.Y = y;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    g.DrawRectangle(_myPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

                }
            }
            if (CurrentAction == ClickAction.BottomSizing)
            {

                if (y > CurrentTopLeft.Y + 10)
                {

                    //Erase the previous rectangle
                    g.Clear(BackColor); //g.DrawRectangle(_eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
                    CurrentBottomRight.Y = y;
                    RectangleHeight = CurrentBottomRight.Y - CurrentTopLeft.Y;
                    g.DrawRectangle(_myPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

                }

            }

        }

        private void DragSelection()
        {
            //Ensure that the rectangle stays within the bounds of the screen

            //Erase the previous rectangle
            g.Clear(BackColor); //g.DrawRectangle(_eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

            int x = Cursor.Position.X - _screen.WorkingArea.X;
            int y = Cursor.Position.Y - _screen.WorkingArea.Y;


            if (x - DragClickRelative.X > 0 && x - DragClickRelative.X + RectangleWidth < _screen.Bounds.Width)
            {

                CurrentTopLeft.X = x - DragClickRelative.X;
                CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;

            }
            else
                //Selection area has reached the right side of the screen
                if (x - DragClickRelative.X > 0)
                {

                    CurrentTopLeft.X = _screen.Bounds.Width - RectangleWidth;
                    CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;

                }
                //Selection area has reached the left side of the screen
                else
                {

                    CurrentTopLeft.X = 0;
                    CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;

                }

            if (y - DragClickRelative.Y > 0 && y - DragClickRelative.Y + RectangleHeight < _screen.Bounds.Height)
            {

                CurrentTopLeft.Y = y - DragClickRelative.Y;
                CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;

            }
            else
                //Selection area has reached the bottom of the screen
                if (y - DragClickRelative.Y > 0)
                {

                    CurrentTopLeft.Y = _screen.Bounds.Height - RectangleHeight;
                    CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;

                }
                //Selection area has reached the top of the screen
                else
                {

                    CurrentTopLeft.Y = 0;
                    CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;

                }

            //Draw a new rectangle
            g.DrawRectangle(_myPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);

        }

        private void DrawSelection()
        {

            this.Cursor = Cursors.Arrow;

            //Erase the previous rectangle
            g.Clear(BackColor); //g.DrawRectangle(_eraserPen, CurrentTopLeft.X, CurrentTopLeft.Y, CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);

            //Calculate X Coordinates
            int x = Cursor.Position.X - _screen.WorkingArea.X;
            int y = Cursor.Position.Y - _screen.WorkingArea.Y;

            if (x < ClickPoint.X)
            {

                CurrentTopLeft.X = x;
                CurrentBottomRight.X = ClickPoint.X;

            }
            else
            {

                CurrentTopLeft.X = ClickPoint.X;
                CurrentBottomRight.X = x;

            }

            //Calculate Y Coordinates
            if (y < ClickPoint.Y)
            {

                CurrentTopLeft.Y = y;
                CurrentBottomRight.Y = ClickPoint.Y;

            }
            else
            {

                CurrentTopLeft.Y = ClickPoint.Y;
                CurrentBottomRight.Y = y;

            }

            //Draw a new rectangle
            g.DrawRectangle(_myPen, CurrentTopLeft.X, CurrentTopLeft.Y, CurrentBottomRight.X - CurrentTopLeft.X, CurrentBottomRight.Y - CurrentTopLeft.Y);

        }

        private void ScreenArea_Load(object sender, EventArgs e)
        {
            if (Area != Rectangle.Empty)
            {
                CurrentTopLeft.X = Area.X;
                CurrentTopLeft.Y = Area.Y;
                RectangleWidth = Area.Width;
                RectangleHeight = Area.Height;
                CurrentBottomRight.X = CurrentTopLeft.X + RectangleWidth;
                CurrentBottomRight.Y = CurrentTopLeft.Y + RectangleHeight;
                ClickPoint = new Point(CurrentTopLeft.X, CurrentTopLeft.Y);
                RectangleDrawn = true;
                CurrentAction = ClickAction.NoClick;
            }
        }

        private void ScreenArea_Paint(object sender, PaintEventArgs e)
        {
            if (RectangleDrawn)
                g.DrawRectangle(_myPen, CurrentTopLeft.X, CurrentTopLeft.Y, RectangleWidth, RectangleHeight);
        }
    }
}
