using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using DevExpress.XtraEditors;


namespace WindowsFormsApplication1
{
    
 
        public class RoundedButton : SimpleButton, INotifyPropertyChanged
        {
            public object bindForClick;
            public string Title { get; set; }
            private bool _isClicked;
            public bool hasBoarder { get; set; } = false;
            public Boolean isClicked { get { return _isClicked; } set { _isClicked = value; OnPropertyChanged("isClicked"); } }
            public Boolean isHover { get; set; }
            public Boolean showBackground { get; set; } = false;
            public Color cBack { get; set; } = ThemeClass.fgPrimary;
            public Color cBackActive { get; set; } = ThemeClass.fgPrimary;
            public Color cBackHover { get; set; } = Color.LightBlue;
            public Color cBackHoverActive { get; set; } = Color.LightBlue;
            public Color cTitle { get; set; } = ThemeClass.acTertiary;
            public Color cTitleActive { get; set; } = ThemeClass.acTertiary;
            public Color cTitleHover { get; set; } = ThemeClass.acTertiary;
            public Color cTitleActiveHover { get; set; } = ThemeClass.acTertiary;
            public Image imageNormal { get; set; }
            public Image imageClicked { get; set; }
            public Image imageHover { get; set; }
            public int imageFitType { get; set; } = 0;
            public int cornerRadius { get; set; } = 20;
            public Point titleOffset { get; set; } = new Point(0, 0);

            public event PropertyChangedEventHandler PropertyChanged;
            public enum imageOptions
            {
                Fill,
                Fit,
                FitCenter,
                FitRight,
                Center,
                Raw
            }

            public RoundedButton()
            {

            }

            protected override void OnGotFocus(EventArgs e)
            {
                base.OnGotFocus(e);
                this.Invalidate();
            }
            protected override void OnLostFocus(EventArgs e)
            {
                base.OnLostFocus(e);
                this.Invalidate();
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                // First, we let the base do it's thing
                //base.OnPaint(e);
                Color aColor;                           //Color for background
                Color bColor;                           //Color for text
                Image bgImage = null;
                if (isClicked)
                {
                    if (isHover)
                    {
                        aColor = cBackHoverActive;
                        bColor = cTitleActiveHover;
                        if (imageHover != null) bgImage = imageHover;
                        else if (imageClicked != null) bgImage = imageClicked;
                        else if (imageNormal != null) bgImage = imageNormal;
                    }
                    else
                    {
                        aColor = cBackActive;
                        bColor = cTitleActive;
                        if (imageClicked != null) bgImage = imageClicked;
                        else if (imageNormal != null) bgImage = imageNormal;
                    }
                }
                else
                {
                    if (isHover)
                    {
                        aColor = cBackHover;
                        bColor = cTitleHover;
                        if (imageHover != null) bgImage = imageHover;
                        else if (imageNormal != null) bgImage = imageNormal;
                    }
                    else
                    {
                        aColor = cBack;
                        bColor = cTitle;
                        if (imageNormal != null) bgImage = imageNormal;
                    }
                }
                // Make sure we're using antialiasing
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                RectangleF destRect = new RectangleF(0F, 0F, this.Width, this.Height);
                if (bgImage != null)
                {
                    switch (imageFitType)
                    {
                        case (int)imageOptions.Fill:
                            break;
                        case (int)imageOptions.Fit:
                            {
                                SizeF newSize = scaledSize(bgImage);
                                destRect = new RectangleF(0F, 0F, newSize.Width, newSize.Height);
                            }
                            break;
                        case (int)imageOptions.FitCenter:
                            {
                                SizeF newSize = scaledSize(bgImage);
                                float offsetX = (Width - newSize.Width) / 2;
                                float offsetY = (Height - newSize.Height) / 2;
                                destRect = new RectangleF(offsetX, offsetY, newSize.Width, newSize.Height);
                            }
                            break;
                        case (int)imageOptions.FitRight:
                            {
                                SizeF newSize = scaledSize(bgImage);
                                float offsetX = Width - newSize.Width;
                                float offsetY = Height - newSize.Height;
                                destRect = new RectangleF(offsetX, offsetY, newSize.Width, newSize.Height);
                            }
                            break;
                        case (int)imageOptions.Center:
                            {
                                float offsetX = (Width - bgImage.Width) / 2;
                                float offsetY = (Height - bgImage.Height) / 2;
                                destRect = new RectangleF(offsetX, offsetY, bgImage.Width, bgImage.Height);
                                break;
                            }
                        case (int)imageOptions.Raw:
                            destRect = new RectangleF(0F, 0F, bgImage.Width, bgImage.Height);
                            break;
                    }
                }

                // Add an opaque rounded rectangle over the client area
                if (bgImage == null || showBackground)
                {
                    using (SolidBrush rectangleBrush = new SolidBrush(aColor))
                    {
                        //Oversize background by 1 pixel to keep from seeing edge effects
                        DrawRoundedRectangle(g, new Rectangle(ClientRectangle.X - 1, ClientRectangle.Y - 1, ClientRectangle.Width + 1, ClientRectangle.Height + 1), cornerRadius, rectangleBrush);
                    }
                }
                if (bgImage != null)
                {
                    RectangleF sourceRect = new RectangleF(0F, 0F, bgImage.Width, bgImage.Height);
                    g.DrawImage(bgImage, destRect, sourceRect, GraphicsUnit.Pixel);
                }

                // Finally, we add the title
                if (!string.IsNullOrEmpty(Title))
                {
                    using (SolidBrush textBrush = new SolidBrush(bColor))
                    {
                        SizeF titleSize = g.MeasureString(Title, Font);
                        g.DrawString(Title, Font, textBrush, (ClientRectangle.Width - titleSize.Width) / 2 + titleOffset.X, (ClientRectangle.Height - titleSize.Height) / 2 + titleOffset.Y);
                    }
                }

                //Add border if has focus
                if (Focused && hasBoarder)
                {
                    using (SolidBrush rectangleBrush = new SolidBrush(Color.LightGray))
                    {
                        Pen rectanglePen = new Pen(Color.Black, 1);
                        rectanglePen.DashStyle = DashStyle.Dot;
                        int offset = 5;
                        Rectangle boarder = new Rectangle(ClientRectangle.X + offset, ClientRectangle.Y + offset, ClientRectangle.Width - offset, ClientRectangle.Height - offset);
                        DrawUnfilledRoundedRectangle(g, boarder, cornerRadius - offset, rectanglePen);
                    }
                }

            }
            private SizeF scaledSize(Image bgImage)
            {
                float imageWidth = 0;
                float imageHeight = 0;
                float aspectRatio = bgImage.Width / bgImage.Height;
                if (Height * aspectRatio > Width)
                {
                    //Width controls size
                    imageWidth = Width;
                    imageHeight = Width / aspectRatio;
                }
                else
                {
                    //Height controls size
                    imageHeight = Height;
                    imageWidth = Height * aspectRatio;
                }
                return new SizeF(imageWidth, imageHeight);
            }

