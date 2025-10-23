using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;
    public Vector3 playerPosition;
    public Vector3 companionPosition;


    //values inside "gamedata" constructor will be default vaules for game 
    public GameData()
    {
        this.deathCount = 0;
        playerPosition = Vector3.zero;
        companionPosition = Vector3.zero;
    }
}
    
