using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbyUI : MonoBehaviour
{
    public ArenaSettings arenaSettings;

    public async void ReadArenaSettings()
    {
        string? text1 = await GoogleSheetClient.Instance.GetCellData("B1", 2000);
        string? text2 = await GoogleSheetClient.Instance.GetCellData("C1", 2000);
        string? text3 = await GoogleSheetClient.Instance.GetCellData("D1", 2000);

        string? value1 = await GoogleSheetClient.Instance.GetCellData("B2", 2000);
        string? value2 = await GoogleSheetClient.Instance.GetCellData("C2", 2000);
        string? value3 = await GoogleSheetClient.Instance.GetCellData("D2", 2000);

        Debug.Log($"{text1} = {value1}");
        Debug.Log($"{text2} = {value2}");
        Debug.Log($"{text3} = {value3}");
    }

    public async void WriteArenaSettings()
    {
        string? value1 = await GoogleSheetClient.Instance.GetCellData("B3", 2000);
        string? value2 = await GoogleSheetClient.Instance.GetCellData("C3", 2000);
        string? value3 = await GoogleSheetClient.Instance.GetCellData("D3", 2000);

        string[] data = { value1, value2, value3 };
        float.TryParse(value1, out arenaSettings.PlayerPlatformRatio);
        int.TryParse(value2, out arenaSettings.PlatformDurability);
        int.TryParse(value1, out arenaSettings.CanonPlayerRatio);

        await GoogleSheetClient.Instance.WriteLineData("B2:D2", 2000, data);

    }
}
