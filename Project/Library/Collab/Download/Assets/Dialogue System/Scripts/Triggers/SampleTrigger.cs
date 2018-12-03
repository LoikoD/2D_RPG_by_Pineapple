using UnityEngine;
using System.Collections;

public class SampleTrigger : DialogueTrigger {

	public override void FireTrigger ()
	{
		if (triggerOnce && triggered){   //This is just to avoid multiple triggers if you only want it to trigger once
			return;
		}
		triggered = true;


		//add whatever you want to happen here
	}
}
