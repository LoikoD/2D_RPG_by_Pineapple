using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestsWindowController : MonoBehaviour {

    public GameObject area;
    public GameObject activeQuests;
    public GameObject completedQuests;
    public GameObject quest;
    public GameObject questWindow;
    public QuestManager questManager;
    private int activeNum;
    private int completedNum;
    public bool menu;

    // Use this for initialization
    void Start () {
        activeNum = 0;
        completedNum = 0;

        for (int i = 0; i < GameManager.instance.gameData.activeQuests.Count; ++i)
        {
            if (GameManager.instance.gameData.activeQuests[i])
            {
                AddActiveQuest(i, questManager.quests[i].questName, questManager.quests[i].xpGain);
            }
        }

        for (int i = 0; i < GameManager.instance.gameData.completedQuests.Length; ++i)
        {
            if (GameManager.instance.gameData.completedQuests[i])
            {
                AddCompletedQuest(i, questManager.quests[i].questName, questManager.quests[i].xpGain);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeybindsManager.Instance.KeyBinds["QuestsWindowButton"]) && !menu)
        {
            ChangeWindowState();
        }
    }

    public void AddActiveQuest(int id, string name, int xp)
    {
        if (activeNum == 0)
        {
            Destroy(activeQuests.transform.GetChild(0).gameObject);
        }
        else
        {
            area.GetComponent<VerticalLayoutGroup>().spacing += 25;
        }
        GameObject newQuest = Instantiate(quest, activeQuests.transform) as GameObject;
        newQuest.name = "Quest" + id.ToString();
        newQuest.GetComponent<Text>().text = name;
        newQuest.transform.Find("Xp").GetComponent<Text>().text = xp.ToString() + " XP";
        activeNum++;
    }

    public void AddCompletedQuest(int id, string name, int xp)
    {
        if (completedNum == 0)
        {
            Destroy(completedQuests.transform.GetChild(0).gameObject);
        }
        GameObject newQuest = Instantiate(quest, completedQuests.transform) as GameObject;
        newQuest.name = "Quest" + id.ToString();
        newQuest.GetComponent<Text>().text = name;
        newQuest.transform.Find("Xp").GetComponent<Text>().text = xp.ToString() + " XP";
        completedNum++;
    }

    public void MoveQuest(int id)
    {
        activeNum--;
        if (activeNum == 0)
        {
            GameObject noneText = Instantiate(quest, activeQuests.transform) as GameObject;
            noneText.name = "None";
            noneText.GetComponent<Text>().text = "Нет";
            noneText.transform.Find("Xp").GetComponent<Text>().text = string.Empty;
        }
        else
        {
            area.GetComponent<VerticalLayoutGroup>().spacing -= 25;
        }
        if (completedNum == 0)
        {
            Destroy(completedQuests.transform.GetChild(0).gameObject);
        }
        activeQuests.transform.Find("Quest" + id.ToString()).SetParent(completedQuests.transform);
        completedNum++;
    }

    public void ChangeWindowState()
    {
        GameManager.instance.playerCharacter.GetComponent<PlayerMovement>().canMove = questWindow.activeSelf;
        questWindow.SetActive(!questWindow.activeSelf);
    }
}
