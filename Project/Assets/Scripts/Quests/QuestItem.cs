using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour {

	public int questNum;
	public string itemName;
    public int itemID;


	private QuestManager theQM;



	// Use this for initialization
	void Start () {

		theQM = FindObjectOfType<QuestManager> ();
        if (GameManager.instance.gameData.foundItems[itemID])
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Player") {

			if (!theQM.completedQuests [questNum] && theQM.quests [questNum].gameObject.activeSelf) {

				theQM.itemCollected = itemName;
                GameManager.instance.gameData.foundItems[itemID] = true;

				gameObject.SetActive (false);
			}
		}
	}
}
