using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MoodTrackerApp.States
{
    /// <summary>
    /// Класс состояния "Просмотр записей".
    /// Реализует режим, в котором пользователь может просматривать существующие записи о настроении.
    /// </summary>
    public class ViewState : AppState
    {
        /// <summary>
        /// Обновляет интерфейс главного окна для режима просмотра записей.
        /// </summary>
        /// <param name="window">Главное окно приложения, требующее обновления.</param>
        public override void Handle(MainWindow window)
        {
            window.UpdateAddButtonState(false);
            window.AddButton.IsEnabled = false;

            window.StatusText.Text = "Режим просмотра записей";
            window.DeleteButton.Visibility = Visibility.Collapsed;
        }
    }
}
