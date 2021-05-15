using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Plugin.SimpleAudioPlayer;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FinalProject
{
    public partial class Bank : ContentPage
    {
        public Bank()
        {
            InitializeComponent();
            ResetListViewSources();
        }
        protected override void OnAppearing()
        {
            ResetListViewSources();
        }

        WithdrawlPopup withdrawlPopup;

        private async void LaunchWithdrawl()
        {
            withdrawlPopup = new WithdrawlPopup();
            withdrawlPopup.Disappearing += (sender2, e2) =>
            {
                ResetListViewSources();
            };
            await Navigation.PushModalAsync(withdrawlPopup, true);
            
        }

        public void ResetListViewSources()
        {
            List<Withdrawl> withdrawls = DB.conn.Table<Withdrawl>().ToList();

            double totalWithdrawls = 0;
            foreach (Withdrawl withdrawl in withdrawls)
                totalWithdrawls += withdrawl.Amount;

            if (!Preferences.ContainsKey("CurrentBalance"))
                Preferences.Set("CurrentBalance", 0.0);
            Preferences.Set("CurrentBalance", (Preferences.Get("TotalSaved", 0.0) - totalWithdrawls));

            bankLabel.Text = "$" + Preferences.Get("CurrentBalance", 0.0);
            withdrawlList.ItemsSource = withdrawls;
        }

        void WithdrawlClicked(System.Object sender, System.EventArgs e)
        {
            LaunchWithdrawl();
        }

        void withdrawlList_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            Withdrawl withdrawlTapped = (Withdrawl)((ListView)sender).SelectedItem;
            MoreInfoTap(withdrawlTapped);

        }

        async void MoreInfoTap(Withdrawl withdrawl)
        {
            bool answer = await DisplayAlert(withdrawl.DatePerformed.ToShortDateString() + " - $" + withdrawl.Amount.ToString(), "Reason: " + withdrawl.Reason, "Delete", "Cancel");
            if (answer)
            {
                bool areYouSure = await DisplayAlert("Are you sure?", "", "Delete", "Cancel");
                if (areYouSure)
                {
                    DB.conn.Delete(withdrawl);
                    ResetListViewSources();
                }
                }
            
        }
    }
}
