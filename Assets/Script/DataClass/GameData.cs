using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct GameData
{
    public int playerAmount;
    public int arenaDimension;
    public int maxDurabilityPlatforms;
    public List<CannonData> cannonsData;
    public List<PlayerData> playerDatas;
    public List<PlatformData> platformDatas;

    public GameData(int pAmount, List<PlayerData> playerData, List<PlatformData> platformData, int arenaDim, int maxDura, List<CannonData> cannonDatas)
    {
        playerAmount = pAmount;
        playerDatas = playerData;
        platformDatas = platformData;
        arenaDimension = arenaDim;
        maxDurabilityPlatforms = maxDura;
        cannonsData = cannonDatas ;
    }
}
