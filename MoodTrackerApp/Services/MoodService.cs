using MoodTrackerApp.Data;
using MoodTrackerApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTrackerApp.Services
{
    public class MoodService
    {
        private DatabaseHelper _dbHelper;

        public MoodService()
        {
            _dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// Добавляет новую запись о настроении
        /// </summary>
        /// <param name="entry">Объект MoodEntry для добавления</param>
        /// <returns>True если успешно, иначе False</returns>
        public bool AddMoodEntry(MoodEntry entry)
        {
            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO MoodEntries (EntryDate, MoodLevel, Notes) 
                                   VALUES (@EntryDate, @MoodLevel, @Notes)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EntryDate", entry.EntryDate);
                        command.Parameters.AddWithValue("@MoodLevel", entry.MoodLevel);
                        command.Parameters.AddWithValue("@Notes", entry.Notes ?? "");

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Получает все записи о настроении
        /// </summary>
        /// <returns>Список записей MoodEntry</returns>
        public List<MoodEntry> GetAllEntries()
        {
            var entries = new List<MoodEntry>();

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM MoodEntries ORDER BY EntryDate DESC";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            entries.Add(new MoodEntry
                            {
                                Id = reader.GetInt32(0),
                                EntryDate = reader.GetDateTime(1),
                                MoodLevel = reader.GetInt32(2),
                                Notes = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                CreatedAt = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return entries;
        }
        /// <summary>
        /// Получает записи за указанный период
        /// </summary>
        public List<MoodEntry> GetEntriesByDateRange(DateTime? startDate, DateTime? endDate)
        {
            var entries = new List<MoodEntry>();

            try
            {
                using (var connection = _dbHelper.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT * FROM MoodEntries WHERE 1=1";
                    var parameters = new List<SqlParameter>();

                    if (startDate.HasValue)
                    {
                        query += " AND EntryDate >= @StartDate";
                        parameters.Add(new SqlParameter("@StartDate", startDate.Value));
                    }

                    if (endDate.HasValue)
                    {
                        query += " AND EntryDate <= @EndDate";
                        parameters.Add(new SqlParameter("@EndDate", endDate.Value.AddDays(1))); // Включить весь конечный день
                    }

                    query += " ORDER BY EntryDate DESC";

                    using (var command = new SqlCommand(query, connection))
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.Add(param);
                        }

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                entries.Add(new MoodEntry
                                {
                                    Id = reader.GetInt32(0),
                                    EntryDate = reader.GetDateTime(1),
                                    MoodLevel = reader.GetInt32(2),
                                    Notes = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                    CreatedAt = reader.GetDateTime(4)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при получении записей: {ex.Message}");
            }

            return entries;
        }
    }
}