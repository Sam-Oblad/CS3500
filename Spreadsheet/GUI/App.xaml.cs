namespace GUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = new Window(MainPage)
            {
                Width = 840,
                Height = 472,
            };
            return window;
        }
    }
}
