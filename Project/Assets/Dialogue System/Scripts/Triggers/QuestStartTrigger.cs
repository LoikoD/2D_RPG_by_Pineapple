using UnityEngine;
using System.Collections;

public class QuestStartTrigger : DialogueTrigger {

    private QuestManager theQm;

    public int questNum;

    public bool startQuest;
    public bool endQuest;

    void Start()
    {

        theQm = FindObjectOfType<QuestManager>();

    }

    public override void FireTrigger ()
	{
		if (triggerOnce && triggered){   //This is just to avoid multiple triggers if you only want it to trigger once
			return;
		}
		triggered = true;

        if (!theQm.completedQuests[questNum]) // quest not completed
        {
            if (startQuest && !theQm.quests[questNum].gameObject.activeSelf)
            {
                theQm.quests[questNum].startQuest();
            }
            if (endQuest && theQm.quests[questNum].gameObject.activeSelf)
            {
                theQm.quests[questNum].endQuest();
            }
        }
    }
}
