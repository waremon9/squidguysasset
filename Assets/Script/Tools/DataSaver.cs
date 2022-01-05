using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataSaver : MySingleton<DataSaver>
{
    public override bool DoDestroyOnLoad { get; }

    public void WriteData()
    {

    }

    public void SaveData(List<PlayerData> playersData, List<PlatformData> platformsData, GameData gameData)
    {
        string playerDataJson = "";
        string platformDataJson = "";
        string gameDataJson = "";
        foreach (var playerData in playersData)
        {
            playerDataJson += JsonUtility.ToJson(playerData);
        }

        foreach (var platformData in platformsData)
        {
            platformDataJson += JsonUtility.ToJson(platformData);
        }

        gameDataJson = JsonUtility.ToJson(gameData);


        File.WriteAllText(Application.persistentDataPath + "/PlayersData.json", playerDataJson);
        File.WriteAllText(Application.persistentDataPath + "/PlatformData.json", platformDataJson);
        File.WriteAllText(Application.persistentDataPath + "/GameData.json", gameDataJson);
        Debug.Log(Application.persistentDataPath);
    }
}
