using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour {

	private QuestManager theQm;

	public int questNum;

	public bool startQuest;
	public bool endQuest;



	// Use this for initialization
	void Start () {

		theQm = FindObjectOfType<QuestManager>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Player") 
		{

			if(!theQm.completedQuests[questNum]) // quest not completed
			{
				if (startQuest && !theQm.quests[questNum].gameObject.activeSelf)
                {
                    theQm.quests [questNum].startQuest ();
				}
				if (endQuest && theQm.quests[questNum].gameObject.activeSelf) 
				{
					theQm.quests [questNum].endQuest ();
				}
			}
		}	
	}
}
