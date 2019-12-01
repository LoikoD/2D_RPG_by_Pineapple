using UnityEngine;
using System.Collections;

public class StartBattle : DialogueTrigger
{

    public override void FireTrigger()
    {
        if (triggerOnce && triggered)
        {
            return;
        }
        triggered = true;

        GameManager.instance.curSquad = gameObject.GetComponentInParent<EnemySquadData>();
        GameManager.instance.gameState = GameManager.GameStates.BATTLE_STATE;
    }
}
