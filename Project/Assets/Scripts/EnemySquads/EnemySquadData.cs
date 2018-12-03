﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquadData : MonoBehaviour {

    public string squadName;
    public string mapName;
    public int maxAmountEnemies = 3;
    public string BattleScene;
    public int orderInList = -1;
    public List<GameObject> enemies = new List<GameObject>();
    public int xpGain;

    private void Start()
    {
        xpGain = 0;
        foreach (GameObject enemy in enemies)
        {
            xpGain += enemy.GetComponent<EnemyStateMachine>().enemy.xpGain;
        }
    }
}
