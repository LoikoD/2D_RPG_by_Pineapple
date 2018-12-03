using UnityEngine;
using System.Collections;

public class DialogueWithTrigger : BaseDialogue {

    private bool triggered = false;
    public bool allowed = false;

	void Update () {
		if(Input.GetKeyDown(KeyCode.F) && enter && !talking && Time.timeScale != 0.0f && !questDialogue){
            if (GameManager.instance.gameData.npcs[npcID] !=  dialogueID && GameManager.instance.gameData.npcs[npcID] != 0)
            {
                ChangeDialogueToID(GameManager.instance.gameData.npcs[npcID], true);
            }
            transform.GetComponent<SheepBehavior>().canMove = false;
            FindObjectOfType<PlayerMovement>().canMove = false;
            NextMessage();
		}

        if(enter && questDialogue)
        {
            allowed = true;
        }

        if(allowed && !talking && Time.timeScale != 0.0f && questDialogue && !triggered)
        {
            if (GameManager.instance.gameData.npcs[npcID] != 0)
            {
                ChangeDialogueToID(GameManager.instance.gameData.npcs[npcID], true);
            }
            triggered = true;
            FindObjectOfType<PlayerMovement>().canMove = false;
            NextMessage();
        }
		
		if (Time.timeScale == 0)
		{
			talking = true;
			Invoke("EnableTalking", 0.1f);
		}
	}

	//MUST RETURN FALSE TO LEAVE DIALOG FLOW AS NORMAL
	public override bool CheckSpecialConditions(int messageNumber)
	{
		if (message[messageNumber].hasTrigger){
			ActivateTrigger(message[messageNumber].triggerID);
            return true;
		}
		return false;
	}

	//Activates trigger of index "index" on trigger list
	void ActivateTrigger(int index){
//		GetComponent<DialogueTriggerContainer>().Triggers[index].gameObject.SetActive(true);
		GetComponent<DialogueTriggerContainer>().Triggers[index].FireTrigger();
	}
}
