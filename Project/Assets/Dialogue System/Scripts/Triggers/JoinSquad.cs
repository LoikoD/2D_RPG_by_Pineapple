using UnityEngine;
using System.Collections;

public class JoinSquad : DialogueTrigger {

    private int heroSquadNum;
    private string heroName;
    

	public override void FireTrigger ()
	{
		if (triggerOnce && triggered){   //This is just to avoid multiple triggers if you only want it to trigger once
			return;
		}
		triggered = true;
        HeroNpc heroNpc = transform.GetComponent<HeroNpc>();
        heroName = transform.GetComponent<HeroNpc>().heroName;
       heroSquadNum = GameManager.instance.squadsManager.FindIndexByNameInHeroNpcs(heroName);
        GameManager.instance.gameData.heroSquad[heroNpc.possibleHeroesNum] = true;
        GameManager.instance.gameData.joinedHeroNpcs.Add(heroSquadNum);
        Destroy(gameObject);
	}
}
