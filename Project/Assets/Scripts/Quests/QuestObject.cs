using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestObject : MonoBehaviour {

    public int curQuestNum;

	public QuestManager theQM;

    public string questName;

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

    public int xpGain;

    private DialogueWithTrigger dialogue;
    private SfxManager sfxManager;
    public QuestsWindowController questsWindowController;

	// Use this for initialization
	void Start () {
        dialogue = transform.GetComponent<DialogueWithTrigger>();
        sfxManager = FindObjectOfType<SfxManager>();
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

                if (enemyKillCount >= enemiesToKill)
                {

                    endQuest();

                }
            }

		}

	}

	public void startQuest()
    {
        gameObject.SetActive(true);
        theQM.updateQuestText.GetComponent<Text>().text = "Новое задание";
        theQM.ShowUpdateQuestText();
        questsWindowController.AddActiveQuest(curQuestNum, questName, xpGain);
    }


	public void endQuest()
	{
        dialogue.allowed = true;
        Invoke("SetDisable", 1f);
        theQM.completedQuests [curQuestNum] = true;
        GameManager.instance.gameData.npcs[npcID] = npcDialogueIdAfterQuest;
        GameManager.instance.XpGain(xpGain);
        theQM.updateQuestText.GetComponent<Text>().text = "Задание выполнено";
        theQM.ShowUpdateQuestText();
        sfxManager.questCompletedSound.Play();
        questsWindowController.MoveQuest(curQuestNum);
    }

    void SetDisable()
    {
        gameObject.SetActive(false);
    }
}
