using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Plugin.SimpleAudioPlayer;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FinalProject
{
    public partial class Settings : ContentPage
    {
        string[] vices = {"Smoking", "Drinking", "Gambling",
                                  "Marijuana", "Adderall", "Pills",
                                  "Drugs", "Sugar", "Fast Food",
                                  "Video Games"};
        string[] timeFrames = { "Day", "Week", "Month" };

        ISimpleAudioPlayer player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
        public Settings()
        {
            InitializeComponent();
            Load("success.mp3");

            foreach (string vice in vices)
                vicePicker.Items.Add(vice);

            foreach (string timeFrame in timeFrames)
                timeFramePicker.Items.Add(timeFrame);

        }

        protected override void OnAppearing()
        {
            stopDatePicker.Date = Preferences.Get("StopDate", new DateTime(2000, 1, 1));
            vicePicker.SelectedIndex = Array.IndexOf(vices, Preferences.Get("CurrentVice", "CurrentVice"));
            timeFramePicker.SelectedIndex = Array.IndexOf(timeFrames, Preferences.Get("TimeFrame", "TimeFrame"));
            costStepper.Value = Preferences.Get("Cost", 0);
            costLabel.Text = "is " + Preferences.Get("Currency", "$") + costStepper.Value;
            currencyEntry.Text = Preferences.Get("Currency", "$");
        }

        private void Load(string file)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            String ns = "FinalProject";
            Stream audioStream = assembly.GetManifestResourceStream(ns + "." + file);
            player.Load(audioStream);
        }

        private void OnClicked(object sender, EventArgs e)
        {

            Preferences.Set("StopDate", stopDatePicker.Date);

            Preferences.Set("CurrentVice", vices[vicePicker.SelectedIndex]);

            Preferences.Set("TimeFrame", timeFrames[timeFramePicker.SelectedIndex]);

            Preferences.Set("Cost", costStepper.Value);

            Preferences.Set("Currency", currencyEntry.Text);

            player.Play();
        }

        void costStepper_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            costLabel.Text = "is $" + costStepper.Value;
        }
    }
}
