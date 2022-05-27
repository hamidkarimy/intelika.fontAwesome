using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Text;

namespace intelika.fontAwesome
{
    public class DuotoneIcon
    {
        public class Properties
        {
            /// <summary>
            /// Image square size in pixels
            /// </summary>
            public int? Size { get; set; }
            /// <summary>
            /// Position of image
            /// </summary>
            public Point? Location { get; set; }
            /// <summary>
            /// Color of image
            /// </summary>public Color ForeColor { get; set; }
            public Color? ForeColor { get; set; }
            /// <summary>
            /// Background color of image
            /// </summary>
            public Color? BackColor { get; set; }
            /// <summary>
            /// Border of image color
            /// </summary>
            public Color? BorderColor { get; set; }
            /// <summary>
            /// Visible Border or not
            /// </summary>
            public bool? ShowBorder { get; set; }
            /// <summary>
            /// Image/icon type
            /// </summary>
            public NormalIconType Type { get; set; }

            private Properties()
            {
                Size = 32;
                Location = new Point(0, 0);
                ForeColor = Color.Black;
                BackColor = Color.Transparent;
                BorderColor = Color.Gray;
                ShowBorder = false;
            }

            public Properties(Properties iconProperty)
            {
                Size = iconProperty.Size == null ? Default.Size : iconProperty.Size;
                Location = iconProperty.Location == null ? Default.Location : iconProperty.Location;
                ForeColor = iconProperty.ForeColor == null ? Default.ForeColor : iconProperty.ForeColor;
                BackColor = iconProperty.BackColor == null ? Default.BackColor : iconProperty.BackColor;
                BorderColor = iconProperty.BorderColor == null ? Default.BorderColor : iconProperty.BorderColor;
                ShowBorder = iconProperty.ShowBorder == null ? Default.ShowBorder : iconProperty.ShowBorder;
                Type = iconProperty.Type;
            }
            public Properties(NormalIconType type, Color? color, int size)
            {
                Size = size;
                Location = new Point(0, 0);
                ForeColor = color == null ? Default.ForeColor : (Color)color;
                BackColor = Color.Transparent;
                BorderColor = Color.Gray;
                ShowBorder = false;
                Type = type;
            }

            private static Properties _default;
            public static Properties Default
            {
                get
                {
                    if (_default == null)
                    {
                        _default = new Properties();
                    }
                    return _default;
                }
                internal set
                {
                    _default = value;
                }
            }

        }
        private PrivateFontCollection _fonts = new PrivateFontCollection();
        private const string fontLightName = "fa-light-300.ttf";
        private const string fontRegularName = "fa-regular-400.ttf";
        private const string fontThinName = "fa-thin-100.ttf";
        private const string fontSolidName = "fa-solid-900.ttf";

        internal DuotoneIcon()
        {
            LoadFont();
        }
        /// <summary>
        /// Download (if neccessary) and load the font file.
        /// </summary>
        private void LoadFont()
        {
            _fonts.AddFontFile(fontRegularName);
        }
        public static DuotoneIcon _instance;

        public static DuotoneIcon Instance
        {
            get
            {
                if (_instance == null)
                {
                    Initialize();
                }
                return _instance;
            }
        }
        public static void Initialize()
        {
            //load font to memory         
            _instance = new DuotoneIcon();
        }
        /// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="props">The props.</param>
		/// <returns></returns>
		public Bitmap GetImage(Properties iconProperty)
        {
            var props = new Properties(iconProperty);
            return GetImageInternal(props);
        }
        public Bitmap GetImage(NormalIconType type, Color? color, int size = 32)
        {
            Color iconColor = color == null ? Color.Black : (Color)color;
            var props = new Properties(type, color, size);
            return GetImageInternal(props);
        }
        private Font GetIconFont(int pixelSize)
        {
            var size = pixelSize / (16f / 12f); //pixel to point conversion rate
                                                //maybe caching would be useful
            var font = new Font(_fonts.Families[0], size, FontStyle.Regular, GraphicsUnit.Point);
            return font;
        }
        private Size GetFontIconRealSize(int size, int iconIndex)
        {
            var bmpTemp = new Bitmap(size, size);
            using (Graphics g1 = Graphics.FromImage(bmpTemp))
            {
                g1.TextRenderingHint = TextRenderingHint.AntiAlias;
                g1.PixelOffsetMode = PixelOffsetMode.HighQuality;
                var font = GetIconFont(size);
                if (font != null)
                {
                    string character = char.ConvertFromUtf32(iconIndex);
                    var format = new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.Word
                    };

                    var sizeF = g1.MeasureString(character, font, new Point(0, 0), format);
                    return sizeF.ToSize();
                }
            }
            return new Size(size, size);
        }
        private Bitmap ResizeImage(Bitmap imgToResize, DuotoneIcon.Properties props)
        {
            var srcWidth = imgToResize.Width;
            var srcHeight = imgToResize.Height;

            float ratio = (srcWidth > srcHeight) ? (srcWidth / (float)srcHeight) : (srcHeight / (float)srcWidth);

            var dstWidth = (int)Math.Ceiling(srcWidth / ratio);
            var dstHeight = (int)Math.Ceiling(srcHeight / ratio);

            var x = (int)Math.Round(((int)props.Size - dstWidth) / 2f, 0);
            var y = (int)(1 + Math.Round(((int)props.Size - dstHeight) / 2f, 0));

            Bitmap b = new Bitmap((int)props.Size + ((Point)props.Location).X, (int)props.Size + ((Point)props.Location).Y);
            using (Graphics g = Graphics.FromImage((Image)b))
            {
                g.Clear((Color)props.BackColor);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(imgToResize, x + ((Point)props.Location).X, y + ((Point)props.Location).Y, dstWidth, dstHeight);
            }
            return b;
        }
        private Bitmap GetImageInternal(Properties props)
        {
            var size = GetFontIconRealSize((int)props.Size, (int)props.Type);
            var bmpTemp = new Bitmap(size.Width, size.Height);
            using (Graphics g1 = Graphics.FromImage(bmpTemp))
            {
                g1.TextRenderingHint = TextRenderingHint.AntiAlias;
                //g1.Clear(Color.Transparent);
                var font = GetIconFont((int)props.Size);
                if (font != null)
                {
                    string character = char.ConvertFromUtf32((int)props.Type);
                    var format = new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.Character
                    };

                    g1.DrawString(character, font, new SolidBrush((Color)props.ForeColor), 0, 0);
                    g1.DrawImage(bmpTemp, 0, 0);
                }
            }

            var bmp = ResizeImage(bmpTemp, props);
            if ((bool)props.ShowBorder)
            {
                using (Graphics g2 = Graphics.FromImage(bmp))
                {
                    var pen = new Pen((Color)props.BorderColor, 1);
                    var borderRect = new Rectangle(0, 0, (int)(props.Size - pen.Width), (int)(props.Size - pen.Width));
                    g2.DrawRectangle(pen, borderRect);
                }
            }
            return bmp;
        }
    }
}
