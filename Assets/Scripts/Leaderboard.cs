using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.Collections.Generic;

public class Leaderboard : MonoBehaviour
{
    private string dbName = "URI=file:Leaderboard.db";

    // Data structure to hold leaderboard entries
    private List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

    // Leaderboard entry structure
    public class LeaderboardEntry
    {
        public string name;
        public int killCount;
        public int floorCount;
        public int timeCount;
        // Add other fields as needed
    }

    void Start()
    {
        createDB();
    }

    public void createDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS leaderboard (name VARCHAR(50), killCount INT, floorCount INT, timeCount INT, healthValue FLOAT, armorValue INT, damageValue FLOAT, helmEquipped VARCHAR(50), chestEquipped VARCHAR(50), legEquipped VARCHAR(50), weaponEquipped VARCHAR(50))";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public void addScore(string playerName, int killScore, int floorScore, int timeScore, float healthScore, float armorScore, float damageScore, string equippedHelm, string equippedChest, string equippedLeg, string equippedWeapon)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO leaderboard (name, killCount, floorCount, timeCount, healthValue, armorValue, damageValue, helmEquipped, chestEquipped, legEquipped, weaponEquipped) VALUES (@name, @killCount, @floorCount, @timeCount, @healthValue, @armorValue, @damageValue, @helmEquipped, @chestEquipped, @legEquipped, @weaponEquipped)";
                command.Parameters.AddWithValue("@name", playerName);
                command.Parameters.AddWithValue("@killCount", killScore);
                command.Parameters.AddWithValue("@floorCount", floorScore);
                command.Parameters.AddWithValue("@timeCount", timeScore);
                command.Parameters.AddWithValue("@healthValue", healthScore);
                command.Parameters.AddWithValue("@armorValue", armorScore);
                command.Parameters.AddWithValue("@damageValue", damageScore);
                command.Parameters.AddWithValue("@helmEquipped", equippedHelm);
                command.Parameters.AddWithValue("@chestEquipped", equippedChest);
                command.Parameters.AddWithValue("@legEquipped", equippedLeg);
                command.Parameters.AddWithValue("@weaponEquipped", equippedWeapon);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }

    public List<LeaderboardEntry> OrderedByName()
    {
        List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM leaderboard ORDER BY name";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LeaderboardEntry entry = new LeaderboardEntry();
                        entry.name = reader["name"].ToString();
                        entry.killCount = Convert.ToInt32(reader["killCount"]);
                        entry.floorCount = Convert.ToInt32(reader["floorCount"]);
                        entry.timeCount = Convert.ToInt32(reader["timeCount"]);
                        // Add other fields as needed
                        entries.Add(entry);
                    }
                }
            }
            connection.Close();
        }
        return entries;
    }

    public List<LeaderboardEntry> OrderedByFloor()
    {
        List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM leaderboard ORDER BY floorCount";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LeaderboardEntry entry = new LeaderboardEntry();
                        entry.name = reader["name"].ToString();
                        entry.killCount = Convert.ToInt32(reader["killCount"]);
                        entry.floorCount = Convert.ToInt32(reader["floorCount"]);
                        entry.timeCount = Convert.ToInt32(reader["timeCount"]);
                        entries.Add(entry);
                    }
                }
            }
            connection.Close();
        }
        return entries;
    }

    public List<LeaderboardEntry> OrderedByTime()
    {
        List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM leaderboard ORDER BY timeCount";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LeaderboardEntry entry = new LeaderboardEntry();
                        entry.name = reader["name"].ToString();
                        entry.killCount = Convert.ToInt32(reader["killCount"]);
                        entry.floorCount = Convert.ToInt32(reader["floorCount"]);
                        entry.timeCount = Convert.ToInt32(reader["timeCount"]);
                        entries.Add(entry);
                    }
                }
            }
            connection.Close();
        }
        return entries;
    }

    public List<LeaderboardEntry> OrderedByKillCount()
    {
        List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM leaderboard ORDER BY killCount";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LeaderboardEntry entry = new LeaderboardEntry();
                        entry.name = reader["name"].ToString();
                        entry.killCount = Convert.ToInt32(reader["killCount"]);
                        entry.floorCount = Convert.ToInt32(reader["floorCount"]);
                        entry.timeCount = Convert.ToInt32(reader["timeCount"]);
                        entries.Add(entry);
                    }
                }
            }
            connection.Close();
        }
        return entries;
    }
    public void printScores()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM leaderboard";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        Debug.Log("Name: " + reader["name"] + "\t Kill Count: " + reader["killCount"] + "\t Floor Count: " + reader["floorCount"] + "\t Run Time: " + reader["timeCount"] + "\t Max Health: " + reader["healthValue"] + "\t Max Armor: " + reader["armorValue"] + "\t Max Damage: " + reader["damageValue"] + "\t Equipped Helmet: " + reader["helmEquipped"] + "\t Equipped Chestplate: " + reader["chestEquipped"] + "\t Equipped Leggings: " + reader["legEquipped"] + "\t Equipped Weapon: " + reader["weaponEquipped"]);
                }
            }
            connection.Close();
        }
    }
    public void printScoresOrderedByName()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM leaderboard ORDER BY name";

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        Debug.Log("Name: " + reader["name"] + "\t Kill Count: " + reader["killCount"] + "\t Floor Count: " + reader["floorCount"] + "\t Run Time: " + reader["timeCount"] + "\t Max Health: " + reader["healthValue"] + "\t Max Armor: " + reader["armorValue"] + "\t Max Damage: " + reader["damageValue"] + "\t Equipped Helmet: " + reader["helmEquipped"] + "\t Equipped Chestplate: " + reader["chestEquipped"] + "\t Equipped Leggings: " + reader["legEquipped"] + "\t Equipped Weapon: " + reader["weaponEquipped"]);
                }
            }
            connection.Close();
        }
    }
}

