using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Plugin.SimpleAudioPlayer;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FinalProject
{
    public partial class Numbers : ContentPage
    {
        FirstLaunch firstLaunch;
        ISimpleAudioPlayer player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
        string[] vices = {"Smoking", "Drinking", "Gambling",
                                  "Marijuana", "Adderall", "Pills",
                                  "Drugs", "Sugar", "Fast Food",
                                  "Video Games"};
        public Numbers()
        {
            InitializeComponent();
            Load("applause.mp3");
            if (!Preferences.ContainsKey("isFirstLaunch"))
                LaunchTutorial();
        }

        protected override void OnAppearing()
        {
            string currentVice = Preferences.Get("CurrentVice", "CurrentVice");
            int daysSince = (DateTime.Now - Preferences.Get("StopDate", new DateTime(2000, 1, 1))).Days;
            viceLabel.Text = "No " + currentVice + " For";
            xDaysLabel.Text = daysSince.ToString();

            CalculateMoneySaved();
        }

        private void Load(string file)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            String ns = "FinalProject";
            Stream audioStream = assembly.GetManifestResourceStream(ns + "." + file);
            player.Load(audioStream);
        }

        private async void LaunchTutorial()
        {
            firstLaunch = new FirstLaunch();
            firstLaunch.Disappearing += (sender2, e2) =>
            {
                OnAppearing();
            };
            await Navigation.PushModalAsync(firstLaunch, true);
            Preferences.Set("isFirstLaunch", true);
        }

        private void CalculateMoneySaved()
        {
            int daysSince = (DateTime.Now - Preferences.Get("StopDate", new DateTime(2000, 1, 1))).Days;
            double moneyPerDay = Preferences.Get("Cost", 0);
            string currency = Preferences.Get("Currency", "$");
            switch (Preferences.Get("TimeFrame", "TimeFrame"))
            {
                case "Day":
                    break;
                case "Week":
                    moneyPerDay /= 7;
                    break;
                case "Month":
                    moneyPerDay /= 30;
                    break;
            }

            if (!Preferences.ContainsKey("TotalSaved"))
                Preferences.Set("TotalSaved", 0.0);
            Preferences.Set("TotalSaved", Math.Round(moneyPerDay * daysSince, 2));

            currencySymbol.Text = currency.ToString();
            moneySavedLabel.Text = Preferences.Get("TotalSaved", 0.0).ToString();

        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            player.Play();
        }

    }
}
