using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Text;

namespace ZP.Project.Web
{
    /// <summary>
    /// VerifyCodeHandler 的摘要说明
    /// </summary>
    public class VerifyCodeHandler : IHttpHandler, IRequiresSessionState
    {
        private HttpContext _context;
        public void ProcessRequest(HttpContext context)
        {
            this._context = context;
            this._random = new Random();
            this._code = GetRandomCode();
            this._context.Session["ValidateCode"] = this._code;
            //var cookie = this._context.Request.Cookies["ASP.NET_SessionId"];
            //cookie.Secure = true;
            //this._context.Response.SetCookie(cookie);
            this.SetPageNoCache();
            this.OnPaint();
        }


        static string[] FontItems = new string[] { "Arial", "Helvetica", "Geneva", "sans-serif", "Verdana", "黑体", "微软雅黑", "新宋体" };

        static Brush[] BrushItems = new Brush[] { Brushes.OliveDrab,
                                                  Brushes.ForestGreen,
                                                  Brushes.DarkCyan,
                                                  Brushes.LightSlateGray,
                                                  Brushes.RoyalBlue,
                                                  Brushes.SlateBlue,
                                                  Brushes.DarkViolet,
                                                  Brushes.MediumVioletRed,
                                                  Brushes.IndianRed,
                                                  Brushes.Firebrick,
                                                  Brushes.Chocolate,
                                                  Brushes.Peru,
                                                  Brushes.Goldenrod
                                            };

        static string[] BrushName = new string[] {    "OliveDrab",
                                                  "ForestGreen",
                                                  "DarkCyan",
                                                  "LightSlateGray",
                                                  "RoyalBlue",
                                                  "SlateBlue",
                                                  "DarkViolet",
                                                  "MediumVioletRed",
                                                  "IndianRed",
                                                  "Firebrick",
                                                  "Chocolate",
                                                  "Peru",
                                                  "Goldenrod"
                                             };

        private static Color BackColor = Color.White;
        private static Pen BorderColor = Pens.DarkGray;
        private static int Width = 120;
        private static int Height = 38;

        private Random _random;
        private string _code;
        private int _brushNameIndex;


        /**/
        /**/
        /**/
        /// <summary>
        /// 设置页面不被缓存
        /// </summary>
        private void SetPageNoCache()
        {
            this._context.Response.Buffer = true;
            this._context.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            this._context.Response.Expires = 0;
            this._context.Response.CacheControl = "no-cache";
            this._context.Response.AppendHeader("Pragma", "No-Cache");
        }

