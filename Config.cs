using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Win32;

namespace BubbleClock
{
    public partial class Config
    {
        public Options options = new Options();
        Random random = new Random();

        public class Options
        {
            public UInt32 backgroundColor = 0x00000000; // 纯黑色
            public UInt32 hourColor = 0x000061B6; // 深蓝色
            public UInt32 minuteColor = 0x00529EE8; // 淡蓝色
            public UInt32 secondColor = 0x00FFFFFF; // 白色
            public string fontName = "Arial Rounded MT Bold";
            public bool secondJump = true;
            public bool floatingEffect = true;
            public bool backgroundRandom = true;
            public bool fontRandom = true;

            public UInt32 flashSpeed = 120; // 120 Hz
            public UInt32 floatingSize = 30; // size of floating effect
            public List<UInt32> fontColorRange = new List<UInt32> {
                0x00CADBD4, // 淡灰色
                0x00F2D5CF, // 淡粉色
                0x00EFD5DC, // 淡紫色
                0x00DEA592, // 淡棕色
                0x00627676, // 深灰色
                0x00B5AF5A, // 深黄色
                0x0037613C, // 深绿色
                0x00478058, // 深蓝绿色
                0x00646D20, // 深黄绿色
                0x00B3CAD8, // 淡蓝色
            };

            public UInt32 fontAlpha = 70; // 70%
            public UInt32 SecondAlpha = 80; // 80%

            public int lastHour = -1; // 上一次的小时数, 用于随机颜色
            public List<Color> fontColors = new List<Color>();
        }

        public string LoadConfig()
        {
            try
            {
                // 打开或创建注册表项
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\BubbleClockPro", true);

                if (key == null)
                {
                    throw new Exception("Registry key not found.");
                }

                // 加载配置
                options.backgroundColor = Convert.ToUInt32(key.GetValue("BackgroundColor", options.backgroundColor));
                options.hourColor = Convert.ToUInt32(key.GetValue("HourColor", options.hourColor));
                options.minuteColor = Convert.ToUInt32(key.GetValue("MinuteColor", options.minuteColor));
                options.secondColor = Convert.ToUInt32(key.GetValue("SecondColor", options.secondColor));
                options.fontName = (string)(key.GetValue("FontName", options.fontName));
                options.secondJump = Convert.ToBoolean(key.GetValue("SecondJump", options.secondJump));
                options.floatingEffect = Convert.ToBoolean(key.GetValue("FloatingEffect", options.floatingEffect));
                options.backgroundRandom = Convert.ToBoolean(key.GetValue("BackgroundRandom", options.backgroundRandom));
                options.fontRandom = Convert.ToBoolean(key.GetValue("FontRandom", options.fontRandom));
                options.flashSpeed = Convert.ToUInt32(key.GetValue("FlashSpeed", options.flashSpeed));
                options.floatingSize = Convert.ToUInt32(key.GetValue("FloatingSize", options.floatingSize));

                // 读取 FontColorRange
                string fontColorRangeString = (string)(key.GetValue("FontColorRange", string.Empty));
                if (!string.IsNullOrEmpty(fontColorRangeString))
                {
                    options.fontColorRange = fontColorRangeString.Split(',')
                        .Select(color => Convert.ToUInt32(color))
                        .ToList();
                }

                options.fontAlpha = Convert.ToUInt32(key.GetValue("FontAlpha", options.fontAlpha));
                options.SecondAlpha = Convert.ToUInt32(key.GetValue("SecondAlpha", options.SecondAlpha));

                key.Close();

                FontColorListInit();
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"load config failed, reason is : {ex.Message}");
                return $"load config failed, reason is : {ex.Message}";
            }
        }

