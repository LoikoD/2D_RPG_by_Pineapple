using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//These are for saving and loading binary files (c# library, not unity) doesnt work on web
using System;                 
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[RequireComponent (typeof (DialogueTriggerContainer))]
//There is no id=0 dialogue, to make this easier
public class DialogueDatabase : MonoBehaviour {

	public static DialogueDatabase instance;          //Quick reference on runtime

	public string fileName;  //This is the name of the file to load from/save to, this can be used to easily load different languages for example

	public List<TextDialogue> messageDatabase = new List<TextDialogue>();
	
	void Awake () {
		instance = this;
		LoadDialogueDatabase();
	}

	//Stores the dialogue in database and returns its id
	public int AddDialogueToDatabase(TextDialogue dialogue){
		if (messageDatabase.Count == 0){
			messageDatabase.Add(new TextDialogue());
		}

		int dialoguePos;

		dialoguePos = FindLastEmptySpot();
		if (dialoguePos == 0){ //This means list doesnt have empty "fields"
			dialoguePos = messageDatabase.Count;
			messageDatabase.Add(dialogue);
		}
		else{
			messageDatabase[dialoguePos] = dialogue;
		}
		return dialoguePos;
	}


	public int FindLastEmptySpot(){
		for (int i=1; i< messageDatabase.Count; i++){
			if (messageDatabase[i].ownerID == 0){             //no dialogue instance has id=0, so this is an empty field
				return i;
			}
		}
		return 0; //This means the list is actually full (no empty fields)
	}


	//Receives a dialogueID and returns a list of index of his messages
	public List<int> GetMessageIDList(int dialogueID){

		List<int> tempList = new List<int>();

		for (int i=1; i< messageDatabase.Count; i++){
			if (messageDatabase[i].ownerID == dialogueID){
				tempList.Add(i);
			}
		}
		return tempList;
	}


	//Deletes all messages of id 
	public void CleanID(int id){
		for (int i=0; i< messageDatabase.Count; i++)
		{
			if (messageDatabase[i].ownerID == id)
			{
				messageDatabase[i] = new TextDialogue();
			}
		}
	}



	public void SaveDialogueDatabase()
	{
		BinaryFormatter bf = new BinaryFormatter();
//		FileStream file = File.Create(Application.streamingAssetsPath + "/" + fileName); //Use this to save to folder "StreamingAssets", and comment next line
		FileStream file = File.Create(Application.dataPath + "/Dialogue System/" + fileName);

		bf.Serialize(file, messageDatabase);
		file.Close();
	}


	public void LoadDialogueDatabase()
	{
		//		if (File.Exists(Application.streamingAssetsPath + "/" + fileName))   // //Use this to save to folder "StreamingAssets", and comment next line
		if (File.Exists(Application.dataPath + "/Dialogue System/" + fileName))
		{
			BinaryFormatter bf = new BinaryFormatter();
			//			FileStream file = File.Open(Application.streamingAssetsPath + "/" + fileName, FileMode.Open);  //Use this to save to folder "StreamingAssets", and comment next line
			FileStream file = File.Open(Application.dataPath + "/Dialogue System/" + fileName, FileMode.Open);
			messageDatabase = (List<TextDialogue>) bf.Deserialize(file);
			file.Close();
		}
		else
		{
			Debug.Log("Dialogue file didn't load, check filename");
		}
	}
}
