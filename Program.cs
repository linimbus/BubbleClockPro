namespace BubbleClock
{
    internal static class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                string arg = args[0].ToLower().Trim();
                if (arg.StartsWith("/p"))
                {
                    HandlePreview(args);
                }
                else if (arg.StartsWith("/s"))
                {
                    StartFullScreen();
                }
                else if (arg.StartsWith("/c"))
                {
                    ShowConfiguration();
                }
            }
            else
            {
                StartFullScreen();
            }
        }

        static void HandlePreview(string[] args)
        {
            if (args.Length < 2 || !long.TryParse(args[1], out long handle))
            {
                return;
            }
            var previewForm = new Form1(new IntPtr(handle));
            Application.Run(previewForm);
        }

        static void ShowConfiguration()
        {
            Application.Run(new ConfigForm());
        }

        static void StartFullScreen()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                var form = new Form1(screen.Bounds);
                form.Show();
            }
            Application.Run();
        }
    }
}