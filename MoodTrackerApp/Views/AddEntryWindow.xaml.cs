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
using System.Windows.Shapes;
using MoodTrackerApp.Models;
using MoodTrackerApp.Services;

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
            DatePicker.SelectedDate = now;

            for (int i = 0; i < 24; i++)
            {
                HourComboBox.Items.Add(i.ToString("00"));
            }
            for (int i = 0; i < 60; i++)
            {
                MinuteComboBox.Items.Add(i.ToString("00"));
            }
            HourComboBox.SelectedItem = now.Hour.ToString("00");
            MinuteComboBox.SelectedItem = now.Minute.ToString("00");
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DatePicker.SelectedDate == null ||
                MoodComboBox.SelectedItem == null ||
                HourComboBox.SelectedItem == null ||
                MinuteComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }
            try
            {
                var selectedDate = DatePicker.SelectedDate.Value;
                int hour = int.Parse(HourComboBox.SelectedItem.ToString());
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
