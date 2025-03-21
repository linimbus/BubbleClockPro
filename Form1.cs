// ConfigForm.cs
using System.Windows.Forms;
using System.Drawing; // Add this using directive
using System.ComponentModel;
using System.Drawing.Printing;

namespace BubbleClock
{
    public partial class ConfigForm : Form
    {
        public Config config = new Config();

        public ConfigForm()
        {
            InitializeComponent();
            InitializeUI();
            //InitializeAbout();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ConfigForm));
            SuspendLayout();

            // 
            // ConfigForm
            // 
            this.Icon = (Icon?)resources.GetObject("$this.Icon");
            this.ClientSize = new Size(500, 800);
            this.Name = "Bubble Clock Pro Settings";
            this.Text = "Bubble Clock Pro Settings";
            this.Font = new Font("Arial", 9, FontStyle.Regular); // 设置默认字体
            this.AutoScaleMode = AutoScaleMode.Font;

            ResumeLayout(false);
        }

        private void InitializeAbout()
        {
            var aboutLayout = new TableLayoutPanel
            {
                ColumnCount = 1,
                RowCount = 4,
                Dock = DockStyle.Fill,
            };

            aboutLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            aboutLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            aboutLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            aboutLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));

            var aboutTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Text = "BubbleClock is a simple windows screensaver that can make the windows standby display similar to the iPhone standby digital clock.\r\n\r\n",
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                Font = new Font("Arial", 12, FontStyle.Regular),
            };

            var linkButton = new Button
            {
                Text = "Official Website",
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
            };

            linkButton.Click += (s, e) =>
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://bubbleclock.sourceforge.io",
                    UseShellExecute = true
                });
            };

            var link2Button = new Button
            {
                Text = "PayPal.ME",
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
            };

            link2Button.Click += (s, e) =>
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://paypal.me/lixiangyun",
                    UseShellExecute = true
                });
            };

            var closeButton = new Button
            {
                Text = "OK",
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
            };

            closeButton.Click += (s, e) =>
            {
                Close();
            };

            aboutLayout.Controls.Add(aboutTextBox, 0, 0);
            aboutLayout.Controls.Add(linkButton, 0, 1);
            aboutLayout.Controls.Add(link2Button, 0, 1);
            aboutLayout.Controls.Add(closeButton, 0, 2);

            Controls.Add(aboutLayout);
        }

        private void InitializeUI()
        {
            // Load config
            config.LoadConfig();

            var options = config.options;

            var topLayout = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 21,
                Dock = DockStyle.Fill,
            };

            topLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            topLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            var backgroundColorLabel = new Label
            {
                Text = "Background Color",
                Margin = new Padding(30, 10, 0, 0),
                Size = new Size(200, 40)
            };

            var backgroundColorDisplayPanel = new Panel
            {
                Size = new Size(40, 40),
                BackColor = config.GetBackgroundColor(),
            };

            backgroundColorDisplayPanel.Click += (s, e) =>
            {
                var dialog = new ColorDialog {
                    Color = config.GetBackgroundColor()
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    backgroundColorDisplayPanel.BackColor = dialog.Color;
                    options.backgroundColor = (UInt32)(dialog.Color.R << 16 | dialog.Color.G << 8 | dialog.Color.B);
                }
            };

            topLayout.Controls.Add(backgroundColorLabel);
            topLayout.Controls.Add(backgroundColorDisplayPanel);

            var hourColorLabel = new Label
            {
                Text = "Hours Color",
                Margin = new Padding(30, 5, 0, 0),
                Size = new Size(200, 40),
            };

            var hourColorDisplayPanel = new Panel
            {
                Size = new Size(40, 40),
                BackColor = config.GetHouColor(),
            };

            hourColorDisplayPanel.Click += (s, e) =>
            {
                var dialog = new ColorDialog { 
                    Color = config.GetHouColor()
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    hourColorDisplayPanel.BackColor = dialog.Color;
                    options.hourColor = (UInt32)(dialog.Color.R << 16 | dialog.Color.G << 8 | dialog.Color.B);
                }
            };

            topLayout.Controls.Add(hourColorLabel);
            topLayout.Controls.Add(hourColorDisplayPanel);

            var minuteColorLabel = new Label
            {
                Text = "Minute Color",
                Margin = new Padding(30, 5, 0, 0),
                Size = new Size(200, 40)
            };

            var minuteColorDisplayPanel = new Panel
            {
                Size = new Size(40, 40),
                BackColor = config.GetMinuteColor(),
            };

            minuteColorDisplayPanel.Click += (s, e) =>
            {
                var dialog = new ColorDialog { 
                    Color = config.GetMinuteColor()
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    minuteColorDisplayPanel.BackColor = dialog.Color;
                    options.minuteColor = (UInt32)(dialog.Color.R << 16 | dialog.Color.G << 8 | dialog.Color.B);
                }
            };

            topLayout.Controls.Add(minuteColorLabel);
            topLayout.Controls.Add(minuteColorDisplayPanel);

            var secondColorLabel = new Label
            {
                Text = "Second Color",
                Margin = new Padding(30, 5, 0, 0),
                Size = new Size(200, 40)
            };

            var secondColorDisplayPanel = new Panel
            {
                Size = new Size(40, 40),
                BackColor = config.GetSecondColor(),
            };

            secondColorDisplayPanel.Click += (s, e) =>
            {
                var dialog = new ColorDialog {
                    Color = config.GetSecondColor()
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    secondColorDisplayPanel.BackColor = dialog.Color;
                    options.secondColor = (UInt32)(dialog.Color.R << 16 | dialog.Color.G << 8 | dialog.Color.B);
                }
            };

            topLayout.Controls.Add(secondColorLabel);
            topLayout.Controls.Add(secondColorDisplayPanel);

            var fontLabel = new Label
            {
                Text = "Font Style",
                Size = new Size(200, 40),
                Margin = new Padding(30, 5, 0, 0)
            };

            var fontButton = new Button
            {
                Text = options.fontName,
                Size = new Size(200, 40),
                Margin = new Padding(0, 5, 0, 0),
                Font = new Font(options.fontName, 9, FontStyle.Regular) // 默认字体
            };

            fontButton.Click += (s, e) =>
            {
                var dialog = new FontDialog { 
                    Font = new Font(options.fontName, 9, FontStyle.Regular) // 默认字体
                }; 
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    //Properties.Settings.Default.Font = dialog.Font;
                    fontButton.Font = dialog.Font;
                    fontButton.Text = dialog.Font.Name;
                    options.fontName = dialog.Font.Name;
                }
            };

            topLayout.Controls.Add(fontLabel);
            topLayout.Controls.Add(fontButton);

            var flashSpeedLabel = new Label
            {
                Text = "Flash Speed",
                Size = new Size(200, 40),
                Margin = new Padding(30, 5, 0, 0)
            };

            var flashSpeedComboBox = new ComboBox
            {
                Size = new Size(200, 40),
                Margin = new Padding(0, 5, 0, 0),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            flashSpeedComboBox.Items.AddRange(new object[] { "10 Hz", "20 Hz", "30 Hz", "60 Hz", "120 Hz" });
            flashSpeedComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            flashSpeedComboBox.SelectedIndex = 0;
            for (int i = 0; i < flashSpeedComboBox.Items.Count; i++)
            {
                string item = (string)flashSpeedComboBox.Items[i];
                var value = options.flashSpeed.ToString();
                if (item.IndexOf(value) == 0)
                {
                    flashSpeedComboBox.SelectedIndex = i;
                    break;
                }
            }
            flashSpeedComboBox.SelectedIndexChanged += (s, e) =>
            {
                string value = (string)flashSpeedComboBox.SelectedItem;
                if (value.IndexOf("10") == 0)
                {
                    options.flashSpeed = 10;
                }
                else if (value.IndexOf("20") == 0)
                {
                    options.flashSpeed = 20;
                }
                else if (value.IndexOf("30") == 0)
                {
                    options.flashSpeed = 30;
                }
                else if (value.IndexOf("60") == 0)
                {
                    options.flashSpeed = 60;
                }
                else if (value.IndexOf("120") == 0)
                {
                    options.flashSpeed = 120;
                }
                else
                {
                    options.flashSpeed = 10;
                }
            };

            topLayout.Controls.Add(flashSpeedLabel);
            topLayout.Controls.Add(flashSpeedComboBox);

            var floatingSizeLabel = new Label
            {
                Text = "Floating Size",
                Size = new Size(200, 40),
                Margin = new Padding(30, 5, 0, 0)
            };

            var floatingSizeNumericUpDown = new NumericUpDown
            {
                Size = new Size(200, 40),
                Margin = new Padding(0, 5, 0, 0),
                Minimum = 1,
                Maximum = 100,
                Value = options.floatingSize
            };

            floatingSizeNumericUpDown.ValueChanged += (s, e) =>
            {
                options.floatingSize = (UInt32)floatingSizeNumericUpDown.Value;
            };

            topLayout.Controls.Add(floatingSizeLabel);
            topLayout.Controls.Add(floatingSizeNumericUpDown);

            var fontAlphaLabel = new Label
            {
                Text = "Font Alpha",
                Size = new Size(200, 40),
                Margin = new Padding(30, 5, 0, 0)
            };

            var fontAlphaNumericUpDown = new NumericUpDown
            {
                Size = new Size(200, 40),
                Margin = new Padding(0, 5, 0, 0),
                Minimum = 1,
                Maximum = 100,
                Value = options.fontAlpha
            };

            fontAlphaNumericUpDown.ValueChanged += (s, e) =>
            {
                options.fontAlpha = (UInt32)fontAlphaNumericUpDown.Value;
            };

            topLayout.Controls.Add(fontAlphaLabel);
            topLayout.Controls.Add(fontAlphaNumericUpDown);

            var secondAlphaLabel = new Label
            {
                Text = "Second Alpha",
                Size = new Size(200, 40),
                Margin = new Padding(30, 5, 0, 0)
            };

            var secondAlphaNumericUpDown = new NumericUpDown
            {
                Size = new Size(200, 40),
                Margin = new Padding(0, 5, 0, 0),
                Minimum = 1,
                Maximum = 100,
                Value = options.SecondAlpha
            };

            secondAlphaNumericUpDown.ValueChanged += (s, e) =>
            {
                options.SecondAlpha = (UInt32)secondAlphaNumericUpDown.Value;
            };

            topLayout.Controls.Add(secondAlphaLabel);
            topLayout.Controls.Add(secondAlphaNumericUpDown);

            // Select Font Color Range

            var fontLayout = new TableLayoutPanel
            {
                ColumnCount = 5,
                RowCount = 2,
                Dock = DockStyle.Fill,
            };

            var fontRandomColorLabel = new Label
            {
                Text = "Font Random Color Range",
                Size = new Size(200, 40),
                Margin = new Padding(30, 5, 0, 0)
            };


            List<Panel> displayPanels = new List<Panel>();

            for (int i = 0; i < 10; i++)
            {
                int index = i; // 拷贝引用
                displayPanels.Add(new Panel
                {
                    Size = new Size(40, 40),
                    BackColor = config.GetFontColor(index),
                });

                displayPanels[index].Click += (s, e) =>
                {
                    var dialog = new ColorDialog
                    {
                        Color = config.GetFontColor(index),
                    };
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        displayPanels[index].BackColor = dialog.Color;
                        config.SetFontColor(index, dialog.Color);
                    }
                };

                fontLayout.Controls.Add(displayPanels[index]);
            }

            topLayout.Controls.Add(fontRandomColorLabel);
            topLayout.Controls.Add(fontLayout);

            var checkBox1 = new CheckBox
            {
                Text = "Floating Effect",
                Size = new Size(200, 40),
                Margin = new Padding(30, 5, 0, 0),
                Checked = options.floatingEffect,
            };

            checkBox1.CheckedChanged += (s, e) =>
            {
                options.floatingEffect = checkBox1.Checked;
            };

            var checkBox2 = new CheckBox
            {
                Text = "Seconds Jump",
                Size = new Size(200, 40),
                Margin = new Padding(0, 5, 0, 0),
                Checked = options.secondJump,
            };

            checkBox2.CheckedChanged += (s, e) =>
            {
                options.secondJump = checkBox2.Checked;
            };

            var checkBox3 = new CheckBox
            {
                Text = "Random Font Color",
                Size = new Size(200, 40),
                Margin = new Padding(30, 5, 0, 0),
                Checked = options.fontRandom,
            };

            checkBox3.CheckedChanged += (s, e) =>
            {
                options.fontRandom = checkBox3.Checked;
            };

            var checkBox4 = new CheckBox
            {
                Text = "Random Backgound Color",
                Size = new Size(250, 40),
                Margin = new Padding(0, 5, 0, 0),
                Checked = options.backgroundRandom,
            };

            checkBox4.CheckedChanged += (s, e) =>
            {
                options.backgroundRandom = checkBox4.Checked;
            };

            topLayout.Controls.Add(checkBox1);
            topLayout.Controls.Add(checkBox2);
            topLayout.Controls.Add(checkBox3);
            topLayout.Controls.Add(checkBox4);

            var saveButton = new Button
            {
                Text = "Save",
                Size = new Size(200, 40),
                Margin = new Padding(30, 20, 0, 0)
            };

            saveButton.Click += (s, e) =>
            {
                config.SaveConfig();
                Close();
            };

            var cancelButton = new Button
            {
                Text = "Cancel",
                Size = new Size(200, 40),
                Margin = new Padding(0, 20, 10, 0)
            };

            cancelButton.Click += (s, e) =>
            {
                Close();
            };

            topLayout.Controls.Add(saveButton);
            topLayout.Controls.Add(cancelButton);

            Controls.Add(topLayout);
        }
    }
}

