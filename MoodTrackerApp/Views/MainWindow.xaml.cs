using MoodTrackerApp.Models;
using MoodTrackerApp.Services;
using MoodTrackerApp.States;
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
        private AppState _currentState;

        /// <summary>
        /// Конструктор главного окна приложения
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _moodService = new MoodService();

            // Устанавливаем фильтр по умолчанию (последние 30 дней)
            EndDatePicker.SelectedDate = DateTime.Today;
            StartDatePicker.SelectedDate = DateTime.Today.AddDays(-30);

            // Устанавливаем начальное состояние - просмотр
            ChangeState(new ViewState());
            ApplyFilter();
        }
        public void UpdateAddButtonState(bool isEnabled)
        {
            AddButton.IsEnabled = isEnabled;
            AddMenuItem.IsEnabled = isEnabled;  // ← ПРОСТО!
        }

        /// <summary>
        /// Загружает записи о настроении из базы данных
        /// </summary>
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

        /// <summary>
        /// Изменяет текущее состояние приложения
        /// </summary>
        /// <param name="newState">Новое состояние для установки</param>
        public void ChangeState(AppState newState)
        {
            _currentState = newState;
            _currentState.Handle(this);  // Обновляем интерфейс

            // Визуально выделяем активную кнопку
            UpdateButtonColors();
        }

        /// <summary>
        /// Обновляет цвета кнопок в зависимости от текущего состояния
        /// </summary>
        private void UpdateButtonColors()
        {
            // Проверяем, существуют ли кнопки (защита от NullReferenceException)
            if (ViewModeButton != null && EditModeButton != null)
            {
                // Сбрасываем цвета кнопок
                ViewModeButton.Background = Brushes.LightGray;
                EditModeButton.Background = Brushes.LightGray;

                // Подсвечиваем активную кнопку
                if (_currentState is ViewState)
                    ViewModeButton.Background = Brushes.LightGreen;
                else if (_currentState is EditState)
                    EditModeButton.Background = Brushes.LightYellow;
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Режим просмотра"
        /// </summary>
        private void ViewModeButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeState(new ViewState());
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Режим редактирования"
        /// </summary>
        private void EditModeButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeState(new EditState());
        }

        /// <summary>
        /// Применяет фильтр по датам к списку записей
        /// </summary>
        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        /// <summary>
        /// Сбрасывает фильтр по датам
        /// </summary>
        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
            ApplyFilter();
        }

        /// <summary>
        /// Применяет текущие настройки фильтрации
        /// </summary>
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

        /// <summary>
        /// Обработчик добавления новой записи
        /// </summary>
        private void AddEntry_Click(object sender, RoutedEventArgs e)
        {
            var previousState = _currentState;

            ChangeState(new EditState());

            var addWindow = new AddEntryWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadEntries();
                ChangeState(new ViewState());
                StatusText.Text = "Запись добавлена";
            }
            else
            {
                ChangeState(previousState);
                StatusText.Text = "Добавление отменено";
            }
        }

        /// <summary>
        /// Обновляет список записей
        /// </summary>
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadEntries();
            StatusText.Text = "Данные обновлены";
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Статистика"
        /// </summary>
        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            var statsWindow = new StatisticsWindow(_moodService, _filterStartDate, _filterEndDate);
            statsWindow.ShowDialog();

            // После закрытия окна статистики возвращаемся в режим просмотра
            ChangeState(new ViewState());
        }

        /// <summary>
        /// Экспортирует данные в CSV файл
        /// </summary>
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

        /// <summary>
        /// Открывает окно "О программе"
        /// </summary>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        /// <summary>
        /// Закрывает приложение
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Старые методы (можно удалить или оставить как запасные)
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeState(new EditState());
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeState(new ViewState());
        }

        /// <summary>
        /// Сохранение изменений
        /// </summary>
        private void SaveEditButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeState(new ViewState());
            SaveEditButton.Visibility = Visibility.Collapsed;
            CancelEditButton.Visibility = Visibility.Collapsed;

            StatusText.Text = "Изменения сохранены";
        }

        /// <summary>
        /// Отмена редактирования
        /// </summary>
        private void CancelEditButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeState(new ViewState());
            SaveEditButton.Visibility = Visibility.Collapsed;
            CancelEditButton.Visibility = Visibility.Collapsed;

            StatusText.Text = "Редактирование отменено";
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (EntriesGrid.SelectedItem is MoodEntry selectedEntry)
            {
                var result = MessageBox.Show($"Удалить запись от {selectedEntry.EntryDate:dd.MM.yyyy}?",
                                            "Подтверждение удаления",
                                            MessageBoxButton.YesNo,
                                            MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (_moodService.DeleteMoodEntry(selectedEntry.Id))
                    {
                        LoadEntries();
                        StatusText.Text = "Запись удалена";
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления!");
            }

        }
        
    }
}