using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DB.OpenConnection();

            TabbedPage mainPage = new MainPage();
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
