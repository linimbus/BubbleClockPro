using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BubbleClock
{
    public partial class Form1 : Form
    {
        public Config config = new Config();

        private System.Windows.Forms.Timer timer;

        //private float amplitude = 30; // 漂浮幅度
        private Random random = new Random(); // 使用系统时间作为种子

        // 每个数字的中心点坐标
        private List<Point> centers = new List<Point>();

        private class FloatingText
        {
            public float offsetX, offsetY; // 中心的坐标
            public float angleX, angleY; // 用于控制漂浮路径的角度
            public float speedX, speedY; // 漂浮速度
        }

        private List<FloatingText> floatingTexts = new List<FloatingText>();

        private void InitializeTexts(int length)
        {
            for (int i = 0; i < length; i++)
            {
                floatingTexts.Add(new FloatingText
                {
                    offsetX = 0,
                    offsetY = 0,
                    angleX = i * (float)Math.PI, // 不同相位
                    angleY = i * (float)Math.PI / 2, // 不同相位
                    speedX = 0.03f + (float)random.NextDouble() * 0.02f,
                    speedY = 0.03f + (float)random.NextDouble() * 0.02f,
                });
            }
        }

        private void InitializeCenters(int length)
        {
            centers.Add(new Point((this.ClientSize.Width * 15) / 100, this.ClientSize.Height / 2));
            centers.Add(new Point((this.ClientSize.Width * 35) / 100, this.ClientSize.Height / 2));
            centers.Add(new Point((this.ClientSize.Width * 65) / 100, this.ClientSize.Height / 2));
            centers.Add(new Point((this.ClientSize.Width * 85) / 100, this.ClientSize.Height / 2));
        }

        private void FloatingTexts()
        {
            float amplitude = config.options.floatingSize;
            for (int i = 0; i < floatingTexts.Count; i++ )
            {
                floatingTexts[i].angleX += floatingTexts[i].speedX;
                floatingTexts[i].angleY += floatingTexts[i].speedY;
                floatingTexts[i].offsetY = amplitude * (float)Math.Sin(floatingTexts[i].angleY);
                floatingTexts[i].offsetX = amplitude * (float)Math.Sin(floatingTexts[i].angleX);
            }
        }

        // 全屏模式构造函数
        public Form1(Rectangle bounds)
        {
            InitializeForm();
            Bounds = bounds;
        }

        // 预览模式构造函数
        public Form1(IntPtr previewHandle)
        {
            InitializeForm();
            InitializePreview(previewHandle);
        }

        private void InitializeForm()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            Cursor.Hide(); // 隐藏鼠标光标

            // Load config
            this.config.LoadConfig();

            this.Name = "BubbleClockPro";
            this.Text = "BubbleClockPro Screen Saver";
            this.FormBorderStyle = FormBorderStyle.None;        // 无边框
            this.WindowState = FormWindowState.Maximized;       // 最大化窗口
            this.StartPosition = FormStartPosition.CenterScreen; // 居中显示
            this.BackColor = Color.Black;                       // 设置背景色
            this.ShowInTaskbar = false;                         // 不在任务栏显示
            this.TopMost = true;                               // 窗口置顶
            this.ResumeLayout(false);

            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyPress += Form1_KeyPress;
            this.MouseMove += Form1_MouseMove;
            this.MouseClick += Form1_MouseClick;
            this.DoubleBuffered = true; // 启用双缓冲减少闪烁

            // 添加以下代码确保窗体重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint, true);
        }

        private void InitializePreview(IntPtr previewHandle)
        {
            // 设置预览窗口父子关系
            InitializeForm();

            // 将窗体嵌入系统预览窗口
            Win32.SetParent(this.Handle, previewHandle);
            Win32.SetWindowLong(this.Handle, Win32.GWL_STYLE, Win32.WS_CHILD);

            // 调整窗体尺寸匹配预览区域
            Win32.GetClientRect(previewHandle, out Win32.RECT rect);
            Size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);

            // 禁用交互
            FormBorderStyle = FormBorderStyle.None;
            ControlBox = false;

            // 禁止浮动效果
            config.options.floatingEffect = false;

            // 禁止秒针跳动
            config.options.secondJump = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 设置窗体属性
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;

            // 初始化计时器
            timer = new System.Windows.Forms.Timer();
            timer.Interval = (1000/(int)(config.options.flashSpeed)); // 每 xx ms 更新一次
            timer.Tick += Timer_Tick;
            timer.Start();

            InitializeTexts(5);

            // 计算中心位置
            InitializeCenters(4);
        }

        private void DrawPoint(Graphics g, float x, float y, float size, Color color)
        {
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, x - size / 2, y - size / 2, size, size);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 刷新一次
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawClock2(e.Graphics);
        }

        private void DrawClock2(Graphics g)
        {
            var options = this.config.options;

            if (options.floatingEffect)
            {
                FloatingTexts();
            }

            // 设置高质量渲染
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            // 使用纯色填充背景
            using (SolidBrush brush = new SolidBrush(config.GetBackgroundColor()))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }

            // 获取当前时间
            DateTime now = DateTime.Now;
            var timeString = now.ToString("HHmm");

            using (Font font = new Font(options.fontName, this.ClientSize.Height/2, FontStyle.Bold))
            {
                var colors = config.GetFontColorAlpha(now.Hour);

                // Draw the "hour" and "minute" with pink gradient
                for ( int i = 0; i < 4; i++)
                {
                    SizeF textSize = g.MeasureString(timeString[i].ToString(), font);
                    var fontWidth = textSize.Width;
                    var fontHeight = textSize.Height;

                    using (LinearGradientBrush brushHour = new LinearGradientBrush(
                        new Point(0, 0),
                        new Point(0, this.Height),
                        colors[i % 2], // Lighter pink
                        colors[i % 2])) // Darker pink
                    {
                        //DrawPoint(g, centers[i].X, centers[i].Y, 10, Color.Wheat);

                        // Get the center of the form
                        float centerX = floatingTexts[i].offsetX + centers[i].X;
                        float centerY = floatingTexts[i].offsetY + centers[i].Y;

                        //DrawPoint(g, centerX, centerY, 10, Color.Blue);

                        float angleY = floatingTexts[i].angleY;

                        g.TranslateTransform(centerX, centerY); // 将原点移动到绘画中心

                        g.RotateTransform(3 * (float)Math.Sin(angleY));

                        g.TranslateTransform(-(fontWidth / 2), -(fontHeight / 2));
                        g.DrawString(timeString[i].ToString(), font, brushHour,  0, 100);

                        // 恢复画布的原始状态
                        g.ResetTransform();
                    }
                }

                if (now.Second % 2 == 0 || !options.secondJump)
                {
                    SizeF textSize = g.MeasureString(":", font);
                    var fontWidth = textSize.Width;
                    var fontHeight = textSize.Height;

                    // Draw the "second" with blue gradient
                    using (LinearGradientBrush brushPoint = new LinearGradientBrush(
                        new Point(0, 0),
                        new Point(0, this.Height),
                        colors[2], // Lighter blue
                        colors[2])) // Darker blue
                    {
                        // Get the center of the form
                        float centerX = floatingTexts[4].offsetY + this.ClientSize.Width/2;
                        float centerY = floatingTexts[4].offsetY + this.ClientSize.Height/2;

                        g.DrawString(":", font, brushPoint, centerX - (fontWidth/2), centerY - (fontHeight/2));
                    }
                }
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 按下任意键退出
            Application.Exit();
        }

        private Point lastMousePosition;
        private bool isFirstMove = true;

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isFirstMove)
            {
                // 记录第一次的鼠标位置
                lastMousePosition = new Point(e.X, e.Y);
                isFirstMove = false;
                return;
            }

            // 检测鼠标是否移动超过阈值（例如5像素）
            if (Math.Abs(e.X - lastMousePosition.X) > 5 || 
                Math.Abs(e.Y - lastMousePosition.Y) > 5)
            {
                Application.Exit();
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // 检测鼠标点击
            Application.Exit();
        }
    }
}

internal static class Win32
{
    [DllImport("user32.dll")]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    public static extern bool IsWindow(IntPtr hWnd);

    public const int GWL_STYLE = -16;
    public const int WS_CHILD = 0x40000000;

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}