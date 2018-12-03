using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour {

    public static GameObject dBox;
    public static Text dText;
   // public static Text speakerNameText;
   // public static Image speakerIcon;
    public static GameObject button1;
    public static GameObject button2;
    // private PlayerMovement player;

    public static GameObject buttonNext;

    public static Text button1Text;
    public static Text button2Text;
    public static Text button3Text;

    public static BaseDialogue currentDialogue;

  //  public bool dialogueActive;

  //  public string[] dialogueLines;
  //  public int currentLine;
    
	// Use this for initialization
	void Start () {
        dBox = transform.GetChild(0).gameObject;
        dText = dBox.transform.Find("Text").GetComponent<Text>();

        button1 = dBox.transform.Find("Choice1").gameObject;
        button2 = dBox.transform.Find("Choice2").gameObject;

        buttonNext = dBox.transform.Find("Next").gameObject;

        button1Text = button1.transform.GetChild(0).GetComponent<Text>();
        button2Text = button2.transform.GetChild(0).GetComponent<Text>();

       // player = FindObjectOfType<PlayerMovement>();
	}

    // Update is called once per frame
    /*void Update()
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
    }*/

    public static void ShowDialogue()
    {
        int messageIndex = currentDialogue.s - 1;
        dBox.SetActive(true);
        dText.text = currentDialogue.message[messageIndex].messageText;

        DisableAllButtons();  //Disables all buttons
        EnableChoices(currentDialogue.message[messageIndex].choices.Length);  //Enables needed buttons
    }

    public static void EndDialog()
    {
        dBox.SetActive(false);
    }

    public void NextMessage()
    {
        currentDialogue.NextMessage();
    }


    //Choices Buttons
    public void FirstDialogueChoice()
    {
        currentDialogue.GoToNextMessage(currentDialogue.message[currentDialogue.s - 1].choiceDestiny[0]);
    }

    public void SecondDialogueChoice()
    {
        currentDialogue.GoToNextMessage(currentDialogue.message[currentDialogue.s - 1].choiceDestiny[1]);
    }

    public static void DisableAllButtons()
    {
        button1.SetActive(false);
        button2.SetActive(false);

        buttonNext.SetActive(false);
    }

    public static void EnableChoices(int numberOfChoices)
    {
        if (numberOfChoices == 0)
        {
            buttonNext.SetActive(true);
            EventSystem.current.SetSelectedGameObject(buttonNext);
        }
        else if (numberOfChoices == 1)
        {
            button1.SetActive(true);
            button1Text.text = currentDialogue.message[currentDialogue.s - 1].choices[0];
            //			EventSystem.current.SetSelectedGameObject(button1);
        }
        else if (numberOfChoices == 2)
        {
            button1.SetActive(true);
            button1Text.text = currentDialogue.message[currentDialogue.s - 1].choices[0];
            button2.SetActive(true);
            button2Text.text = currentDialogue.message[currentDialogue.s - 1].choices[1];
            //			EventSystem.current.SetSelectedGameObject(button1);
        }

    }
}
