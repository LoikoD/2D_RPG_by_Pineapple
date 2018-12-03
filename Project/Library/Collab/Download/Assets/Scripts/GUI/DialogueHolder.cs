using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHolder : MonoBehaviour {

    private DialogueManager dManager;

    public string dialogueSpeakerName;
    public string[] dialogueLines;
    public Sprite icon;

	// Use this for initialization
	void Start () {
        dManager = FindObjectOfType<DialogueManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        {
            if (collision.gameObject.name == "Player")
            {
                if (Input.GetKeyUp(KeyCode.F))
                {
                    if (!dManager.dialogueActive)
                    {
                        dManager.speakerNameText.gameObject.SetActive(true);
                        dManager.speakerNameText.text = dialogueSpeakerName;
                        dManager.dialogueLines = dialogueLines;
                        dManager.speakerIcon.gameObject.SetActive(true);
                        dManager.speakerIcon.sprite = icon;
                        dManager.currentLine = 0;
                        dManager.ShowDialogue();
                    }

                    if (GetComponentInParent<SheepBehavior>() != null)
                    {
                        GetComponentInParent<SheepBehavior>().canMove = false;
                    }
                }
            }
        }
    }
}
