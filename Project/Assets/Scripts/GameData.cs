using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData {
    
    //public string scene;
    public string mapName;

    public float heroPositionX;
    public float heroPositionY;

    public List<bool> heroSquad = new List<bool>();
    public List<int> defeatedSquads = new List<int>();
    public List<int> joinedHeroNpcs = new List<int>();
    public bool[] completedQuests;
    public List<bool> activeQuests;
    public List<int> enemiesKillCountList = new List<int>();
    public List<bool> foundItems = new List<bool>();
    public List<int> npcs = new List<int>();
    public List<int> attributes = new List<int>();
    public int potionQuantity;
    public int unusedAttributesPoints;
    public int currentXp;
    public int maxXp;
    public int potentialXp;
    public List<string> keyNames = new List<string>();
    public List<KeyCode> keyCodes = new List<KeyCode>();

    public GameData()
    {
        //scene = "WorldScene";
        mapName = "Beach";
        heroPositionX = 25.3f;
        heroPositionY = -3.3f;
    }
    
    public void NewGame()
    {
        mapName = "Beach";
        heroPositionX = 25.3f;
        heroPositionY = -3.3f;

        heroSquad.Clear();
        heroSquad.Add(false);
        heroSquad.Add(true);
        heroSquad.Add(false);

        defeatedSquads.Clear();
        joinedHeroNpcs.Clear();
        Array.Clear(completedQuests, 0, completedQuests.Length);

        activeQuests.Clear();
        for (int i = 0; i < completedQuests.Length; ++i)
        {
            activeQuests.Add(false);
        }

        enemiesKillCountList.Clear();
        for (int i = 0; i < completedQuests.Length; ++i)
        {
            enemiesKillCountList.Add(0);
        }

        foundItems.Clear();
        for (int i = 0; i < 10; ++i)
        {
            foundItems.Add(false);
        }

        npcs.Clear();
        for (int i = 0; i < 10; ++i)
        {
            npcs.Add(0);
        }

        attributes.Clear();
        for (int i = 0; i < 4; ++i)
        {
            attributes.Add(1);
        }

        potionQuantity = 3;
        unusedAttributesPoints = 5;
        currentXp = 0;
        maxXp = 100;
        potentialXp = 0;

        keyNames.Add("UP");
        keyCodes.Add(KeyCode.W);
        keyNames.Add("DOWN");
        keyCodes.Add(KeyCode.S);
        keyNames.Add("LEFT");
        keyCodes.Add(KeyCode.A);
        keyNames.Add("RIGHT");
        keyCodes.Add(KeyCode.D);
        keyNames.Add("AttributesWindow");
        keyCodes.Add(KeyCode.Tab);
        keyNames.Add("QuestsWindowButton");
        keyCodes.Add(KeyCode.J);
        keyNames.Add("Submit");
        keyCodes.Add(KeyCode.Return);
        keyNames.Add("Cancel");
        keyCodes.Add(KeyCode.Escape);


    }
}
