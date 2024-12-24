namespace MedicalSystem
{
    internal static class Program
    {
        public static SystemController Controller { get; private set; }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

            Controller = new SystemController();
        }
    }
}