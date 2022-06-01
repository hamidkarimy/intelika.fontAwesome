using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace intelika.fontAwesome
{
    public class OneColorIcons
    {
        public static iconStyle IconStyle { get; set; }
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
        public enum iconStyle
        {
            regular,
            light,
            thin,
            solid,
            duotone
        }
        internal OneColorIcons(iconStyle style)
        {
            LoadFont(style);
        }
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        public  void AddMemoryFont(byte[] fontResource)
        {
            IntPtr p;
            uint c = 0;

            p = Marshal.AllocCoTaskMem(fontResource.Length);
            Marshal.Copy(fontResource, 0, p, fontResource.Length);
            AddFontMemResourceEx(p, (uint)fontResource.Length, IntPtr.Zero, ref c);
            _fonts.AddMemoryFont(p, fontResource.Length);
            Marshal.FreeCoTaskMem(p);

            p = IntPtr.Zero;
        }
        /// <summary>
        /// Download (if neccessary) and load the font file.
        /// </summary>
        private void LoadFont(iconStyle style)
        {
            switch (style)
            {
                case iconStyle.regular:
                    AddMemoryFont(fontAwesome.Properties.Resources.fa_regular_400);
                    break;
                case iconStyle.light:
                    AddMemoryFont(fontAwesome.Properties.Resources.fa_light_300);
                    break;
                case iconStyle.thin:
                    AddMemoryFont(fontAwesome.Properties.Resources.fa_thin_100);
                    break;
                case iconStyle.solid:
                    AddMemoryFont(fontAwesome.Properties.Resources.fa_solid_900);
                    break;
                case iconStyle.duotone:
                    AddMemoryFont(fontAwesome.Properties.Resources.fa_duotone_900);
                    break;
                default:
                    break;
            }
        }
        public static OneColorIcons _regularInstance;
        public static OneColorIcons _lightInstance;
        public static OneColorIcons _thinInstance;
        public static OneColorIcons _solidInstance;
        public static OneColorIcons _duotoneInstance;
        public static OneColorIcons RegularInstance
        {
            get
            {
                if (_regularInstance == null)
                {
                    Initialize(iconStyle.regular);
                }
                return _regularInstance;
            }
        }
        public static OneColorIcons LightInstance
        {
            get
            {
                if (_lightInstance == null)
                {
                    Initialize(iconStyle.light);
                }
                return _lightInstance;
            }
        }
        public static OneColorIcons ThinInstance
        {
            get
            {
                if (_thinInstance == null)
                {
                    Initialize(iconStyle.thin);
                }
                return _thinInstance;
            }
        }
        public static OneColorIcons SolidInstance
        {
            get
            {
                if (_solidInstance == null)
                {
                    Initialize(iconStyle.solid);
                }
                return _solidInstance;
            }
        }
        public static OneColorIcons DuotoneInstancee
        {
            get
            {
                if (_duotoneInstance == null)
                {
                    Initialize(iconStyle.duotone);
                }
                return _duotoneInstance;
            }
        }
        public static void Initialize(iconStyle style)
        {
            //load font to memory
            switch (style)
            {
                case iconStyle.regular:
                    if (_regularInstance == null)
                    {
                        _regularInstance = new OneColorIcons(style);
                    }
                    IconStyle=style;
                    break;
                case iconStyle.light:
                    if (_lightInstance == null)
                    {
                        _lightInstance = new OneColorIcons(style);
                    }
                    IconStyle=style;
                    break;
                case iconStyle.thin:
                    if (_thinInstance == null)
                    {
                        _thinInstance = new OneColorIcons(style);
                    }
                    IconStyle=style;
                    break;
                case iconStyle.solid:
                    if (_solidInstance == null)
                    {
                        _solidInstance = new OneColorIcons(style);
                    }
                    IconStyle=style;
                    break;
                case iconStyle.duotone:
                    if (_duotoneInstance == null)
                    {
                        _duotoneInstance = new OneColorIcons(style);
                    }
                    IconStyle=style;
                    break;
                default:
                    break;
            }
           
        }
        /// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="props">The props.</param>
		/// <returns></returns>
		public Bitmap GetImage(Properties iconProperty)
        {
            var props = new Properties(iconProperty);
            return GetImageInternal(props,IconStyle);
        }
        public Bitmap GetImage(NormalIconType type,Color? color, int size = 32)
        {
            Color iconColor = color == null ? Color.Black : (Color)color;
            var props = new Properties(type, iconColor, size);
            return GetImageInternal(props, IconStyle);
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
        private Bitmap ResizeImage(Bitmap imgToResize, OneColorIcons.Properties props)
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
        private Bitmap GetImageInternal(Properties props, iconStyle style)
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
                    if (style == iconStyle.duotone)
                    {
                        var a = props.Type.ToString();
                        string character1 = char.ConvertFromUtf32(((int)props.Type)+ 1048576);
                        Color mainColor = (Color)props.ForeColor;
                        Color secendColor = Color.FromArgb(mainColor.A - 150, mainColor);
                        g1.DrawString(character1, font, new SolidBrush(secendColor), 0, 0);
                    }
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
