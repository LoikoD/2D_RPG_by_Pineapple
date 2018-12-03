using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour {

	Camera myCam;
	// Use this for initialization
	void Start () {
		myCam = GetComponent<Camera> ();

	}
	




	public void SetBGColor(BattleStateMachine.BattleMaps map)
	{
		switch (map) {

		case BattleStateMachine.BattleMaps.ForestBM: 
			myCam.backgroundColor = new Color(0.03f, 0.5f, 0.21f);
			return;
		case BattleStateMachine.BattleMaps.PrisonBM:
			myCam.backgroundColor = new Color (0.56f, 0.39f, 0.26f);
			return;
		default:
			myCam.backgroundColor = new Color (0f, 0f, 0f); 
			return;
		}
	}
}
