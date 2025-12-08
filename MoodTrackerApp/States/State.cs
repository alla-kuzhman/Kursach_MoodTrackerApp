using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTrackerApp.States
{   
    /// <summary>
    /// Абстрактный базовый класс для всех состояний приложения.
    /// Реализует паттерн "Состояние" (State), позволяющий объекту изменять свое поведение
    /// при изменении внутреннего состояния.
    /// </summary>
    public abstract class AppState
    {
        /// <summary>
        /// Обрабатывает смену состояния, обновляя пользовательский интерфейс
        /// в соответствии с текущим состоянием.
        /// </summary>
        /// <param name="window">Главное окно приложения, элементы которого нужно обновить.</param>
        public abstract void Handle(MainWindow window);
    }
}
