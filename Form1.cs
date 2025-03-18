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
            this.ClientSize = new Size(600, 400);
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
                RowCount = 8,
                Dock = DockStyle.Fill,
            };

            topLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            topLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            var backgroundColorLabel = new Label
            {
                Text = "Background Color",
                Margin = new Padding(30, 10, 0, 0),
                AutoSize = true
            };

            var backgroundColorDisplayPanel = new Panel
            {
                Size = new Size(40, 40),
                BackColor = config.GetBackgroundColor(),
            };

            backgroundColorDisplayPanel.Click += (s, e) =>
            {
                var dialog = new ColorDialog();
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
                AutoSize = true
            };

            var hourColorDisplayPanel = new Panel
            {
                Size = new Size(40, 40),
                BackColor = config.GetHouColor(),
            };

            hourColorDisplayPanel.Click += (s, e) =>
            {
                var dialog = new ColorDialog();
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
                AutoSize = true
            };

            var minuteColorDisplayPanel = new Panel
            {
                Size = new Size(40, 40),
                BackColor = config.GetMinuteColor(),
            };

            minuteColorDisplayPanel.Click += (s, e) =>
            {
                var dialog = new ColorDialog();
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
                AutoSize = true
            };

            var secondColorDisplayPanel = new Panel
            {
                Size = new Size(40, 40),
                BackColor = config.GetSecondColor(),
            };

            secondColorDisplayPanel.Click += (s, e) =>
            {
                var dialog = new ColorDialog();
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
                AutoSize = true,
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
                var dialog = new FontDialog();
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

            var checkBox1 = new CheckBox
            {
                Text = "Floating Effect",
                AutoSize = true,
                Margin = new Padding(30, 5, 0, 0),
                Checked = options.floatingEffect,
            };

            checkBox1.CheckedChanged += (s, e) =>
            {
                options.floatingEffect = checkBox1.Checked;
            };

            var checkBox2 = new CheckBox
            {
                Text = "Seconds Jump Effect",
                AutoSize = true,
                Margin = new Padding(0, 5, 10, 0),
                Checked = options.secondJump,
            };

            checkBox2.CheckedChanged += (s, e) =>
            {
                options.secondJump = checkBox2.Checked;
            };

            var checkBox3 = new CheckBox
            {
                Text = "Random Font Color",
                AutoSize = true,
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
                AutoSize = true,
                Margin = new Padding(0, 5, 10, 0),
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

