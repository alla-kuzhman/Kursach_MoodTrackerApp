using MoodTrackerApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTrackerApp.Utilities
{
    public class CsvExporter
    {
        public bool ExportToCsv(List<MoodEntry> entries, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("Дата;Уровень настроения;Настроение;Заметки");

                    foreach (var entry in entries)
                    {
                        var notes = entry.Notes?.Replace(";", ",") ?? ""; 
                        writer.WriteLine($"\"{entry.EntryDate:dd.MM.yyyy HH:mm}\";{entry.MoodLevel};{entry.MoodDescription};\"{notes}\"");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при экспорте: {ex.Message}");
                return false;
            }
        }
    }
}