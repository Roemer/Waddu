using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Waddu.Controls
{
    /// <summary>
    /// A visible Separator Control
    /// © 2006-2008
    /// </summary>
    public class SeparatorControl : Control
    {
        private Pen m_penGray = null;
        private Pen m_penWhite = null;

        private int m_lineLength = 0;
        private LineMode m_lineMode = 0;

        // Enums
        public enum LineMode
        {
            down,
            up,
            left,
            right
        }

        // Properties
        public int LineLength
        {
            get
            {
                return m_lineLength;
            }
            set
            {
                m_lineLength = value;
            }
        }

        [RefreshProperties(RefreshProperties.Repaint)]
        public LineMode Mode
        {
            get
            {
                return m_lineMode;
            }
            set
            {
                m_lineMode = value;
                Anchor = Anchor;
            }
        }

        [DefaultValue(false), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool TabStop
        {
            get
            {
                return base.TabStop;
            }
            set
            {
                base.TabStop = value;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
            }
        }

        [DefaultValue(""), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
            }
        }

        public new AnchorStyles Anchor
        {
            get
            {
                return base.Anchor;
            }
            set
            {
                bool oldTop = (base.Anchor & AnchorStyles.Top) == AnchorStyles.Top;
                bool oldBottom = (base.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom;
                bool oldLeft = (base.Anchor & AnchorStyles.Left) == AnchorStyles.Left;
                bool oldRight = (base.Anchor & AnchorStyles.Right) == AnchorStyles.Right;

                bool newTop = (value & AnchorStyles.Top) == AnchorStyles.Top;
                bool newBottom = (value & AnchorStyles.Bottom) == AnchorStyles.Bottom;
                bool newLeft = (value & AnchorStyles.Left) == AnchorStyles.Left;
                bool newRight = (value & AnchorStyles.Right) == AnchorStyles.Right;

                if (m_lineMode == LineMode.down || m_lineMode == LineMode.up)
                {
                    if (newTop && newBottom)
                    {
                        if (oldTop)
                        {
                            if (oldBottom)
                            {
                                value &= ~AnchorStyles.Bottom;
                            }
                            else
                            {
                                value &= ~AnchorStyles.Top;
                            }
                        }
                        else
                        {
                            value &= ~AnchorStyles.Bottom;
                        }
                    }
                }

                if (m_lineMode == LineMode.left || m_lineMode == LineMode.right)
                {
                    if (newLeft && newRight)
                    {
                        if (oldLeft)
                        {
                            if (oldRight)
                            {
                                value &= ~AnchorStyles.Right;
                            }
                            else
                            {
                                value &= ~AnchorStyles.Left;
                            }
                        }
                        else
                        {
                            value &= ~AnchorStyles.Right;
                        }
                    }
                }

                base.Anchor = value;
            }
        }

        public SeparatorControl()
        {
            TabStop = false;
            m_lineLength = 40;
            m_lineMode = LineMode.down;
            m_penGray = new Pen(SystemColors.ControlDark, 1);
            m_penWhite = new Pen(Color.White, 1);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (m_lineMode == LineMode.down || m_lineMode == LineMode.up)
            {
                m_lineLength = this.Width;
            }
            else
            {
                m_lineLength = this.Height;
            }
            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            if (this.Visible == true)
            {
                if (m_lineMode == LineMode.down)
                {
                    e.Graphics.DrawLine(m_penGray, new Point(0, 0), new Point(m_lineLength, 0));
                    e.Graphics.DrawLine(m_penWhite, new Point(0, 0 + 1), new Point(m_lineLength, 0 + 1));
                    this.Size = new Size(m_lineLength, 2);
                }
                else if (m_lineMode == LineMode.up)
                {
                    e.Graphics.DrawLine(m_penGray, new Point(0, 0 + 1), new Point(m_lineLength, 0 + 1));
                    e.Graphics.DrawLine(m_penWhite, new Point(0, 0), new Point(m_lineLength, 0));
                    this.Size = new Size(m_lineLength, 2);
                }
                else if (m_lineMode == LineMode.left)
                {
                    e.Graphics.DrawLine(m_penGray, new Point(0, 0), new Point(0, m_lineLength));
                    e.Graphics.DrawLine(m_penWhite, new Point(0 + 1, 0), new Point(0 + 1, m_lineLength));
                    this.Size = new Size(2, m_lineLength);
                }
                else if (m_lineMode == LineMode.right)
                {
                    e.Graphics.DrawLine(m_penGray, new Point(0 + 1, 0), new Point(0 + 1, m_lineLength));
                    e.Graphics.DrawLine(m_penWhite, new Point(0, 0), new Point(0, m_lineLength));
                    this.Size = new Size(2, m_lineLength);
                }
            }

            base.OnPaint(e);
        }

    }
}
