using MoodTrackerApp.Services;
using MoodTrackerApp.Utilities;
using MoodTrackerApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoodTrackerApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MoodService _moodService;
        private DateTime? _filterStartDate;
        private DateTime? _filterEndDate;
        public MainWindow()
        {
            InitializeComponent();
            _moodService = new MoodService();
            /// <summary>
            /// Устанавливаем фильтр по умолчанию (последние 30 дней)
            /// </summary>
            EndDatePicker.SelectedDate = DateTime.Today;
            StartDatePicker.SelectedDate = DateTime.Today.AddDays(-30);

            ApplyFilter();
        }
        private void LoadEntries()
        {
            var entries = _moodService.GetEntriesByDateRange(_filterStartDate, _filterEndDate);
            EntriesGrid.ItemsSource = entries;

            if (entries.Any())
            {
                var avgMood = entries.Average(e => e.MoodLevel);
                var filterInfo = _filterStartDate.HasValue ?
                    $" (фильтр: {_filterStartDate.Value:dd.MM.yyyy} - {_filterEndDate.Value:dd.MM.yyyy})" : "";

                StatsText.Text = $"Всего записей: {entries.Count}{filterInfo}\nСреднее настроение: {avgMood:F1}";
            }
            else
            {
                StatsText.Text = "Записей пока нет";
            }
        }
        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }
        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
            ApplyFilter();
        }
        private void ApplyFilter()
        {
            _filterStartDate = StartDatePicker.SelectedDate;
            _filterEndDate = EndDatePicker.SelectedDate;

            if (_filterEndDate.HasValue && !_filterStartDate.HasValue)
            {
                _filterStartDate = _filterEndDate.Value.AddDays(-30);
                StartDatePicker.SelectedDate = _filterStartDate;
            }
            if (_filterStartDate.HasValue && !_filterEndDate.HasValue)
            {
                _filterEndDate = DateTime.Today;
                EndDatePicker.SelectedDate = _filterEndDate;
            }

            LoadEntries();
        }
        private void AddEntry_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEntryWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadEntries();
                StatusText.Text = "Запись добавлена";
            }
        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadEntries();
            StatusText.Text = "Данные обновлены";
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            var statsWindow = new StatisticsWindow(_moodService, _filterStartDate, _filterEndDate);
            statsWindow.ShowDialog();
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            var entries = _moodService.GetEntriesByDateRange(_filterStartDate, _filterEndDate);
            var exporter = new CsvExporter();

            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = $"mood_entries_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (saveDialog.ShowDialog() == true)
            {
                if (exporter.ExportToCsv(entries, saveDialog.FileName))
                {
                    StatusText.Text = $"Данные экспортированы в {saveDialog.FileName}";
                    MessageBox.Show("Экспорт завершен успешно!", "Экспорт данных",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Ошибка при экспорте данных!", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
