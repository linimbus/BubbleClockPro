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

                key.Close();
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

                key.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"save config failed, reason is : {ex.Message}");
            }
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

        public Color GetBackgroundColor(int alpha)
        {
            return Color.FromArgb(alpha, (int)((options.backgroundColor >> 16) & 0xFF), (int)((options.backgroundColor >> 8) & 0xFF), (int)((options.backgroundColor) & 0xFF));
        }

        public Color GetHouColor(int alpha)
        {
            return Color.FromArgb(alpha, (int)((options.hourColor >> 16) & 0xFF), (int)((options.hourColor >> 8) & 0xFF), (int)((options.hourColor) & 0xFF));
        }

        public Color GetMinuteColor(int alpha)
        {
            return Color.FromArgb(alpha, (int)((options.minuteColor >> 16) & 0xFF), (int)((options.minuteColor >> 8) & 0xFF), (int)((options.minuteColor) & 0xFF));
        }

        public Color GetSecondColor(int alpha)
        {
            return Color.FromArgb(alpha, (int)((options.secondColor >> 16) & 0xFF), (int)((options.secondColor >> 8) & 0xFF), (int)((options.secondColor) & 0xFF));
        }
    }   
}
