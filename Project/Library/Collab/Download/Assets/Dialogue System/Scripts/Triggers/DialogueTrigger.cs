using UnityEngine;
using System.Collections;


public class DialogueTrigger : MonoBehaviour {

	public bool triggerOnce;   //if true, the trigger will only run once
	public bool triggered;     

	public virtual void FireTrigger()
	{
		//this is the method that is overriden for different kind of triggers
	}
}
