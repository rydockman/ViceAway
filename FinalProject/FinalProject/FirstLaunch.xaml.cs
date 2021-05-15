using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FinalProject
{
    public partial class FirstLaunch : ContentPage
    {
        string[] vices = {"Smoking", "Drinking", "Gambling",
                                  "Marijuana", "Adderall", "Pills",
                                  "Drugs", "Sugar", "Fast Food",
                                  "Video Games"};
        string[] timeFrames = {"Day", "Week", "Month"};
        public FirstLaunch()
        {
            InitializeComponent();

            foreach (string vice in vices)
                vicePicker.Items.Add(vice);

            foreach (string timeFrame in timeFrames)
                timeFramePicker.Items.Add(timeFrame);

            stopDatePicker.Date = DateTime.Now;
            vicePicker.SelectedIndex = 0;
            timeFramePicker.SelectedIndex = 0;
            costStepper.Value = 0;
            currencyEntry.Text = "$";

        }
        async private void OnClicked(object sender, EventArgs e)
        {
            if (!Preferences.ContainsKey("StopDate"))
                Preferences.Set("StopDate", new DateTime(2000, 1, 1));
            Preferences.Set("StopDate", stopDatePicker.Date);

            if (!Preferences.ContainsKey("CurrentVice"))
                Preferences.Set("CurrentVice", "CurrentVice");
            Preferences.Set("CurrentVice", vices[vicePicker.SelectedIndex]);

            if (!Preferences.ContainsKey("TimeFrame"))
                Preferences.Set("TimeFrame", "TimeFrame");
            Preferences.Set("TimeFrame", timeFrames[timeFramePicker.SelectedIndex]);

            if (!Preferences.ContainsKey("Cost"))
                Preferences.Set("Cost", 0);
            Preferences.Set("Cost", costStepper.Value);

            if (!Preferences.ContainsKey("Currency"))
                Preferences.Set("Currency", "$");
            Preferences.Set("Currency", currencyEntry.Text);

            Preferences.Set("isFirstLaunch", false);

            await Navigation.PopModalAsync();
        }

        void costStepper_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            costLabel.Text = "is $" + costStepper.Value;
        }
    }
}
