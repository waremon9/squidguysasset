using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SaveSystem : MySingleton<SaveSystem>
{
    [SerializeField] private string savePath;
    [SerializeField] private string saveFileName;
    [SerializeField] private string saveFileExtension;
    [SerializeField] private DataLoadedEvent dataLoadedEvent;

    private GameData gData;

    public override bool DoDestroyOnLoad { get; }

    public async void LoadData()
    {
        gData = await GetDataFromDisk();
        dataLoadedEvent.Raise(gData);
        Debug.Log("game data loaded");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
    }

    public async void  SaveData()
    {
        GetData();

        await WriteData(gData);
    }

    private void GetData()
    {
        List<GameObject> gameObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("ToSave"));

        List<PlayerData> playerDatas = new List<PlayerData>();
        List<PlatformData> platformDatas = new List<PlatformData>();
        List<CannonData> cannonDatas = new List<CannonData>();

        foreach (var gameObject in gameObjects)
        {
            if (gameObject.GetComponent<Player>() != null)
            {
                Player playerTemp = gameObject.GetComponent<Player>();
                if(!playerTemp.IsDead)
                    playerDatas.Add(playerTemp.GetPlayerData());
            }
            else if (gameObject.GetComponent<Platform>() != null)
            {
                platformDatas.Add(gameObject.GetComponent<Platform>().GetPlatformData());
            }
            else if (gameObject.GetComponent<PushCanon>() != null)
            {
                cannonDatas.Add(gameObject.GetComponent<PushCanon>().GetCannonData());
            }
        }


        GameData gameData = new GameData( 
            GameManager.Instance.GetMaxPlayerAmount(), 
            playerDatas, 
            platformDatas, 
            ArenaManager.Instance.Dimension,
            ArenaManager.Instance.ArenaSettings.PlatformDurability,
            cannonDatas);

        gData = gameData;
    }

    private async Task WriteData(GameData data)
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, savePath); 
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        string filePath = Path.Combine(directoryPath, saveFileName + saveFileExtension);

        string jsonData = JsonUtility.ToJson(data, true);
        byte[] bytes = Encoding.Unicode.GetBytes(jsonData);

        Debug.Log(filePath);

        using (FileStream filestream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
        {
            await filestream.WriteAsync(bytes,0,bytes.Length);
        }
    }

    private async Task<GameData> GetDataFromDisk()
    {

        string directoryPath = Path.Combine(Application.persistentDataPath, savePath);
        string filePath = Path.Combine(directoryPath, saveFileName + saveFileExtension);

        if (!Directory.Exists(directoryPath) || !File.Exists(filePath))
        {
            throw new Exception("The save file doesn't exist");
        }

        string data = "";

        using (FileStream streamReader = new FileStream(filePath, FileMode.Open,FileAccess.Read,FileShare.Read))
        {
            byte[] bytes = new byte[streamReader.Length];
            await streamReader.ReadAsync(bytes, 0, bytes.Length);

            data = Encoding.Unicode.GetString(bytes);
        }
            Debug.Log(data);
        
        GameData gameData = JsonUtility.FromJson<GameData>(data);

        return gameData;
    }
}
