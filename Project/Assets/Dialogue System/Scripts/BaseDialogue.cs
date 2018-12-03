using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseDialogue : MonoBehaviour {


    public int npcID;

    public int dialogueID; //Just to diferentiate instances for safer referencing from database

	[HideInInspector]
	public List<TextDialogue> message = new List<TextDialogue>();

	[HideInInspector]
	public List<int> messageId = new List<int>();  //This list stores the id for all messages, so it can easily reference them from database
	
	[HideInInspector]
	public bool enter = false;                     //Player in range?
	[HideInInspector]
	public int s  = 0;                             //This is just a counter to keep track of current message
	protected GameObject player;                   //player gameobject

	protected bool talking = false;

	//These variables are for changing dialogue id
	protected bool changeID = false;
	protected int newDialogueID;

    public bool questDialogue;

    void Start(){
		LoadDialogueFromDatabase();
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.F) && enter && !talking && Time.timeScale != 0.0f){        //checking if player is in range, not currently in dialog and game is not paused
			NextMessage();
		}

		if (Time.timeScale == 0)
		{
			talking = true;
			Invoke("EnableTalking", 0.1f);            //This is a "cooldown" for the dialogue, so it doesnt start just when it finishes
		}
	}
	
	protected void OnTriggerEnter2D (Collider2D other) {
		if(other.CompareTag("Player")){
			s = 0;
			player = other.gameObject;
			enter = true;
//			Debug.Log("in trigger");
		}
		
	}
    
	protected void OnTriggerExit2D (Collider2D other) {
		if(other.CompareTag("Player")){
			s = 0;
			enter = false;
			CloseTalk();
//			Debug.Log("out of trigger");
		}
		
	}
	
	protected void CloseTalk(){
		HideGUI();
		Time.timeScale = 1.0f;

		//Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;

		s = 0;
		ChangeID();
	}

	//This function is used when there is no dialog choice and it goes to the next message in the message list
	public virtual void NextMessage(){
		if(!enter && !questDialogue){
			return;
		}

		//Making sure s isnt out of bounds
		if (s < message.Count){
			if (CheckSpecialConditions(s)) //This is where the trigger is executed
            {
                CloseTalk();
                return;
            }
		}
		
		s++;
		if(s > message.Count){
			CloseTalk();
		}else{
			//Time.timeScale = 0.0f;

			//Cursor.lockState = CursorLockMode.Confined;
			//Cursor.visible = true;

			ShowGUI();
		}
	}
	
	//Its done on this function and not on "NextMessage" because this is the one that controls flow, the other is just cyclic (you can jump from message 1 to message 6 for example, the other goes from 1 to 2)
	public virtual void GoToNextMessage(int messageNumber)
	{
		if(!enter){
			return;
		}

		//Making sure s isnt out of bounds
		if (messageNumber <= message.Count){
			if (CheckSpecialConditions(messageNumber))   //This is where the trigger is executed
			{
				return;
			}
		}

		s = messageNumber + 1;  //this +1 is to keep consistency, the s is always 1 over the actual message
		if(s > message.Count){
			CloseTalk();
		}else{
			//Time.timeScale = 0.0f;
			
			//Cursor.lockState = CursorLockMode.Confined;
			//Cursor.visible = true;

			ShowGUI();
		}
	}


	//Add conditions here, like shop and other stuff
	public virtual bool CheckSpecialConditions(int messageNumber)
	{
		//This function should return false if you want the dialogue to continue with its flow
		return false;
	}


	
	protected void ShowGUI()   //enable the dialogue box
	{
		talking = true;
        DialogueManager.currentDialogue = this;
        DialogueManager.ShowDialogue();
	}
	

	void HideGUI()   //disable the dialogue box
	{
        DialogueManager.EndDialog();
        if (!questDialogue)
        {
            transform.GetComponent<SheepBehavior>().canMove = true;
        }
        FindObjectOfType<PlayerMovement>().canMove = true;
        Invoke("EnableTalking", 0.1f);
	}

	void EnableTalking()
	{
		talking = false;
	}


	//Function to load messages from database
	public void LoadDialogueFromDatabase(){
		//Load messageId from database
		ReloadMessageID();

		message = new List<TextDialogue>();

		for (int i=0; i< messageId.Count; i++){
			message.Add(GameObject.FindWithTag("Dialogue Database").GetComponent<DialogueDatabase>().messageDatabase[messageId[i]]);

		}
	}


	//This is only used in editor
	//Adds/Updates everything in message list to database
	public void UpdateDialogueInDatabase(){
		//Clears list of message index
		messageId = new List<int>();

		//Clean messages from this id in database
		GameObject.FindWithTag("Dialogue Database").GetComponent<DialogueDatabase>().CleanID(dialogueID);

		for (int i=0; i< message.Count; i++){

			message[i].ownerID = dialogueID;

			//Add all as new
			messageId.Add( GameObject.FindWithTag("Dialogue Database").GetComponent<DialogueDatabase>().AddDialogueToDatabase(message[i]) );
		}
	}


	void ReloadMessageID(){
		messageId = new List<int>();

		messageId = GameObject.FindWithTag("Dialogue Database").GetComponent<DialogueDatabase>().GetMessageIDList(dialogueID);
	}


	//Change dialogueID, dont use to change dialogue while its open, the bool instant is just what it says, if you make it true, it will load the new dialogues instantly, if false it will be changed when current dialogue ends
	public void ChangeDialogueToID(int _dialogueID, bool instant){

		changeID = true;
		newDialogueID = _dialogueID;
        GameManager.instance.gameData.npcs[npcID] = newDialogueID;
		if (instant){
			ChangeID();
		}
	}

	void ChangeID(){
		if (changeID){
			dialogueID = newDialogueID;
			changeID = false;
			LoadDialogueFromDatabase();
		}
	}

}
