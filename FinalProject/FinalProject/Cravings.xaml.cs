using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FinalProject
{
    public partial class Cravings : ContentPage
    {
        List<ChartData> chartList;
        string[] cravingsString = {"I'm at a party or in a bar", "I just drank alcohol", "I am with a group of people",
                "I am working", "I am drinking coffee/tea", "I just woke up/got out of bed", "I am stressed", "I have a break right now",
                "I am doing nothing/bored", "I am with a group of people", "I want to relax", "I am hungry", "I just finished eating"}; // 13 options

        SKColor[] colors = new SKColor[]{new SKColor(214, 143, 14), new SKColor(214, 84, 14), new SKColor(144, 214, 14), new SKColor(14, 98, 214),
                                     new SKColor(255, 95, 95), new SKColor(214, 14, 175), new SKColor(206, 214, 14), new SKColor(94, 14, 214),
                                     new SKColor(14, 214, 116), new SKColor(197, 209, 199),
                                     new SKColor(55, 142, 191), new SKColor(184, 160, 231), new SKColor(95, 198, 255)};
        public Cravings()
        {
            InitializeComponent();
            if (!Preferences.ContainsKey("CravingsCreated")) { 
                Preferences.Set("CravingsCreated", true);

                int i = 0;
                foreach (string craving in cravingsString)
                {
                    CravingsClass newCraving = new CravingsClass
                    {
                        DisplayName = craving,
                        Hex = "#" + string.Format("{0:X2}{1:X2}{2:X2}", colors[i].Red, colors[i].Green, colors[i].Blue),
                        Count = 0
                    };
                    DB.conn2.Insert(newCraving);
                i++;
                }
            }
            ResetListViewSources();
        }

        protected override void OnAppearing()
        {
            ResetListViewSources();
        }

        public void ResetListViewSources()
        {
            List<CravingsClass> cravings = DB.conn2.Table<CravingsClass>().ToList();

            chartList = new List<ChartData>();
            for (int i = 0; i < cravings.Count; i++)
            {
                ChartData chartData = new ChartData(cravings[i].Count, colors[i]);
                if (cravings[i].Count != 0)
                    chartList.Add(chartData);
            }

            cravings = cravings.OrderBy(i => i.Count).Reverse().ToList();
            cravingsListView.ItemsSource = cravings;

        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            ChartData[] chartData = chartList.ToArray();

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            int totalValues = 0;

            foreach (ChartData item in chartData)
            {
                totalValues += item.Value;
            }

            SKPoint center = new SKPoint(info.Width / 2, info.Height / 2);
            float radius = Math.Min(info.Width / 2, info.Height / 2) - 2;
            SKRect rect = new SKRect(center.X - radius, center.Y - radius,
                                     center.X + radius, center.Y + radius);

            float startAngle = 0;

            if (chartData.Length == 1)
            {
                SKPaint singlePaint = new SKPaint
                {
                    Color = chartData[0].Color,
                    Style = SKPaintStyle.Fill
                };
                canvas.DrawCircle(center.X, center.Y, radius, singlePaint);
            }

            foreach (ChartData item in chartData)
            {
                float sweepAngle = 360f * item.Value / totalValues;

                using (SKPath path = new SKPath())
                using (SKPaint fillPaint = new SKPaint())
                {
                    path.MoveTo(center);
                    path.ArcTo(rect, startAngle, sweepAngle, false);
                    path.Close();

                    fillPaint.Style = SKPaintStyle.Fill;
                    fillPaint.Color = item.Color;

                    canvas.Save();

                    canvas.DrawPath(path, fillPaint);
                    canvas.Restore();

                }

                startAngle += sweepAngle;
            }

            SKPaint outlinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 5,
                Color = SKColors.Black
            };

            outlinePaint.Style = SKPaintStyle.Stroke;
            outlinePaint.StrokeWidth = 7;
            outlinePaint.Color = SKColors.Black;

            canvas.DrawCircle(center.X, center.Y, radius, outlinePaint);
        }

        class ChartData
        {
            public ChartData(int value, SKColor color)
            {
                Value = value;
                Color = color;
            }

            public int Value { private set; get; }

            public SKColor Color { private set; get; }
        }

        void cravingsListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            CravingsClass cravingTapped = (CravingsClass)((ListView)sender).SelectedItem;
            Increment(cravingTapped);
        }

        async void Increment(CravingsClass craving)
        {
            string action = await DisplayActionSheet("Change Value", "Cancel", null, "Increase By 1", "Decrease By 1", "Reset Value");
            if (action == "Increase By 1")
                craving.Count++;
            if (action == "Decrease By 1" && craving.Count > 0)
                craving.Count--;
            if (action == "Reset Value")
                craving.Count = 0;
            DB.conn2.Update(craving);
            pieChart.InvalidateSurface();
            ResetListViewSources();

        }
    }
}
