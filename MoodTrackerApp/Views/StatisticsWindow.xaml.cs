using MoodTrackerApp.Models;
using MoodTrackerApp.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MoodTrackerApp.Views
{
    /// <summary>
    /// Логика взаимодействия для StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public PlotModel MoodChart { get; private set; }
        public PlotModel MoodDistributionChart { get; private set; }
        public StatisticsWindow(MoodService moodService, DateTime? startDate, DateTime? endDate)
        {
            InitializeComponent();

            var entries = moodService.GetEntriesByDateRange(startDate, endDate);
            GenerateCharts(entries);
            DataContext = this;
        }
        private void GenerateCharts(List<MoodEntry> entries)
        {
            if (!entries.Any())
            {
                MoodChart = new PlotModel { Title = "Нет данных для отображения" };
                MoodDistributionChart = new PlotModel { Title = "Нет данных для отображения" };
                return;
            }

            MoodChart = new PlotModel { Title = "Динамика настроения" };

            var dateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Дата",
                StringFormat = "dd.MM.yy"
            };
            MoodChart.Axes.Add(dateAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Уровень настроения",
                Minimum = 0.5,
                Maximum = 5.5
            };
            MoodChart.Axes.Add(valueAxis);

            var lineSeries = new LineSeries
            {
                Title = "Настроение",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White,
                MarkerFill = OxyColors.Blue,
                Color = OxyColors.Blue
            };

            foreach (var entry in entries.OrderBy(e => e.EntryDate))
            {
                lineSeries.Points.Add(new DataPoint(
                    DateTimeAxis.ToDouble(entry.EntryDate),
                    entry.MoodLevel
                ));
            }

            MoodChart.Series.Add(lineSeries);

            MoodDistributionChart = new PlotModel { Title = "Распределение настроений" };

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            var barAxis = new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0 };

            MoodDistributionChart.Axes.Add(categoryAxis);
            MoodDistributionChart.Axes.Add(barAxis);

            var moodGroups = entries.GroupBy(e => e.MoodLevel)
                                   .OrderBy(g => g.Key)
                                   .Select(g => new { Mood = g.Key, Count = g.Count() });

            var barSeries = new BarSeries { FillColor = OxyColors.SteelBlue };

            int index = 0;
            foreach (var group in moodGroups)
            {
                barSeries.Items.Add(new BarItem { Value = group.Count });
                categoryAxis.Labels.Add(GetMoodDescription(group.Mood));
                index++;
            }

            MoodDistributionChart.Series.Add(barSeries);
        }

        private string GetMoodDescription(int moodLevel)
        {
            switch (moodLevel)
            {
                case 1:
                    return "Ужасное";
                case 2:
                    return "Плохое";
                case 3:
                    return "Нейтральное";
                case 4:
                    return "Хорошее";
                case 5:
                    return "Отличное";
                default:
                    return "Неизвестно";
            }
        }
    }
}