using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour {

    public int curQuestNum;

	public QuestManager theQM;

	public string startText;
	public string endText;

	public bool isItemQuest;
	public string targetItem;

	public bool isEnemyQuest;
	public string targetEnemy;

	public int enemiesToKill;
	public int enemyKillCount;

    public int npcID;
    public int npcDialogueIdAfterQuest;

    private DialogueWithTrigger dialogue;

	// Use this for initialization
	void Start () {
        dialogue = transform.GetComponent<DialogueWithTrigger>();

	}
	
	// Update is called once per frame
	void Update () {

		if (isItemQuest) {
			if (theQM.itemCollected == targetItem) {

				theQM.itemCollected = null;

				endQuest ();
			}
		}

		if (isEnemyQuest) {

			if (theQM.enemyKilled == targetEnemy) {

				theQM.enemyKilled = null;

				enemyKillCount++;
			}

			if (enemyKillCount >= enemiesToKill) {

				endQuest ();

			}

		}

	}

	public void startQuest()
    {
        gameObject.SetActive(true);

    }


	public void endQuest()
	{
        dialogue.allowed = true;
        Invoke("SetDisable", 1f);
        theQM.completedQuests [curQuestNum] = true;
        GameManager.instance.gameData.npcs[npcID] = npcDialogueIdAfterQuest;
	}

    void SetDisable()
    {
        gameObject.SetActive(false);
    }
}
