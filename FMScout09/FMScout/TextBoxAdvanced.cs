using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace FMScout.Controls
{
    /// <summary>A special custom rounding GroupBox with many painting features.</summary>
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public class TextBoxAdvanced : System.Windows.Forms.RichTextBox
    {
        #region Variables

        //private System.Drawing.Color V_BackColor = SystemColors.Control;
        //private System.Drawing.Color V_BorderColor = SystemColors.Window;
        #endregion

        #region Properties

        /// <summary>This feature will paint the background color of the control.</summary>
        //[Category("Appearance"), Description("This feature will paint the background color of the control.")]
        //public override System.Drawing.Color BackColor { get { return V_BackColor; } set { V_BackColor = value; this.Refresh(); } }

        /// <summary>This feature will paint the background color of the control.</summary>
        //[Category("Appearance"), Description("This feature will paint the background color of the control.")]
        //public System.Drawing.Color BorderColor { get { return V_BorderColor; } set { V_BorderColor = value; this.Refresh(); } }

        #endregion

        #region Constructor

        /// <summary>This method will construct a new GroupBox control.</summary>
        public TextBoxAdvanced()
        {
            InitializeStyles();
            InitializeTextBox();
        }


        #endregion

        #region DeConstructor

        /// <summary>This method will dispose of the GroupBox control.</summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }


        #endregion

        #region Initialization

        /// <summary>This method will initialize the controls custom styles.</summary>
        private void InitializeStyles()
        {
            //Set the control styles----------------------------------
            this.SetStyle(ControlStyles.DoubleBuffer, true);
          //  this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
          //  this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //--------------------------------------------------------
        }


        /// <summary>This method will initialize the GroupBox control.</summary>
        private void InitializeTextBox()
        {
            this.Resize += new EventHandler(TextBox_Resize);
            this.Name = "TextBoxAdvanced";
            this.Size = new System.Drawing.Size(100, 100);
        }


        #endregion

        #region Protected Methods

        /// <summary>Overrides the OnPaint method to paint control.</summary>
        /// <param name="e">The paint event arguments.</param>
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    PaintBack(e.Graphics);
        //}

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    this.Invalidate();
        //}

        //protected override void OnKeyUp(KeyEventArgs e)
        //{
        //    base.OnKeyUp(e);
        //    this.Invalidate();
        //}

        //protected override void OnKeyPress(KeyPressEventArgs e)
        //{
        //    base.OnKeyPress(e);
        //    this.Invalidate();
        //}

        //protected override void OnMouseUp(MouseEventArgs e)
        //{
        //    base.OnMouseUp(e);
        //    this.Invalidate();
        //}

        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    base.OnMouseDown(e);
        //    this.Invalidate();
        //}

        //protected override void OnMouseLeave(EventArgs e)
        //{
        //    base.OnMouseLeave(e);
        //    this.Invalidate();
        //}

        //protected override void OnTextChanged(EventArgs e)
        //{
        //    base.OnTextChanged(e);
        //    this.Invalidate();
        //}

        //protected override void OnMouseHover(EventArgs e)
        //{
        //    base.OnMouseHover(e);
        //    this.Invalidate();
        //}

        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    base.OnMouseMove(e);
        //    this.Invalidate();
        //}

        //protected override void OnMouseEnter(EventArgs e)
        //{
        //    base.OnMouseEnter(e);
        //    this.Invalidate();
        //}

        //protected override void OnLostFocus(EventArgs e)
        //{
        //    base.OnLostFocus(e);
        //    this.Invalidate();
        //}

        //protected override void OnGotFocus(EventArgs e)
        //{
        //    base.OnGotFocus(e);
        //    this.Invalidate();
        //}

        #endregion

        #region Private Methods

        /// <summary>This method will paint the control.</summary>
        /// <param name="g">The paint event graphics object.</param>
        //private void PaintBack(System.Drawing.Graphics g)
        //{
        //    //Set Graphics smoothing mode to Anit-Alias-- 
        //    g.SmoothingMode = SmoothingMode.AntiAlias;
        //    //-------------------------------------------

        //    //Declare Variables------------------
        //    int ArcX1 = 0;
        //    int ArcX2 = this.Width;
        //    int ArcY1 = 0;
        //    int ArcY2 = this.Height;
        //    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
        //    SolidBrush BackgroundBrush = new SolidBrush(BackColor);
        //    Pen BorderPen = new Pen(new SolidBrush(BorderColor));

        //    path.AddLine(ArcX1, ArcY1, ArcX2, ArcY1);
        //    path.AddLine(ArcX2, ArcY1, ArcX2, ArcY2);
        //    path.AddLine(ArcX2, ArcY2, ArcX1, ArcY2);
        //    path.AddLine(ArcX1, ArcY2, ArcX1, ArcY1);

        //    //Paint Borded-----------------------
        //    //g.DrawPath(BorderPen, path);
        //    //-----------------------------------

        //    g.FillRectangle(BackgroundBrush, ArcX1, ArcY1, ArcX2, ArcY2);

        //    SizeF StringSize = g.MeasureString(this.Text, this.Font);
        //    Size StringSize2 = StringSize.ToSize();

        //    int offsetX = (int)((this.Width * 0.5) - (StringSize2.Width * 0.5));
        //    int offsetY = (int)((this.Height * 0.5) - (StringSize2.Height * 0.5));

        //    if (this.SelectedText != null)
        //    {
        //        //g.FillRectangle(SystemBrushes.Highlight, 
        //        //this.Selec
        //    }
        //    g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), ArcX1 + offsetX, ArcY1 + offsetY);

        //    //Destroy Graphic Objects------------
        //    if (path != null) { path.Dispose(); }
        //    if (BackgroundBrush != null) { BackgroundBrush.Dispose(); }
        //    if (BorderPen != null) { BorderPen.Dispose(); }
        //    //-----------------------------------
        //}


        protected override void OnReadOnlyChanged(EventArgs e)
        {
            base.OnReadOnlyChanged(e);
            if (this.ReadOnly)
                this.Cursor = Cursors.Default;
            else
                this.Cursor = Cursors.IBeam;
        }

        protected override void  OnSelectionChanged(EventArgs e)
        {
 	         base.OnSelectionChanged(e);
             // Move the cursor to the end
             if (this.SelectionStart != this.TextLength)
                 this.SelectionStart = this.TextLength;
        }

        /// <summary>This method fires when the GroupBox resize event occurs.</summary>
        /// <param name="sender">The object the sent the event.</param>
        /// <param name="e">The event arguments.</param>
        private void TextBox_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }


        #endregion
    }
}
