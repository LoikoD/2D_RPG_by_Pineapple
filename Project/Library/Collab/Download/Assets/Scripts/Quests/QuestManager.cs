using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {

	public QuestObject[] quests;

	public bool[] completedQuests;

	public string itemCollected;

	public string enemyKilled;

    

	// Use this for initialization
	void Start () {
	
		completedQuests = new bool[quests.Length];

    }
	
	// Update is called once per frame
	void Update () {

	}

	public void showQuestText(string questText)
	{
        
		/*theDM.dialogueLines = new string[1];
		theDM.dialogueLines [0] = questText;
                                                                           TO DO
		theDM.currentLine = 0;
		theDM.ShowDialogue ();*/
		
	}

    public List<int> EnemiesCountList()
    {
        List<int> enemiesCountList = new List<int>();
        for (int i = 0; i < quests.Length; ++i)
        {
            enemiesCountList.Add(quests[i].enemyKillCount);
        }
        return enemiesCountList;
    }

    public void SetEnemiesKillCount(List<int> enemiesCountList)
    {
        for (int i = 0; i < quests.Length; ++i)
        {
            quests[i].enemyKillCount = enemiesCountList[i];
        }
    }

    public List<bool> GetListOfActiveQuests()
    {
        List<bool> activeQuests = new List<bool>();
        for (int i = 0; i < quests.Length; ++i)
        {
            if (quests[i].gameObject.activeSelf)
            {
                activeQuests.Add(true);
            } else
            {
                activeQuests.Add(false);
            }
        }
        return activeQuests;
    }

    public void ActivateQuestsFromList(List<bool> activeQuests)
    {
        for (int i = 0; i < quests.Length; ++i)
        {
            quests[i].gameObject.SetActive(activeQuests[i]);
        }
    }
}
