using MoodTrackerApp.Models;
using MoodTrackerApp.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MoodTrackerApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AddEntryWindow.xaml
    /// </summary>
    public partial class AddEntryWindow : Window
    {
        private MoodService _moodService;
        public AddEntryWindow()
        {
            InitializeComponent();
            _moodService = new MoodService();
            /// <summary>
            /// Инициализирует элементы управления датой и временем
            /// Устанавливает текущую дату и заполняет ComboBox для часов и минут
            /// </summary>
            InitializeDateTime();
            /// <summary>
            /// Нейтральное по умолчанию настроение
            /// </summary>
            MoodComboBox.SelectedIndex = 2;
        }
        /// <summary>
        /// Установка текущей даты и времени
        /// </summary>
        private void InitializeDateTime()
        {
            var now = DateTime.Now;
            DataPicker.SelectedDate = now;

            for (int i = 0; i < 24; i++)
            {
                HourComoboBox.Items.Add(i.ToString("00"));
            }
            for (int i = 0; i < 60; i++)
            {
                MinuteComboBox.Items.Add(i.ToString("00"));
            }
            HourComoboBox.SelectedItem = now.Hour.ToString("00");
            MinuteComboBox.SelectedItem = now.Minute.ToString("00");
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataPicker.SelectedDate == null ||
                MoodComboBox.SelectedItem == null ||
                HourComoboBox.SelectedItem == null ||
                MinuteComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }
            try
            {
                var selectedDate = DataPicker.SelectedDate.Value;
                int hour = int.Parse(HourComoboBox.SelectedItem.ToString());
                int minute = int.Parse(MinuteComboBox.SelectedItem.ToString());

                var entryDateTime = new DateTime(
                    selectedDate.Year,
                    selectedDate.Month,
                    selectedDate.Day,
                    hour, minute, 0);

                var entry = new MoodEntry
                {
                    EntryDate = entryDateTime,
                    MoodLevel = int.Parse((MoodComboBox.SelectedItem as ComboBoxItem).Tag.ToString()),
                    Notes = NotesTextBox.Text
                };

                if (_moodService.AddMoodEntry(entry))
                {
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Ошибка при сохранении");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
