using UnityEngine;
using Mono.Data.Sqlite;

public class Leaderboard : MonoBehaviour{
    private string dbName = "URI=file:Leaderboard.db";

    void Start(){
    }

    public void createDB(){
        using (var connection = new SqliteConnection(dbName)){
            connection.Open();

            using (var command = connection.CreateCommand()){
                command.CommandText = "CREATE TABLE IF NOT EXISTS leaderboard (name VARCHAR(50), killCount INT, floorCount INT, timeCount TIMESTAMP, healthValue FLOAT, armorValue INT, damageValue FLOAT, helmEquipped VARCHAR(50), chestEquipped VARCHAR(50), legEquipped VARCHAR(50), weaponEquipped VARCHAR(50))";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
    public void addScore(string playerName, int killScore, int floorScore, int timeScore, float healthScore, int armorScore, float damageScore, string equippedHelm, string equippedChest, string equippedLeg, string equippedWeapon){
        using (var connection = new SqliteConnection(dbName)){
            connection.Open();

            using (var command = connection.CreateCommand()){
                command.CommandText = "INSERT INTO leaderboard (name, killCount, floorCount, timeCount, healthValue, armorValue, damageValue, helmEquipped, chestEquipped, legEquipped, weaponEquipped) VALUES ('" + playerName + "', '" + killScore + "', '" + floorScore + "', '" + timeScore + "', '" + healthScore + "', '" + armorScore + "', '" + damageScore + "', '" + equippedHelm + "', '" + equippedChest + "', '" + equippedLeg + "', '" + equippedWeapon + "')";
                command.ExecuteNonQuery();
            }
        connection.Close();
        }
    }
}
