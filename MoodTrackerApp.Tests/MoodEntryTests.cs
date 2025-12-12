using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoodTrackerApp.Models;
using System;

namespace MoodTrackerApp.Tests
{
    /// <summary>
    /// Класс для тестирования функциональности класса MoodEntry.
    /// Содержит модульные тесты для проверки корректности работы
    /// основных методов и свойств модели данных.
    /// </summary>
    [TestClass]
    public class MoodEntryTests
    {
        /// <summary>
        /// Проверяет, что при уровне настроения 1 свойство MoodDescription
        /// возвращает строковое значение "Ужасное".
        /// </summary>
        [TestMethod]
        public void MoodDescription_Level1_ReturnsУжасное()
        {
            var entry = new MoodEntry { MoodLevel = 1 };
            var description = entry.MoodDescription;
            Assert.AreEqual("Ужасное", description);
        }

        /// <summary>
        /// Проверяет, что при уровне настроения 5 свойство MoodDescription
        /// возвращает строковое значение "Отличное".
        /// </summary>
        [TestMethod]
        public void MoodDescription_Level5_ReturnsОтличное()
        {
            var entry = new MoodEntry { MoodLevel = 5 };
            var description = entry.MoodDescription;
            Assert.AreEqual("Отличное", description);
        }

        /// <summary>
        /// Проверяет, что при уровне настроения 3 свойство MoodDescription
        /// возвращает строковое значение "Нейтральное".
        /// </summary>
        [TestMethod]
        public void MoodDescription_Level3_ReturnsНейтральное()
        {
            // Arrange
            var entry = new MoodEntry { MoodLevel = 3 };

            // Act
            var description = entry.MoodDescription;

            // Assert
            Assert.AreEqual("Нейтральное", description);
        }

        /// <summary>
        /// Проверяет обработку некорректного значения уровня настроения.
        /// Ожидается, что при значении, выходящем за допустимый диапазон (1-5),
        /// свойство MoodDescription вернет "Неизвестно".
        /// </summary>
        [TestMethod]
        public void MoodDescription_InvalidLevel_ReturnsНеизвестно()
        {
            var entry = new MoodEntry { MoodLevel = 999 };
            var description = entry.MoodDescription;
            Assert.AreEqual("Неизвестно", description);
        }

        /// <summary>
        /// Проверяет значения свойств по умолчанию при создании нового экземпляра MoodEntry.
        /// Убеждается, что свойства инициализируются ожидаемыми значениями.
        /// </summary>
        [TestMethod]
        public void MoodEntry_DefaultValues_AreSet()
        {
            var entry = new MoodEntry();

            Assert.AreEqual(0, entry.Id, "Id должен быть 0 по умолчанию");
            Assert.AreEqual(0, entry.MoodLevel, "MoodLevel должен быть 0 по умолчанию");

            Assert.AreEqual(default(DateTime), entry.CreatedAt,
                "CreatedAt должен иметь значение по умолчанию для DateTime");
            Assert.AreEqual(default(DateTime), entry.EntryDate,
                "EntryDate должен иметь значение по умолчанию для DateTime");
        }

        /// <summary>
        /// Проверяет, что при уровне настроения 2 свойство MoodDescription
        /// возвращает строковое значение "Плохое".
        /// </summary>
        [TestMethod]
        public void MoodDescription_Level2_ReturnsПлохое()
        {
            var entry = new MoodEntry { MoodLevel = 2 };
            var description = entry.MoodDescription;
            Assert.AreEqual("Плохое", description);
        }

        /// <summary>
        /// Проверяет, что при уровне настроения 4 свойство MoodDescription
        /// возвращает строковое значение "Хорошее".
        /// </summary>
        [TestMethod]
        public void MoodDescription_Level4_ReturnsХорошее()
        {
            var entry = new MoodEntry { MoodLevel = 4 };
            var description = entry.MoodDescription;
            Assert.AreEqual("Хорошее", description);
        }

        /// <summary>
        /// Комплексный тест, проверяющий все допустимые значения уровня настроения (1-5).
        /// Убеждается, что каждый уровень корректно преобразуется в соответствующее
        /// текстовое описание.
        /// </summary>
        [TestMethod]
        public void MoodDescription_AllLevelsCovered()
        {
            var testCases = new[]
            {
                (Level: 1, Expected: "Ужасное"),
                (Level: 2, Expected: "Плохое"),
                (Level: 3, Expected: "Нейтральное"),
                (Level: 4, Expected: "Хорошее"),
                (Level: 5, Expected: "Отличное"),
            };

            foreach (var testCase in testCases)
            {
                var entry = new MoodEntry { MoodLevel = testCase.Level };
                var actualDescription = entry.MoodDescription;
                Assert.AreEqual(testCase.Expected, actualDescription,
                    $"Уровень настроения {testCase.Level} должен возвращать '{testCase.Expected}'");
            }
        }
    }
}