            static void DrawRoundedRectangle(Graphics g, Rectangle r, int d, Brush b)
            {
                if (d == 0)
                {
                    using (GraphicsPath gp = new GraphicsPath())
                    {
                        gp.AddRectangle(r);

                        // Draws filled rectangle
                        g.FillPath(b, gp);
                    }
                }
                else
                {
                    using (GraphicsPath gp = new GraphicsPath())
                    {
                        gp.AddArc(r.X, r.Y, d, d, 180, 90);
                        gp.AddArc(r.Width - d, r.Y, d, d, 270, 90);
                        gp.AddArc(r.Width - d, r.Height - d, d, d, 0, 90);
                        gp.AddArc(r.Y, r.Height - d, d, d, 90, 90);
                        gp.CloseFigure();

                        // Draws filled rectangle
                        g.FillPath(b, gp);
                    }
                }
            }
            static void DrawUnfilledRoundedRectangle(Graphics g, Rectangle r, int d, Pen p)
            {
                if (d == 0)
                {
                    using (GraphicsPath gp = new GraphicsPath())
                    {
                        gp.AddRectangle(r);

                        // Draws filled rectangle
                        g.DrawPath(p, gp);
                    }
                }
                else
                {
                    using (GraphicsPath gp = new GraphicsPath())
                    {
                        gp.AddArc(r.X, r.Y, d, d, 180, 90);
                        gp.AddArc(r.Width - d, r.Y, d, d, 270, 90);
                        gp.AddArc(r.Width - d, r.Height - d, d, d, 0, 90);
                        gp.AddArc(r.Y, r.Height - d, d, d, 90, 90);
                        gp.CloseFigure();

                        // Draws unfilled rectangle
                        g.DrawPath(p, gp);
                    }
                }
            }
            protected override void OnClick(EventArgs e)
            {
                base.OnClick(e);
                isClicked = !isClicked;
            }
            protected override void OnMouseEnter(EventArgs e)
            {
                base.OnMouseEnter(e);
                isHover = true;
            }
            protected override void OnMouseLeave(EventArgs e)
            {
                base.OnMouseLeave(e);
                isHover = false;
            }
            protected void OnPropertyChanged(string name)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }
    }


