using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MoodTrackerApp.States
{
    /// <summary>
    /// Класс состояния "Редактирование записей".
    /// Реализует режим, в котором пользователь может редактировать существующие записи.
    /// </summary>
    public class EditState : AppState
    {
        /// <summary>
        /// Обновляет интерфейс главного окна для режима редактирования записей.
        /// </summary>
        /// <param name="window">Главное окно приложения, требующее обновления.</param>
        public override void Handle(MainWindow window)
        {

            window.AddButton.IsEnabled = true;
            window.UpdateAddButtonState(true);
            window.StatusText.Text = "Режим редактирования записи";
            window.DeleteButton.Visibility = Visibility.Visible;
        }
    }
}
