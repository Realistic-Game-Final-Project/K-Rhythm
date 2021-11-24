using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;

    [System.Serializable]
    public class Player
    {
        public int health;
        public float mana;
    }
    [System.Serializable]
    public class PlayerList
    {
        public Player[] player;
    }

    public PlayerList myPlayerList = new PlayerList();

    void Awake()
    {
        myPlayerList = JsonUtility.FromJson<PlayerList>(textJSON.text);        
    }
}