        /**/
        /**/
        /**/
        /// <summary>
        /// 取得一个 4 位的随机码
        /// </summary>
        /// <returns></returns>
        private string GetRandomCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 4);
        }

        /**/
        /**/
        /**/
        /// <summary>
        /// 随机取一个字体
        /// </summary>
        /// <returns></returns>
        private Font GetFont()
        {
            int fontIndex = _random.Next(0, FontItems.Length);
            FontStyle fontStyle = GetFontStyle(_random.Next(0, 2));
            return new Font(FontItems[fontIndex], 22, fontStyle);
        }

        /**/
        /**/
        /**/
        /// <summary>
        /// 取一个字体的样式
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private FontStyle GetFontStyle(int index)
        {
            switch (index)
            {
                case 0:
                    return FontStyle.Bold;
                case 1:
                    return FontStyle.Italic;
                default:
                    return FontStyle.Regular;
            }
        }

        /**/
        /**/
        /**/
        /// <summary>
        /// 随机取一个笔刷
        /// </summary>
        /// <returns></returns>
        private Brush GetBrush()
        {
            int brushIndex = _random.Next(0, BrushItems.Length);
            _brushNameIndex = brushIndex;
            return BrushItems[brushIndex];
        }



        /**/
        /**/
        /**/
        /// <summary>
        /// 绘画事件
        /// </summary>
        private void OnPaint()
        {
            Bitmap objBitmap = null;
            Graphics g = null;

            try
            {
                objBitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                g = Graphics.FromImage(objBitmap);

                Paint_Background(g);
                Paint_Text(g);
                Paint_TextStain(objBitmap);
                //Paint_Border(g);

                Bitmap waveBitmap = null;
                try { waveBitmap = WaveDistortion(objBitmap); }
                catch { }
                objBitmap = waveBitmap != null ? waveBitmap : objBitmap;

                objBitmap.Save(this._context.Response.OutputStream, ImageFormat.Jpeg);
                this._context.Response.ContentType = "image/jpg";
            }
            catch { }
            finally
            {
                if (null != objBitmap)
                    objBitmap.Dispose();
                if (null != g)
                    g.Dispose();
            }
        }

        /**/
        /**/
        /**/
        /// <summary>
        /// 绘画背景颜色
        /// </summary>
        /// <param name="g"></param>
        private void Paint_Background(Graphics g)
        {
            g.Clear(BackColor);
        }

        /**/
        /**/
        /**/
        /// <summary>
        /// 绘画边框
        /// </summary>
        /// <param name="g"></param>
        private void Paint_Border(Graphics g)
        {
            g.DrawRectangle(BorderColor, 0, 0, Width - 1, Height - 1);
        }

        /**/
        /**/
        /**/
        /// <summary>
        /// 绘画文字
        /// </summary>
        /// <param name="g"></param>
        private void Paint_Text(Graphics g)
        {
            g.DrawString(_code, GetFont(), GetBrush(), 6, 3);
        }

        /**/
        /**/
        /**/
        /// <summary>
        /// 绘画文字噪音点
        /// </summary>
        /// <param name="g"></param>
        private void Paint_TextStain(Bitmap b)
        {
            for (int n = 0; n < 30; n++)
            {
                int x = _random.Next(Width);
                int y = _random.Next(Height);
                b.SetPixel(x, y, Color.FromName(BrushName[_brushNameIndex]));
            }

        }





        #region  KCAPTCHA 波纹扭曲
        /// <summary>
        /// # KCAPTCHA PROJECT VERSION 1.2.6
        /// www.captcha.ru, www.kruglov.ru
        /// 波形扭曲 FROM KCAPTCHA
        /// </summary>
        /// <param name="srcBmp">待扭曲的图像 必须为 PixelFormat.Format24bppRgb 格式图像</param>
        /// <returns></returns>
        private Bitmap WaveDistortion(Bitmap srcBmp)
        {
            Random randx = new Random();
            if (srcBmp == null)
                return null;
            if (srcBmp.PixelFormat != PixelFormat.Format24bppRgb)
                throw new ArgumentException("srcBmp PixelFormat.Format24bppRgb 格式图像", "srcBmp");

            var width = srcBmp.Width;
            var height = srcBmp.Height;

            Bitmap destBmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            {
                //前景色
                Color foreground_color = Color.FromArgb(randx.Next(10, 100), randx.Next(10, 100), randx.Next(10, 100));
                //背景色
                //Color background_color = Color.FromArgb(randx.Next(200, 250), randx.Next(200, 250), randx.Next(200, 250));
                Color background_color = Color.White;
                using (Graphics newG = Graphics.FromImage(destBmp))
                {
                    newG.Clear(background_color);
                    // periods 时间
                    double rand1 = randx.Next(710000, 1200000) / 10000000.0;
                    double rand2 = randx.Next(710000, 1200000) / 10000000.0;
                    double rand3 = randx.Next(710000, 1200000) / 10000000.0;
                    double rand4 = randx.Next(710000, 1200000) / 10000000.0;
                    // phases  相位
                    double rand5 = randx.Next(0, 31415926) / 10000000.0;
                    double rand6 = randx.Next(0, 31415926) / 10000000.0;
                    double rand7 = randx.Next(0, 31415926) / 10000000.0;
                    double rand8 = randx.Next(0, 31415926) / 10000000.0;
                    // amplitudes 振幅
                    double rand9 = randx.Next(330, 420) / 110.0;
                    double rand10 = randx.Next(330, 450) / 110.0;
                    double amplitudesFactor = randx.Next(5, 6) / 10.0;//振幅小点防止出界
                    double center = width / 2.0;

                    //wave distortion 波纹扭曲
                    BitmapData destData = destBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, destBmp.PixelFormat);
                    BitmapData srcData = srcBmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, srcBmp.PixelFormat);
                    for (var x = 0; x < width; x++)
                    {
                        for (var y = 0; y < height; y++)
                        {
                            var sx = x + (Math.Sin(x * rand1 + rand5)
                                        + Math.Sin(y * rand3 + rand6)) * rand9 - width / 2 + center + 1;
                            var sy = y + (Math.Sin(x * rand2 + rand7)
                                        + Math.Sin(y * rand4 + rand8)) * rand10 * amplitudesFactor; //振幅小点防止出界

                            int color, color_x, color_y, color_xy;
                            Color overColor = Color.Empty;

                            if (sx < 0 || sy < 0 || sx >= width - 1 || sy >= height - 1)
                            {
                                continue;
                            }
                            else
                            {
                                color = BitmapDataColorAt(srcData, (int)sx, (int)sy).B;
                                color_x = BitmapDataColorAt(srcData, (int)(sx + 1), (int)sy).B;
                                color_y = BitmapDataColorAt(srcData, (int)sx, (int)(sy + 1)).B;
                                color_xy = BitmapDataColorAt(srcData, (int)(sx + 1), (int)(sy + 1)).B;
                            }

                            if (color == 255 && color_x == 255 && color_y == 255 && color_xy == 255)
                            {
                                continue;
                            }
                            else if (color == 0 && color_x == 0 && color_y == 0 && color_xy == 0)
                            {
                                overColor = Color.FromArgb(foreground_color.R, foreground_color.G, foreground_color.B);
                            }
                            else
                            {
                                double frsx = sx - Math.Floor(sx);
                                double frsy = sy - Math.Floor(sy);
                                double frsx1 = 1 - frsx;
                                double frsy1 = 1 - frsy;

                                double newColor =
                                     color * frsx1 * frsy1 +
                                     color_x * frsx * frsy1 +
                                     color_y * frsx1 * frsy +
                                     color_xy * frsx * frsy;

                                if (newColor > 255) newColor = 255;
                                newColor = newColor / 255;
                                double newcolor0 = 1 - newColor;

                                int newred = Math.Min((int)(newcolor0 * foreground_color.R + newColor * background_color.R), 255);
                                int newgreen = Math.Min((int)(newcolor0 * foreground_color.G + newColor * background_color.G), 255);
                                int newblue = Math.Min((int)(newcolor0 * foreground_color.B + newColor * background_color.B), 255);

                                overColor = Color.FromArgb(newred, newgreen, newblue);
                            }
                            BitmapDataColorSet(destData, x, y, overColor);
                        }
                    }
                    destBmp.UnlockBits(destData);
                    srcBmp.UnlockBits(srcData);
                    this.Paint_Border(newG);
                }
                if (srcBmp != null)
                    srcBmp.Dispose();
            }
            return destBmp;
        }

        /// <summary>
        /// 获得 BitmapData 指定坐标的颜色信息
        /// 实现 PHP imagecolorat
        /// </summary>
        /// <param name="srcData">从图像数据获得颜色 必须为 PixelFormat.Format24bppRgb 格式图像数据</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>x,y 坐标的颜色数据</returns>
        /// <remarks>
        /// Format24BppRgb 已知X，Y坐标，像素第一个元素的位置为Scan0+(Y*Stride)+(X*3)。
        /// 这是blue字节的位置，接下来的2个字节分别含有green、red数据。
        /// </remarks>
        static Color BitmapDataColorAt(BitmapData srcData, int x, int y)
        {
            if (srcData.PixelFormat != PixelFormat.Format24bppRgb)
                throw new ArgumentException("srcData PixelFormat.Format24bppRgb 格式图像数据", "srcData");

            byte[] rgbValues = new byte[3];
            Marshal.Copy((IntPtr)((int)srcData.Scan0 + ((y * srcData.Stride) + (x * 3))), rgbValues, 0, 3);
            return Color.FromArgb(rgbValues[2], rgbValues[1], rgbValues[0]);
        }
        /// <summary>
        /// 设置 BitmapData 指定坐标的颜色信息
        /// 实现 PHP ImageColorSet
        /// </summary>
        /// <param name="destData">设置图像数据的颜色 必须为 PixelFormat.Format24bppRgb 格式图像数据</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color">待设置颜色</param>
        /// <remarks>
        /// Format24BppRgb 已知X，Y坐标，像素第一个元素的位置为Scan0+(Y*Stride)+(X*3)。
        /// 这是blue字节的位置，接下来的2个字节分别含有green、red数据。
        /// </remarks>
        static void BitmapDataColorSet(BitmapData destData, int x, int y, Color color)
        {
            if (destData.PixelFormat != PixelFormat.Format24bppRgb)
                throw new ArgumentException("destData PixelFormat.Format24bppRgb 格式图像数据", "destData");

            byte[] rgbValues = new byte[3] { color.B, color.G, color.R };
            Marshal.Copy(rgbValues, 0, (IntPtr)((int)destData.Scan0 + ((y * destData.Stride) + (x * 3))), 3);
        }
        #endregion


        #region 生成随机汉字
        public string CreateRegionCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
            Random rnd = new Random();
            //定义一个object数组用来
            object[] bytes = new object[strlength];
            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中
             每个汉字有四个区位码组成
             区位码第1位和区位码第2位作为字节数组第一个元素
             区位码第3位和区位码第4位作为字节数组第二个元素
             */
            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();
                //区位码第2位
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的种子避免产生重复值
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();
                //区位码第3位
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();
                //区位码第4位
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();
                //定义两个字节变量存储产生的随机汉字区位码
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中
                byte[] str_r = new byte[] { byte1, byte2 };
                //将产生的一个汉字的字节数组放入object数组中
                bytes.SetValue(str_r, i);
            }
            Encoding gb = Encoding.GetEncoding("gb2312");
            string[] str = new string[strlength];
            string chars = "";
            for (int i = 0; i < strlength; i++)
            {
                str[i] = gb.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[])));
                chars = chars + str[i];
            }
            return chars;
        }
        #endregion



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }
}