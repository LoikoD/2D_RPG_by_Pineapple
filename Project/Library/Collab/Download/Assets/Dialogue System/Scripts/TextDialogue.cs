using UnityEngine;
using System.Collections;

[System.Serializable]
public class TextDialogue {

	public string messageText;                    //This is what the npc will say
	public string[] choices = new string[0];      //These are the player dialogue choices
	public int[] choiceDestiny = new int[0];      //These are where the choices take the player 

	public bool hasTrigger = false;               //If the dialogue has a trigger
	public int triggerID;                         //This is just the position of the trigger in the trigger list

	[HideInInspector]
	public int ownerID;                           //This is a number used to identify the instance of Dialogue
}
