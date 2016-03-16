using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Waddu.UI.Controls
{
    /// <summary>
    /// An extended Split Container with nicer Graphics
    /// © 2006-2008
    /// </summary>
    public class SplitContainerEx : SplitContainer
    {
        // Enums
        public enum LineMode
        {
            Hidden = 0,
            Normal = 1,
            Inverted = 2
        }

        public enum Panels
        {
            None = 0,
            Panel1 = 1,
            Panel2 = 2
        }

        private LineMode m_modeTopLeftLine = LineMode.Normal;
        private LineMode m_modeBottomRightLine = LineMode.Normal;
        private LineMode m_modeCenterLine = LineMode.Hidden;
        private LineMode m_modeDragLines = LineMode.Normal;
        private int m_dragLineWidth = 40;
        private int m_dragLineOffset;
        private int m_panel1MaxSize;
        private int m_panel2MaxSize;
        private Pen m_penGray = new Pen(SystemColors.ControlDark, 1);
        private Pen m_penWhite = new Pen(Color.White, 1);
        private int m_mouseDelta;
        private int m_splitterDistanceBeforeCollapse;
        private int m_PanelSizeBeforeCollapse;
        private Panels m_alternativeCollapsePanel;
        private bool m_alternativeCollapsedDefault;
        private bool m_alternativeCollapsed;

        // Properties
        [Browsable(true), Category("Extended Design"),
        Description("Maximum Size of the first Panel (0 for unlimited)")]
        public int Panel1MaxSize
        {
            get { return m_panel1MaxSize; }
            set { m_panel1MaxSize = value; }
        }
        [Browsable(true), Category("Extended Design"),
       Description("Maximum Size of the second Panel (0 for unlimited)")]
        public int Panel2MaxSize
        {
            get { return m_panel2MaxSize; }
            set { m_panel2MaxSize = value; }
        }
        [Browsable(true), Category("Extended Design"),
        Description("The Length of the Drag Lines (in Pixels)")]
        public int DragLineWidth
        {
            get { return m_dragLineWidth; }
            set { m_dragLineWidth = value; }
        }
        [Browsable(true), Category("Extended Design"),
        Description("An Offset for the Drag Lines (in Pixels)")]
        public int DragLineOffset
        {
            get { return m_dragLineOffset; }
            set { m_dragLineOffset = value; }
        }
        [Browsable(true), Category("Extended Design"),
        Description("Visual Mode of the Drag Lines")]
        public LineMode DragLines
        {
            get { return m_modeDragLines; }
            set { m_modeDragLines = value; }
        }
        [Browsable(true), Category("Extended Design"),
        Description("Visual Mode of the Line on Top (Horizonal) or Left (Vertical)")]
        public LineMode TopLeftLine
        {
            get { return m_modeTopLeftLine; }
            set { m_modeTopLeftLine = value; }
        }
        [Browsable(true), Category("Extended Design"),
        Description("Visual Mode of the Line on Bottom (Horizonal) or Right (Vertical)")]
        public LineMode BottomRightLine
        {
            get { return m_modeBottomRightLine; }
            set { m_modeBottomRightLine = value; }
        }
        [Browsable(true), Category("Extended Design"),
        Description("Visual Mode of the Center Line")]
        public LineMode CenterLine
        {
            get { return m_modeCenterLine; }
            set { m_modeCenterLine = value; }
        }
        [Browsable(true), Category("Extended Design"),
        Description("Alternative collapse panel")]
        public Panels AlternativeCollapsePanel
        {
            get { return m_alternativeCollapsePanel; }
            set { m_alternativeCollapsePanel = value; }
        }
        [Browsable(true), Category("Extended Design"),
        Description("Alternatively collapsed by default")]
        public bool AlternativeCollapseDefault
        {
            get { return m_alternativeCollapsedDefault; }
            set { m_alternativeCollapsedDefault = value; }
        }

        // Constructor
        public SplitContainerEx()
        {
            SplitterWidth = 20;

            SplitterMoved += SplitterMovedHandler;
            DoubleClick += SplitContainerEx_DoubleClick;

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ContainerControl, true);

        }

        protected override void OnCreateControl()
        {
            if (m_alternativeCollapsedDefault)
            {
                AlternativeCollapse();
            }
            base.OnCreateControl();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (m_alternativeCollapsed)
            {
                UpdateAlternativeCollapseSplitter();
            }
        }

        public void AlternativeCollapseToggle()
        {
            if (m_alternativeCollapsed)
            {
                AlternativeCollapseRestore();
            }
            else
            {
                AlternativeCollapse();
            }
        }

        public void AlternativeCollapseRestore()
        {
            if (!DesignMode)
            {
                SplitterDistance = m_splitterDistanceBeforeCollapse;
                if (m_alternativeCollapsePanel == Panels.Panel1)
                {
                    Panel1MinSize = m_PanelSizeBeforeCollapse;
                }
                else
                {
                    Panel2MinSize = m_PanelSizeBeforeCollapse;
                }
                m_alternativeCollapsed = false;
            }
        }

        public void AlternativeCollapse()
        {
            if (!DesignMode)
            {
                m_splitterDistanceBeforeCollapse = SplitterDistance;

                if (m_alternativeCollapsePanel == Panels.Panel1)
                {
                    m_PanelSizeBeforeCollapse = Panel1MinSize;
                    Panel1MinSize = 0;
                }
                else
                {
                    m_PanelSizeBeforeCollapse = Panel2MinSize;
                    Panel2MinSize = 0;
                }

                UpdateAlternativeCollapseSplitter();

                m_alternativeCollapsed = true;
            }
        }

        private void UpdateAlternativeCollapseSplitter()
        {
            if (!DesignMode)
            {
                if (m_alternativeCollapsePanel == Panels.Panel1)
                {
                    SplitterDistance = 0;
                }
                else
                {
                    if (Orientation == Orientation.Horizontal)
                    {
                        SplitterDistance = Height;
                    }
                    else
                    {
                        SplitterDistance = Width;
                    }
                }
            }
        }

        private void SplitContainerEx_DoubleClick(object sender, EventArgs e)
        {
            if (m_alternativeCollapsePanel != Panels.None)
            {
                AlternativeCollapseToggle();
            }
        }

        private void SplitterMovedHandler(Object sender, SplitterEventArgs e)
        {
            SplitterDistance = SplitterDistance;
        }

        #region Drawing Stuff
        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.Clear(BackColor);

            // Get the current Splitter Rectangle
            var splitRect = SplitterRectangle;

            // Check Orientation
            if (Orientation == Orientation.Horizontal)
            {
                var centerStart = splitRect.Top + splitRect.Height / 2 - 1;
                // Center Line
                if (m_modeCenterLine != LineMode.Hidden)
                {
                    DrawLine(e.Graphics, splitRect.Left, centerStart, splitRect.Width, m_modeCenterLine);
                }

                // Top Left Line           
                if ((m_modeTopLeftLine != LineMode.Hidden) && (m_alternativeCollapsed == false || m_alternativeCollapsePanel == Panels.Panel2))
                {
                    DrawLine(e.Graphics, splitRect.Left, splitRect.Top, splitRect.Width, m_modeTopLeftLine);
                }

                // Bottom Right Line
                if ((m_modeBottomRightLine != LineMode.Hidden) && (m_alternativeCollapsed == false || m_alternativeCollapsePanel == Panels.Panel1))
                {
                    DrawLine(e.Graphics, splitRect.Left, splitRect.Bottom - 2, splitRect.Width, m_modeBottomRightLine);
                }

                // Drag Lines
                if (m_modeDragLines != LineMode.Hidden)
                {
                    // Calculate x-Position where to start
                    var xPos = splitRect.Width / 2 - m_dragLineWidth / 2;
                    for (var i = -1; i <= 1; i++)
                    {
                        DrawLine(e.Graphics, xPos, centerStart + m_dragLineOffset + i * 3, m_dragLineWidth, m_modeDragLines);
                    }
                }
            }
            else
            {
                var centerStart = splitRect.Left + splitRect.Width / 2 - 1;
                // Center Line
                if (m_modeCenterLine != LineMode.Hidden)
                {
                    DrawLine(e.Graphics, centerStart, splitRect.Top, splitRect.Height, m_modeCenterLine);
                }

                // Top Left Line
                if ((m_modeTopLeftLine != LineMode.Hidden) && (m_alternativeCollapsed == false || m_alternativeCollapsePanel == Panels.Panel2))
                {
                    DrawLine(e.Graphics, splitRect.Left, splitRect.Top, splitRect.Height, m_modeTopLeftLine);
                }

                // Bottom Right Line
                if ((m_modeBottomRightLine != LineMode.Hidden) && (m_alternativeCollapsed == false || m_alternativeCollapsePanel == Panels.Panel1))
                {
                    DrawLine(e.Graphics, splitRect.Right - 2, splitRect.Top, splitRect.Height, m_modeBottomRightLine);
                }

                // Drag Lines
                if (m_modeDragLines != LineMode.Hidden)
                {
                    // Calculate y-Position where to start
                    var yPos = splitRect.Height / 2 - m_dragLineWidth / 2;
                    for (var i = -1; i <= 1; i++)
                    {
                        DrawLine(e.Graphics, centerStart + m_dragLineOffset + i * 3, yPos, m_dragLineWidth, m_modeDragLines);
                    }
                }
            }

            if (Focused)
            {
                DrawFocus();
            }
        }
        private void DrawLine(Graphics g, int xFrom, int yFrom, int size, LineMode lm)
        {
            Pen pen1 = null;
            Pen pen2 = null;

            if (lm == LineMode.Normal)
            {
                pen1 = m_penGray;
                pen2 = m_penWhite;
            }
            else
            {
                pen1 = m_penWhite;
                pen2 = m_penGray;
            }

            if (Orientation == Orientation.Horizontal)
            {
                g.DrawLine(pen1, new Point(xFrom, yFrom), new Point(xFrom + size, yFrom));
                g.DrawLine(pen2, new Point(xFrom, yFrom + 1), new Point(xFrom + size, yFrom + 1));
            }
            else
            {
                g.DrawLine(pen1, new Point(xFrom, yFrom), new Point(xFrom, yFrom + size));
                g.DrawLine(pen2, new Point(xFrom + 1, yFrom), new Point(xFrom + 1, yFrom + size));
            }
        }
        #endregion

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!IsSplitterFixed)
            {
                if (e.KeyData == Keys.Right || e.KeyData == Keys.Down)
                {
                    SplitterDistance += 1;
                }
                else if (e.KeyData == Keys.Left || e.KeyData == Keys.Up)
                {
                    SplitterDistance -= 1;
                }
                Invalidate();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            DrawFocus();
        }

        private void DrawFocus()
        {
            var r = SplitterRectangle;
            var g = Graphics.FromHwndInternal(Handle);
            r.Inflate(-1, -1);
            ControlPaint.DrawFocusRectangle(g, r, ForeColor, BackColor);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (Orientation.Equals(Orientation.Vertical))
            {
                m_mouseDelta = SplitterDistance - e.X;
                //Cursor.Current = System.Windows.Forms.Cursors.NoMoveHoriz;
                //Cursor.Current = System.Windows.Forms.Cursors.SizeWE;
                Cursor.Current = Cursors.VSplit;
            }
            else
            {
                m_mouseDelta = SplitterDistance - e.Y;
                //Cursor.Current = System.Windows.Forms.Cursors.NoMoveVert;
                //Cursor.Current = System.Windows.Forms.Cursors.SizeNS;
                Cursor.Current = Cursors.HSplit;
            }

            // This disables the normal move behavior
            IsSplitterFixed = true;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            m_mouseDelta = 0;
            // This allows the splitter to be moved normally again
            IsSplitterFixed = false;

            Cursor.Current = Cursors.Default;

            if (Focused)
            {
                DrawFocus();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Check to make sure the splitter won't be updated by the normal move behavior also
            if (IsSplitterFixed)
            {
                // Make sure that the button used to move the splitter is the left mouse button
                if (e.Button.Equals(MouseButtons.Left))
                {
                    // Checks to see if the splitter is aligned Vertically
                    if (Orientation.Equals(Orientation.Vertical))
                    {
                        // Only move the splitter if the mouse is within the appropriate bounds
                        if (e.X > 0 && e.X < Width)
                        {
                            if (m_alternativeCollapsed)
                            {
                                AlternativeCollapseRestore();
                            }

                            // Move the splitter
                            var newDistance = e.X + m_mouseDelta;
                            SplitterDistance = (newDistance <= 0 ? 0 : newDistance);
                        }
                    }
                    // If it isn't aligned verically then it must be horizontal
                    else
                    {
                        // Only move the splitter if the mouse is within the appropriate bounds
                        if (e.Y > 0 && e.Y < Height)
                        {
                            if (m_alternativeCollapsed)
                            {
                                AlternativeCollapseRestore();
                            }

                            // Move the splitter
                            var newDistance = e.Y + m_mouseDelta;
                            SplitterDistance = (newDistance <= 0 ? 0 : newDistance);
                        }
                    }
                }
                // If a button other than left is pressed or no button at all
                else
                {
                    // This allows the splitter to be moved normally again
                    IsSplitterFixed = false;
                }

                Refresh();
            }
        }

        // Override SplitterDistance and addidionally Handle the MaxSize
        public new int SplitterDistance
        {
            get
            {
                return base.SplitterDistance;
            }
            set
            {
                if (Orientation == Orientation.Vertical)
                {
                    if (value > m_panel1MaxSize && m_panel1MaxSize != 0)
                    {
                        value = m_panel1MaxSize;
                    }
                    if ((value + SplitterWidth) < (Width - m_panel2MaxSize) && m_panel2MaxSize != 0)
                    {
                        value = (Width - m_panel2MaxSize) - SplitterWidth;
                    }

                }
                else
                {
                    if (value > m_panel1MaxSize && m_panel1MaxSize != 0)
                    {
                        value = m_panel1MaxSize;
                    }
                    if ((value + SplitterWidth) < (Height - m_panel2MaxSize) && m_panel2MaxSize != 0)
                    {
                        value = (Height - m_panel2MaxSize) - SplitterWidth;
                    }
                }
                base.SplitterDistance = value;
            }
        }
    }
}
