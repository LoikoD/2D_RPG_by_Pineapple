using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject dBox;
    public Text dText;
    public Text speakerNameText;
    public Image speakerIcon;
    private PlayerMovement player;

    public bool dialogueActive;

    public string[] dialogueLines;
    public int currentLine;
    
	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerMovement>();
	}

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            currentLine++;
        }

        if (currentLine >= dialogueLines.Length)
        {
            speakerNameText.gameObject.SetActive(false);
            speakerIcon.gameObject.SetActive(false);
            dBox.SetActive(false);
            dialogueActive = false;
            player.canMove = true;
            currentLine = 0;
        }

        dText.text = dialogueLines[currentLine];
    }

    public void ShowDialogue()
    {
        dialogueActive = true;
        dBox.SetActive(true);
        player.canMove = false;
    }
}
