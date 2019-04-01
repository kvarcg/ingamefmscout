/*
 [PLEASE DO NOT MODIFY THIS HEADER INFORMATION]---------------------
 Title: Grouper
 Description: A rounded groupbox with special painting features. 
 Date Created: December 17, 2005
 Author: Adam Smith
 Author Email: ibulwark@hotmail.com
 Websites: http://www.ebadgeman.com | http://www.codevendor.com
 
 Version History:
 1.0a - Beta Version - Release Date: December 17, 2005 
 -------------------------------------------------------------------
 */

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
	public class GroupBoxAdvanced : System.Windows.Forms.UserControl
	{
        #region Constants

        /// <summary>The sweep angle of the arc.</summary>
        public const int SweepAngle = 90;

        /// <summary>The minimum control height.</summary>
        public const int MinControlHeight = 32;

        /// <summary>The minimum control width.</summary>
        public const int MinControlWidth = 96;

        #endregion

		#region Enumerations

		/// <summary>A special gradient enumeration.</summary>
        public enum GroupBoxGradientMode
        {
            /// <summary>Specifies no gradient mode.</summary>
            None = 4,

            /// <summary>Specifies a gradient from upper right to lower left.</summary>
            BackwardDiagonal = 3,

            /// <summary>Specifies a gradient from upper left to lower right.</summary>
            ForwardDiagonal = 2,

            /// <summary>Specifies a gradient from left to right.</summary>
            Horizontal = 0,

            /// <summary>Specifies a gradient from top to bottom.</summary>
            Vertical = 1
        }

        public enum GroupTitleRenderType
        {
            Standard = 0,
            Fill = 1
        }

        public enum GroupBoxBorderType
        {
            Full = 0,
            HalfTop = 1,
            None = 2
        }

        public enum GroupBoxDrawType
        {
            SingleLine = 0,
            TwoLine = 1,
            TwoLineReversed = 2
        }
		
		#endregion

		#region Variables

		private System.ComponentModel.Container components = null;
		private int V_RoundCorners = 2;
		private string V_GroupTitle = "";
		private System.Drawing.Color V_BorderColor = Color.Black;
		private float V_BorderThickness = 1;
		private bool V_ShadowControl = false;
		private System.Drawing.Color V_BackgroundColor = Color.White;
        private System.Drawing.Color V_BackgroundColor2 = SystemColors.ControlLight;
        private System.Drawing.Color V_BackgroundGradientColor = Color.White;
		private GroupBoxGradientMode V_BackgroundGradientMode = GroupBoxGradientMode.None;
		private System.Drawing.Color V_ShadowColor = Color.DarkGray;
		private int V_ShadowThickness = 3;
		private System.Drawing.Image V_GroupImage = null;
		private System.Drawing.Color V_CustomGroupBoxColor = Color.White;
		private bool V_PaintGroupBox = false;
        private GroupBoxBorderType V_PaintGroupTitleBorderBox = GroupBoxBorderType.Full;
        private Point V_GroupTitleOffset = new Point(-18, -2);
        private GroupTitleRenderType V_GroupTitleRenderType = GroupTitleRenderType.Standard;
        private GroupBoxDrawType V_GroupBoxDrawType = GroupBoxDrawType.SingleLine;
		private System.Drawing.Color V_BackColor = Color.Transparent;
        private int V_TwoLineDistance = 20;
        private int V_TwoLineStart = 18;

		#endregion

		#region Properties

		/// <summary>This feature will paint the background color of the control.</summary>
		[Category("Appearance"), Description("This feature will paint the background color of the control.")]
		public override System.Drawing.Color BackColor{get{return V_BackColor;} set{V_BackColor=value; this.Refresh();}}

		/// <summary>This feature will paint the group title background to the specified color if PaintGroupBox is set to true.</summary>
		[Category("Appearance"), Description("This feature will paint the group title background to the specified color if PaintGroupBox is set to true.")]
		public System.Drawing.Color CustomGroupBoxColor{get{return V_CustomGroupBoxColor;} set{V_CustomGroupBoxColor=value; this.Refresh();}}

		/// <summary>This feature will paint the group title background to the CustomGroupBoxColor.</summary>
		[Category("Appearance"), Description("This feature will paint the group title background to the CustomGroupBoxColor.")]
		public bool PaintGroupBox{get{return V_PaintGroupBox;} set{V_PaintGroupBox=value; this.Refresh();}}

        /// <summary>This feature will paint the group title border to the CustomGroupBoxColor.</summary>
        [Category("Appearance"), Description("This feature will paint the group title border box.")]
        public GroupBoxBorderType GroupBoxBorder { get { return V_PaintGroupTitleBorderBox; } set { V_PaintGroupTitleBorderBox = value; this.Refresh(); } }

        /// <summary>This feature will paint the group title background to the CustomGroupBoxColor.</summary>
        [Category("Appearance"), Description("This feature will offset the group title box.")]
        public Point GroupTitleOffset { get { return V_GroupTitleOffset; } set { V_GroupTitleOffset = value; this.Refresh(); } }

        /// <summary>This feature will paint the background color of the control.</summary>
        [Category("Appearance"), Description("This feature will change the box type.")]
        public GroupTitleRenderType GroupTitleType { get { return V_GroupTitleRenderType; } set { V_GroupTitleRenderType = value; this.Refresh(); } }

        /// <summary>This feature will paint the inner control.</summary>
        [Category("Appearance"), Description("This feature will paint the inner control.")]
        public GroupBoxDrawType GroupBoxDrawStyle { get { return V_GroupBoxDrawType; } set { V_GroupBoxDrawType = value; this.Refresh(); } }

        /// <summary>This feature will set the distance between two line rendering.</summary>
        [Category("Appearance"), Description("This feature will set the distance between two line rendering.")]
        public int TwoLineDistance { 
            get 
            { 
                return V_TwoLineDistance; 
            } 
            set 
            { 
                V_TwoLineDistance = value;
                if (V_TwoLineDistance < 1) V_TwoLineDistance = 1;
                this.Refresh(); 
            } }

        /// <summary>This feature will set the start offset between two line rendering.</summary>
        [Category("Appearance"), Description("This feature will set the start offset between two line rendering.")]
        public int TwoLineStartOffset { get { return V_TwoLineStart; } set { V_TwoLineStart = value; this.Refresh(); } }

		/// <summary>This feature can add a 16 x 16 image to the group title bar.</summary>
		[Category("Appearance"), Description("This feature can add a 16 x 16 image to the group title bar.")]
		public System.Drawing.Image GroupImage{get{return V_GroupImage;} set{V_GroupImage=value; this.Refresh();}}

		/// <summary>This feature will change the control's shadow color.</summary>
		[Category("Appearance"), Description("This feature will change the control's shadow color.")]
		public System.Drawing.Color ShadowColor{get{return V_ShadowColor;} set{V_ShadowColor=value; this.Refresh();}}

		/// <summary>This feature will change the size of the shadow border.</summary>
		[Category("Appearance"), Description("This feature will change the size of the shadow border.")]
		public int ShadowThickness
		{
			get{return V_ShadowThickness;} 
			set
			{
				if(value>10)
				{
					V_ShadowThickness=10;
				}
				else
				{
					if(value<1){V_ShadowThickness=1;}
					else{V_ShadowThickness=value; }
				}

				this.Refresh();
			}
		}

		
		/// <summary>This feature will change the group control color. This color can also be used in combination with BackgroundGradientColor for a gradient paint.</summary>
		[Category("Appearance"), Description("This feature will change the group control color. This color can also be used in combination with BackgroundGradientColor for a gradient paint.")]
		public System.Drawing.Color BackgroundColor{get{return V_BackgroundColor;} set{V_BackgroundColor=value; this.Refresh();}}

        /// <summary>This feature will change the second group control color. This color can also be used in combination with BackgroundGradientColor for a gradient paint.</summary>
        [Category("Appearance"), Description("This feature will change the group control color. This color can also be used in combination with BackgroundGradientColor for a gradient paint.")]
        public System.Drawing.Color BackgroundColor2 { get { return V_BackgroundColor2; } set { V_BackgroundColor2 = value; this.Refresh(); } }

		/// <summary>This feature can be used in combination with BackgroundColor to create a gradient background.</summary>
		[Category("Appearance"), Description("This feature can be used in combination with BackgroundColor to create a gradient background.")]
		public System.Drawing.Color BackgroundGradientColor{get{return V_BackgroundGradientColor;} set{V_BackgroundGradientColor=value; this.Refresh();}}
		
		/// <summary>This feature turns on background gradient painting.</summary>
		[Category("Appearance"), Description("This feature turns on background gradient painting.")]
		public GroupBoxGradientMode BackgroundGradientMode{get{return V_BackgroundGradientMode;} set{V_BackgroundGradientMode=value; this.Refresh();}}

		/// <summary>This feature will round the corners of the control.</summary>
		[Category("Appearance"), Description("This feature will round the corners of the control.")]
		public int RoundCorners
		{
			get{return V_RoundCorners;} 
			set
			{
				if(value>25)
				{
					V_RoundCorners=25;
				}
				else
				{
					if(value<1){V_RoundCorners=1;}
					else{V_RoundCorners=value; }
				}
				
				this.Refresh();
			}
		}

		/// <summary>This feature will add a group title to the control.</summary>
		[Category("Appearance"), Description("This feature will add a group title to the control.")]
		public string GroupTitle{get{return V_GroupTitle;} set{V_GroupTitle=value; this.Refresh();}}

		/// <summary>This feature will allow you to change the color of the control's border.</summary>
		[Category("Appearance"), Description("This feature will allow you to change the color of the control's border.")]
		public System.Drawing.Color BorderColor{get{return V_BorderColor;} set{V_BorderColor=value; this.Refresh();}}

		/// <summary>This feature will allow you to set the control's border size.</summary>
		[Category("Appearance"), Description("This feature will allow you to set the control's border size.")]
		public float BorderThickness
		{
			get{return V_BorderThickness;} 
			set
			{
				if(value>3)
				{
					V_BorderThickness=3;
				}
				else
				{
					if(value<1){V_BorderThickness=1;}
					else{V_BorderThickness=value;}
				}
				this.Refresh();
			}
		}

		/// <summary>This feature will allow you to turn on control shadowing.</summary>
		[Category("Appearance"), Description("This feature will allow you to turn on control shadowing.")]
		public bool ShadowControl{get{return V_ShadowControl;} set{V_ShadowControl=value; this.Refresh();}}

		#endregion

		#region Constructor
		
		/// <summary>This method will construct a new GroupBox control.</summary>
        public GroupBoxAdvanced()
		{
			InitializeStyles();
			InitializeGroupBox();
		}

	
		#endregion

		#region DeConstructor

		/// <summary>This method will dispose of the GroupBox control.</summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing){if(components!=null){components.Dispose();}}
			base.Dispose(disposing);
		}


		#endregion

		#region Initialization

		/// <summary>This method will initialize the controls custom styles.</summary>
		private void InitializeStyles()
		{
			//Set the control styles----------------------------------
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			//--------------------------------------------------------
		}


		/// <summary>This method will initialize the GroupBox control.</summary>
		private void InitializeGroupBox()
		{
			components = new System.ComponentModel.Container();
			this.Resize+=new EventHandler(GroupBox_Resize);
			this.DockPadding.All = 20;
			this.Name = "GroupBox";
			this.Size = new System.Drawing.Size(100, 100);
		}

	
		#endregion

		#region Protected Methods
		
		/// <summary>Overrides the OnPaint method to paint control.</summary>
		/// <param name="e">The paint event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			PaintBack(e.Graphics);
			PaintGroupText(e.Graphics);
		}

		#endregion

		#region Private Methods

		/// <summary>This method will paint the group title.</summary>
		/// <param name="g">The paint event graphics object.</param>
		private void PaintGroupText(System.Drawing.Graphics g)
		{
			//Check if string has something-------------
			if(this.GroupTitle==string.Empty){return;}
			//------------------------------------------

			//Set Graphics smoothing mode to Anit-Alias-- 
			g.SmoothingMode = SmoothingMode.AntiAlias;
			//-------------------------------------------

			//Declare Variables------------------
			SizeF StringSize = g.MeasureString(this.GroupTitle, this.Font);
			Size StringSize2 = StringSize.ToSize();
			if(this.GroupImage!=null){StringSize2.Width+=18;}
			int ArcWidth = this.RoundCorners;
			int ArcHeight = this.RoundCorners;
            int ArcX1 = 28 + GroupTitleOffset.X - 4;
            int ArcX2 = (StringSize2.Width + 34) - (ArcWidth + 1) + GroupTitleOffset.X;
            int ArcY1 = GroupTitleOffset.Y + 3;
            int ArcY2 = 24 - (ArcHeight + 1) + GroupTitleOffset.Y - 2;
            if (this.GroupTitleType == GroupTitleRenderType.Fill)
            {
                ArcX1 = 0 + GroupTitleOffset.X;// 20;
                ArcX2 = this.Width - 3 + GroupTitleOffset.X;// (StringSize2.Width + 34) - (ArcWidth + 1);
                ArcY1 = GroupTitleOffset.Y; // 0
                ArcY2 = 24 - (ArcHeight + 7) + GroupTitleOffset.Y; // + 1
            }
			System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
			System.Drawing.Brush BorderBrush = new SolidBrush(this.BorderColor);
			System.Drawing.Pen BorderPen = new Pen(BorderBrush, this.BorderThickness);
			System.Drawing.Drawing2D.LinearGradientBrush BackgroundGradientBrush = null;
			System.Drawing.Brush BackgroundBrush = (this.PaintGroupBox) ? new SolidBrush(this.CustomGroupBoxColor) : new SolidBrush(this.BackgroundColor);
			System.Drawing.SolidBrush TextColorBrush = new SolidBrush(this.ForeColor);
			System.Drawing.SolidBrush ShadowBrush = null;
			System.Drawing.Drawing2D.GraphicsPath ShadowPath  = null;
			//-----------------------------------

			//Check if shadow is needed----------
			if(this.ShadowControl)
			{
				ShadowBrush = new SolidBrush(this.ShadowColor);
				ShadowPath = new System.Drawing.Drawing2D.GraphicsPath();
				ShadowPath.AddArc(ArcX1+(this.ShadowThickness-1), ArcY1+(this.ShadowThickness-1), ArcWidth, ArcHeight, 180, SweepAngle); // Top Left
				ShadowPath.AddArc(ArcX2+(this.ShadowThickness-1), ArcY1+(this.ShadowThickness-1), ArcWidth, ArcHeight, 270, SweepAngle); //Top Right
				ShadowPath.AddArc(ArcX2+(this.ShadowThickness-1), ArcY2+(this.ShadowThickness-1), ArcWidth, ArcHeight, 360, SweepAngle); //Bottom Right
				ShadowPath.AddArc(ArcX1+(this.ShadowThickness-1), ArcY2+(this.ShadowThickness-1), ArcWidth, ArcHeight, 90, SweepAngle); //Bottom Left
				ShadowPath.CloseAllFigures();

				//Paint Rounded Rectangle------------
				g.FillPath(ShadowBrush, ShadowPath);
				//-----------------------------------
			}
			//-----------------------------------

			//Create Rounded Rectangle Path------
			path.AddArc(ArcX1, ArcY1, ArcWidth, ArcHeight, 180, SweepAngle); // Top Left
			path.AddArc(ArcX2, ArcY1, ArcWidth, ArcHeight, 270, SweepAngle); //Top Right
			path.AddArc(ArcX2, ArcY2, ArcWidth, ArcHeight, 360, SweepAngle); //Bottom Right
			path.AddArc(ArcX1, ArcY2, ArcWidth, ArcHeight, 90, SweepAngle); //Bottom Left
			path.CloseAllFigures(); 
			//-----------------------------------

			//Check if Gradient Mode is enabled--
			if(this.PaintGroupBox)
			{
				//Paint Rounded Rectangle------------
				g.FillPath(BackgroundBrush, path);
				//-----------------------------------
			}
			else
			{
				if(this.BackgroundGradientMode==GroupBoxGradientMode.None)
				{
					//Paint Rounded Rectangle------------
					g.FillPath(BackgroundBrush, path);
					//-----------------------------------
				}
				else
				{
					BackgroundGradientBrush = new LinearGradientBrush(new Rectangle(0,0,this.Width,this.Height), this.BackgroundColor, this.BackgroundGradientColor, (LinearGradientMode)this.BackgroundGradientMode);
				
					//Paint Rounded Rectangle------------
					g.FillPath(BackgroundGradientBrush, path);
					//-----------------------------------
				}
			}
			//-----------------------------------

			//Paint Borded-----------------------
            if (this.GroupBoxBorder == GroupBoxBorderType.Full)
                g.DrawPath(BorderPen, path);
            else if (this.GroupBoxBorder == GroupBoxBorderType.HalfTop)
            {
                System.Drawing.Drawing2D.GraphicsPath halfPath2 = new System.Drawing.Drawing2D.GraphicsPath();
                halfPath2.AddArc(ArcX1, ArcY1, ArcWidth, ArcHeight, 180, SweepAngle); // Top Left
                halfPath2.AddArc(ArcX2, ArcY1, ArcWidth, ArcHeight, 270, SweepAngle); //Top Right
                halfPath2.AddArc(ArcX2, ArcY1 + (ArcY2 - ArcY1) * 0.5f, ArcWidth, ArcHeight, 360, SweepAngle); //Bottom Right
                halfPath2.AddArc(ArcX1, ArcY1 + (ArcY2 - ArcY1) * 0.5f, ArcWidth, ArcHeight, 90, SweepAngle); //Bottom Left
                halfPath2.CloseAllFigures();

                g.DrawPath(BorderPen, halfPath2);
                g.FillRectangle(BackgroundBrush, ArcX1 - 1, ArcY1 + (ArcY2 - ArcY1) * 0.5f + 1, ArcX2, this.BorderThickness + 2);
            }
			//-----------------------------------

			//Paint Text-------------------------
			int CustomStringWidth = (this.GroupImage!=null) ? 46 : 28;
            if (this.GroupTitleType == GroupTitleRenderType.Fill)
                g.DrawString(this.GroupTitle, this.Font, TextColorBrush, (int)((this.Width * 0.5) - (StringSize2.Width * 0.5)) + GroupTitleOffset.X, 1 + GroupTitleOffset.Y); // 5
            else if (this.GroupTitleType == GroupTitleRenderType.Standard) 
                g.DrawString(this.GroupTitle, this.Font, TextColorBrush, CustomStringWidth + GroupTitleOffset.X, 5 + GroupTitleOffset.Y - 1);
			//-----------------------------------

			//Draw GroupImage if there is one----
			if(this.GroupImage!=null)
			{
                g.DrawImage(this.GroupImage, 30 + GroupTitleOffset.X, 4 + GroupTitleOffset.Y, 16, 16);
			}
			//-----------------------------------

			//Destroy Graphic Objects------------
			if(path!=null){path.Dispose();}
			if(BorderBrush!=null){BorderBrush.Dispose();}
			if(BorderPen!=null){BorderPen.Dispose();}
			if(BackgroundGradientBrush!=null){BackgroundGradientBrush.Dispose();}
			if(BackgroundBrush!=null){BackgroundBrush.Dispose();}
			if(TextColorBrush!=null){TextColorBrush .Dispose();}
			if(ShadowBrush!=null){ShadowBrush.Dispose();}
			if(ShadowPath!=null){ShadowPath.Dispose();}
			//-----------------------------------
		}

		
		/// <summary>This method will paint the control.</summary>
		/// <param name="g">The paint event graphics object.</param>
		private void PaintBack(System.Drawing.Graphics g)
		{
			//Set Graphics smoothing mode to Anit-Alias-- 
			g.SmoothingMode = SmoothingMode.AntiAlias;
			//-------------------------------------------

			//Declare Variables------------------
			int ArcWidth = this.RoundCorners * 2;
			int ArcHeight = this.RoundCorners * 2;
			int ArcX1 = 0;
			int ArcX2 = (this.ShadowControl) ? (this.Width - (ArcWidth + 1))-this.ShadowThickness : this.Width - (ArcWidth + 1);
			int ArcY1 = 10;
            //Check if string has something-------------
            if (this.GroupTitle == string.Empty) ArcY1 = 0;
            //------------------------------------------
			int ArcY2 = (this.ShadowControl) ? (this.Height - (ArcHeight + 1))-this.ShadowThickness : this.Height - (ArcHeight + 1);
			System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
			System.Drawing.Brush BorderBrush = new SolidBrush(this.BorderColor);
			System.Drawing.Pen BorderPen = new Pen(BorderBrush, this.BorderThickness);
			System.Drawing.Drawing2D.LinearGradientBrush BackgroundGradientBrush = null;
			System.Drawing.Brush BackgroundBrush = new SolidBrush(this.BackgroundColor);
			System.Drawing.SolidBrush ShadowBrush = null;
			System.Drawing.Drawing2D.GraphicsPath ShadowPath  = null;
			//-----------------------------------

			//Check if shadow is needed----------
			if(this.ShadowControl)
			{
				ShadowBrush = new SolidBrush(this.ShadowColor);
				ShadowPath = new System.Drawing.Drawing2D.GraphicsPath();
				ShadowPath.AddArc(ArcX1+this.ShadowThickness, ArcY1+this.ShadowThickness, ArcWidth, ArcHeight, 180, SweepAngle); // Top Left
				ShadowPath.AddArc(ArcX2+this.ShadowThickness, ArcY1+this.ShadowThickness, ArcWidth, ArcHeight, 270, SweepAngle); //Top Right
				ShadowPath.AddArc(ArcX2+this.ShadowThickness, ArcY2+this.ShadowThickness, ArcWidth, ArcHeight, 360, SweepAngle); //Bottom Right
				ShadowPath.AddArc(ArcX1+this.ShadowThickness, ArcY2+this.ShadowThickness, ArcWidth, ArcHeight, 90, SweepAngle); //Bottom Left
				ShadowPath.CloseAllFigures();

				//Paint Rounded Rectangle------------
				g.FillPath(ShadowBrush, ShadowPath);
				//-----------------------------------
			}
			//-----------------------------------

			//Create Rounded Rectangle Path------
			path.AddArc(ArcX1, ArcY1, ArcWidth, ArcHeight, 180, SweepAngle); // Top Left
			path.AddArc(ArcX2, ArcY1, ArcWidth, ArcHeight, 270, SweepAngle); //Top Right
			path.AddArc(ArcX2, ArcY2, ArcWidth, ArcHeight, 360, SweepAngle); //Bottom Right
			path.AddArc(ArcX1, ArcY2, ArcWidth, ArcHeight, 90, SweepAngle); //Bottom Left
			path.CloseAllFigures(); 
			//-----------------------------------

			//Check if Gradient Mode is enabled--
			if(this.BackgroundGradientMode==GroupBoxGradientMode.None)
			{
				//Paint Rounded Rectangle------------

                if (this.GroupBoxDrawStyle == GroupBoxDrawType.SingleLine)
                    g.FillPath(BackgroundBrush, path);
                else
                {
                    Brush brush1 = null;
                    Brush brush2 = null;
                    if (this.GroupBoxDrawStyle == GroupBoxDrawType.TwoLine)
                    {
                        brush1 = BackgroundBrush;
                        brush2 = new SolidBrush(BackgroundColor2);
                    }
                    else if (this.GroupBoxDrawStyle == GroupBoxDrawType.TwoLineReversed)
                    {
                        brush1 = new SolidBrush(BackgroundColor2);
                        brush2 = BackgroundBrush;
                    }
                     
                    int dis = ArcY2 - ArcY1;
                        int num = dis / TwoLineDistance;
                        int curY = TwoLineStartOffset;
                        GraphicsPath twoLinePath = new GraphicsPath();

                        for (int i = 0; i < num; ++i)
                        {
                            if (i % 2 == 0)
                                g.FillRectangle(brush1, ArcX1 + 1, curY, ArcX2 + 2, TwoLineDistance);
                            else if (i % 2 == 1)
                                g.FillRectangle(brush2, ArcX1 + 1, curY, ArcX2 + 2, TwoLineDistance);

                            curY += TwoLineDistance;
                        }
                }

				//-----------------------------------
			}
			else
			{
				BackgroundGradientBrush = new LinearGradientBrush(new Rectangle(0,0,this.Width,this.Height), this.BackgroundColor, this.BackgroundGradientColor, (LinearGradientMode)this.BackgroundGradientMode);
				
				//Paint Rounded Rectangle------------
				g.FillPath(BackgroundGradientBrush, path);
				//-----------------------------------
			}
			//-----------------------------------

			//Paint Borded-----------------------
			g.DrawPath(BorderPen, path);
			//-----------------------------------

			//Destroy Graphic Objects------------
			if(path!=null){path.Dispose();}
			if(BorderBrush!=null){BorderBrush.Dispose();}
			if(BorderPen!=null){BorderPen.Dispose();}
			if(BackgroundGradientBrush!=null){BackgroundGradientBrush.Dispose();}
			if(BackgroundBrush!=null){BackgroundBrush.Dispose();}
			if(ShadowBrush!=null){ShadowBrush.Dispose();}
			if(ShadowPath!=null){ShadowPath.Dispose();}
			//-----------------------------------
		}

	
		/// <summary>This method fires when the GroupBox resize event occurs.</summary>
		/// <param name="sender">The object the sent the event.</param>
		/// <param name="e">The event arguments.</param>
		private void GroupBox_Resize(object sender, EventArgs e)
		{
			this.Refresh();
		}


		#endregion
	}
}