        public void SaveConfig()
        {
            try
            {
                // 打开或创建注册表项
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\BubbleClockPro");

                // 存储配置
                key.SetValue("BackgroundColor", options.backgroundColor);
                key.SetValue("HourColor", options.hourColor);
                key.SetValue("MinuteColor", options.minuteColor);
                key.SetValue("SecondColor", options.secondColor);

                key.SetValue("FontName", options.fontName);
                key.SetValue("SecondJump", options.secondJump);
                key.SetValue("FloatingEffect", options.floatingEffect);
                key.SetValue("BackgroundRandom", options.backgroundRandom);
                key.SetValue("FontRandom", options.fontRandom);
                key.SetValue("FlashSpeed", options.flashSpeed);
                key.SetValue("FloatingSize", options.floatingSize);

                // 将 fontColorRange 转换为字符串列表并保存
                List<string> fontColorRangeStrings = options.fontColorRange.Select(color => color.ToString()).ToList();
                key.SetValue("FontColorRange", string.Join(",", fontColorRangeStrings));

                key.SetValue("FontAlpha", options.fontAlpha);
                key.SetValue("SecondAlpha", options.SecondAlpha);

                key.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"save config failed, reason is : {ex.Message}");
            }
        }

        public Color GetFontColor(int index)
        {
            return Color.FromArgb((int)((options.fontColorRange[index] >> 16) & 0xFF), (int)((options.fontColorRange[index] >> 8) & 0xFF), (int)((options.fontColorRange[index]) & 0xFF));
        }

        public void SetFontColor(int index, Color color)
        {
            options.fontColorRange[index] = (UInt32)((color.R << 16) | (color.G << 8) | color.B);
        }

        public Color GetBackgroundColor()
        {
            return Color.FromArgb((int)((options.backgroundColor >> 16) & 0xFF), (int)((options.backgroundColor >> 8) & 0xFF), (int)((options.backgroundColor) & 0xFF));
        }

        public Color GetHouColor() { 
            return Color.FromArgb((int)((options.hourColor >> 16) & 0xFF), (int)((options.hourColor >> 8) & 0xFF), (int)((options.hourColor) & 0xFF));
        }

        public Color GetMinuteColor()
        {
            return Color.FromArgb((int)((options.minuteColor >> 16) & 0xFF), (int)((options.minuteColor >> 8) & 0xFF), (int)((options.minuteColor) & 0xFF));
        }

        public Color GetSecondColor()
        {
            return Color.FromArgb((int)((options.secondColor >> 16) & 0xFF), (int)((options.secondColor >> 8) & 0xFF), (int)((options.secondColor) & 0xFF));
        }

        private Color getFontColorAlpha(int index)
        {
            int alpha = (int)((options.fontAlpha * 255) / 100);
            return Color.FromArgb(alpha, (int)((options.fontColorRange[index] >> 16) & 0xFF), (int)((options.fontColorRange[index] >> 8) & 0xFF), (int)((options.fontColorRange[index]) & 0xFF));
        }

        private void FontColorListInit()
        {
            options.fontColors.Clear();
            if (options.fontRandom)
            {
                var idx1 = (random.Next() % options.fontColorRange.Count);
                while (true)
                {
                    var idx2 = (random.Next() % options.fontColorRange.Count);
                    if (idx2 != idx1)
                    {
                        options.fontColors.Add(getFontColorAlpha(idx1));
                        options.fontColors.Add(getFontColorAlpha(idx2));
                        break;
                    }
                }
            }
            else
            {
                options.fontColors.Add(GetHourColorAlpha());
                options.fontColors.Add(GetMinuteColorAlpha());
            }
            options.fontColors.Add(GetSecondColorAlpha());
        }

        public List<Color> GetFontColorAlpha(int hour)
        {
            if (options.fontRandom)
            {
                if (hour != options.lastHour)
                {
                    options.lastHour = hour;
                    FontColorListInit();
                }
            } 
            return options.fontColors;
        }


        public Color GetHourColorAlpha()
        {
            int alpha = (int)((options.fontAlpha * 255) / 100);
            return Color.FromArgb(alpha, (int)((options.hourColor >> 16) & 0xFF), (int)((options.hourColor >> 8) & 0xFF), (int)((options.hourColor) & 0xFF));
        }

        public Color GetMinuteColorAlpha()
        {
            int alpha = (int)((options.fontAlpha * 255) / 100);
            return Color.FromArgb(alpha, (int)((options.minuteColor >> 16) & 0xFF), (int)((options.minuteColor >> 8) & 0xFF), (int)((options.minuteColor) & 0xFF));
        }

        public Color GetSecondColorAlpha()
        {
            int alpha = (int)((options.SecondAlpha * 255) / 100);
            return Color.FromArgb(alpha, (int)((options.secondColor >> 16) & 0xFF), (int)((options.secondColor >> 8) & 0xFF), (int)((options.secondColor) & 0xFF));
        }
    }   
}
