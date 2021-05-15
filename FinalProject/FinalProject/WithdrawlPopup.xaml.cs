using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Plugin.SimpleAudioPlayer;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FinalProject
{
    public partial class WithdrawlPopup : ContentPage
    {
        string withdrawlReason;
        double withdrawlAmount;

        ISimpleAudioPlayer player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
        public WithdrawlPopup()
        {
            InitializeComponent();
            Load("chaching.mp3");
        }

        private void Load(string file)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            String ns = "FinalProject";
            Stream audioStream = assembly.GetManifestResourceStream(ns + "." + file);
            player.Load(audioStream);
        }

        private async void OptionClicked(System.Object sender, System.EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Text)
            {
                case "Amount to withdrawl":
                    try { 
                    withdrawlAmount = Double.Parse(await DisplayPromptAsync("How much are you spending?", "", keyboard: Keyboard.Numeric));
                    } catch
                    {
                        await DisplayAlert("Invalid Entry", "Numeric amounts only", "Ok");
                    }
                    if (Preferences.Get("CurrentBalance", 0.0) - withdrawlAmount >= 0)
                        amountLabel.Text = "$" + withdrawlAmount;
                    else
                        await DisplayAlert("Insufficient Balance", "", "Ok");
                    break;
                case "Reason to withdrawl":
                    withdrawlReason = await DisplayPromptAsync("Reason", "What are you spending on?");
                    reasonLabel.Text = withdrawlReason;
                    break;
            }
        }

        private async void SaveCancelClicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            switch (btn.Text.Trim(' '))
            {
                case "Done":
                    double amount = Double.Parse(amountLabel.Text.Substring(1));
                    string reason = reasonLabel.Text;

                    Withdrawl withdrawl = new Withdrawl
                    {
                        DatePerformed = DateTime.Now,
                        Amount = amount,
                        Reason = reason
                    };
                    DB.conn.Insert(withdrawl);
                    await Navigation.PopModalAsync();
                    player.Play();
                    break;
                case "Cancel":
                    await Navigation.PopModalAsync();
                    break;
            }
        }
    }
}
