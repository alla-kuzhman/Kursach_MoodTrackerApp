using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTrackerApp.Models
{
    public class MoodEntry
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public int MoodLevel { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public string MoodDescription
        {
            get
            {
                switch (MoodLevel)
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
}