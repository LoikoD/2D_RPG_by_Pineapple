
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;


public class HeroStateMachine : MonoBehaviour
{


    public AudioSource attackSound;
    public AudioSource hurtSound;

    private const float healthPotionRestoreAmount = 50f;

    private BattleStateMachine BSM;
    public BaseHero hero;
	Animator anim;
	SpriteRenderer spr_renderer;


    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        ATTACK,
        SPECIAL_ATTACK,
        HEAL,
		HURTED,
        DEAD
    }

    public TurnState currentState;
    //for the HeroBar
    private float curCooldown = 0f;
    private Image ProgressBar;
    private Image HPBar;
    public Sprite HeroIcon;

    public GameObject Selector;

    //IEnumarator
    public GameObject EnemyToAttack;
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animSpeed = 5f;
    private float reduceArmorAmount = 1f;

    //dead
    private bool alive = true;

    //heroPanel
    private HeroPanelStats stats;
    public GameObject HeroBar;
    private Transform HeroPanelSpacer1;
    private Transform HeroPanelSpacer2;


    void Start ()
    {
		anim = GetComponent<Animator> ();
		spr_renderer = GetComponent<SpriteRenderer> ();


        HeroPanelSpacer1 = GameObject.Find("BattleCanvas").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer1");
        HeroPanelSpacer2 = GameObject.Find("BattleCanvas").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer2");
        startPosition = transform.position;
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        CreateHeroPanel();
        currentState = TurnState.PROCESSING;
	}
	
	void Update ()
    {
		switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpgradeProgressBar();
                break;
            case (TurnState.ADDTOLIST):
                BSM.HeroesToManage.Add(this.gameObject);
                currentState = TurnState.WAITING;
                break;
			case (TurnState.WAITING):
                //idle state

                break;
            /*case (TurnState.ACTION):
				StartCoroutine(TimeForAction());
				break;*/
            case (TurnState.HEAL):
                StartCoroutine(Healing());
                break;
			case (TurnState.ATTACK):
				StartCoroutine(TimeForAction(TurnState.ATTACK));
                break;
            case (TurnState.SPECIAL_ATTACK):
                StartCoroutine(TimeForAction(TurnState.SPECIAL_ATTACK));
                break;
            case (TurnState.DEAD):
                if (!alive)
                {
				 
                    return;
                }
                else
                {
                    //change tag
                    this.gameObject.tag = "DeadHero";
                    //not attackable by enemy
                    BSM.HeroesInBattle.Remove(this.gameObject);
                    //not managable
                    BSM.HeroesToManage.Remove(this.gameObject);
                    //deactivate the selector
                    Selector.SetActive(false);
                    //reset gui
                    BSM.attackButton.SetActive(false);
                    BSM.EnemySelectPanel.SetActive(false);
                    //remove item from performlist
                    if (BSM.HeroesInBattle.Count > 0)
                    {
                        for (int i = 0; i < BSM.PerformList.Count; ++i)
                        {
                            if (i != 0)
                            {
                                if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                                {
                                    BSM.PerformList.Remove(BSM.PerformList[i]);
                                }
                                else if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                                {
                                    BSM.PerformList[i].AttackersTarget = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
                                }
                            }
                        }
                    }
                    //dead animation
					this.gameObject.transform.Rotate(0, 0, 90);
					anim.SetBool ("isDead", true);



                    //
                    //reset heroinput
                    //BSM.HeroButtons();

                    alive = false;
                    BSM.battleStates = BattleStateMachine.PerformAction.CHECK_ALIVE;
                }
                break;

        }
	}

    void UpgradeProgressBar ()
    {
        curCooldown = curCooldown + Time.deltaTime;
        float calcCooldown = curCooldown / hero.coolDownTime;
        ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calcCooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if (curCooldown >= hero.coolDownTime)
        {
            currentState = TurnState.ADDTOLIST;
        }
    }

    void UpgradeHpBar()
    {
        float calcHpBar = hero.curHP / hero.baseHP;
        HPBar.transform.localScale = new Vector3(Mathf.Clamp(calcHpBar, 0, 1), HPBar.transform.localScale.y, HPBar.transform.localScale.z);
    }
    private IEnumerator Healing()
    {
        if (actionStarted)
        {
            yield break;
        }
        actionStarted = true;
        GameManager.instance.gameData.potionQuantity -= 1;

        //animation of healing?

        yield return new WaitForSeconds(0.5f);

        //do heal
        RestoreHP(healthPotionRestoreAmount);

        
        //remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);

        if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
        {
            //reset BSM -> Wait
            BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
            //reset this hero state
            curCooldown = hero.coolDownTime / 2;
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }

        //end coroutine
        actionStarted = false;
    }

    private IEnumerator TimeForAction(TurnState actionState)
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        //animate the enemy near the hero to attack
		if (hero.isMelee) {

			Vector3 enemyPosition = new Vector3(EnemyToAttack.transform.position.x - 0.5f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
			while (MoveTowardsEnemy(enemyPosition))
			{
				yield return null;
			}
		}


        attackSound.Play();
		anim.SetBool ("isAttacking", true);

		yield return new WaitForSeconds (0.5f);

		anim.SetBool ("isAttacking", false);


        float calcDamage = hero.curATK + BSM.PerformList[0].choosenAttack.attackDamage;

        if (actionState == TurnState.SPECIAL_ATTACK)
        {
            calcDamage /= 1.5f;
        }
        DoDamage(calcDamage);


        //animate back to startposition
        if (hero.isMelee) {
			Vector3 firstPosition = startPosition;
			while (MoveTowardsStart(firstPosition))
			{
				yield return null;
			}
		}
        if (actionState == TurnState.SPECIAL_ATTACK)
        {
            ReduceArmor();
        }

        //remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);

        if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
        {
            //reset BSM -> Wait
            BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
            //reset this hero state
            curCooldown = 0f;
            currentState = TurnState.PROCESSING;
        }
        else
        {
            currentState = TurnState.WAITING;
        }

        //end coroutine
        actionStarted = false;
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
	private bool MoveTowardsTarget(Vector3 target)
	{
		return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
	}


	public void TakeDamage(float getDamageAmount)
    {

        //StartCoroutine(hurtAnimation());


        //anim.SetBool ("isHurted", true);

        hurtSound.Play();
        StartCoroutine (spriteBlinking ());

        float calcDamage = getDamageAmount * (1 - (0.05f * hero.curDEF) / (1 + 0.05f * Mathf.Abs(hero.curDEF)));
        hero.curHP -= calcDamage;
        if (hero.curHP <= 0)
        {
            hero.curHP = 0;
            currentState = TurnState.DEAD;
        }
        UpgradeHpBar();

    }

    public void RestoreHP(float healAmount)
    {
        //animation of restoring?
		StartCoroutine(healingAnimation());


        hero.curHP += healAmount;
        if (hero.curHP > hero.baseHP)
        {
            hero.curHP = hero.baseHP;
        }
        UpgradeHpBar();
    }

    void DoDamage(float damage)
    {
        EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(damage);
    }

    void ReduceArmor()
    {
        EnemyToAttack.GetComponent<EnemyStateMachine>().TakeAwayArmor(reduceArmorAmount);
    }

    void CreateHeroPanel()
    {
        HeroBar = Instantiate(HeroBar) as GameObject;
        stats = HeroBar.GetComponent<HeroPanelStats>();
        HPBar = stats.HPBar;
        ProgressBar = stats.ProgressBar;
        stats.HeroIcon.sprite = HeroIcon;
        if (BSM.createdHeroBars >= 2)
        {
            HeroBar.transform.SetParent(HeroPanelSpacer2, false);
            ++BSM.createdHeroBars;
        }
        else
        {
            HeroBar.transform.SetParent(HeroPanelSpacer1, false);
            ++BSM.createdHeroBars;
        }
        UpgradeHpBar();
    }


	IEnumerator attackAnimation()
	{
		anim.SetBool ("isAttacking", true);

		//wait a bit
		yield return new WaitForSeconds(1.2f);

		anim.SetBool ("isAttacking", false);
	}

	IEnumerator hurtAnimation()
	{
		anim.SetBool ("isHurted", true);

		//wait a bit
		yield return new WaitForSeconds(1.2f);

		anim.SetBool ("isHurted", false);
	}

	IEnumerator deathAnimation()
	{
		anim.SetBool ("isDead", true);

		yield return new WaitForSeconds (1f);
	}

	IEnumerator healingAnimation()
	{
		anim.SetBool ("isHealed", true);

		yield return new WaitForSeconds (0.75f);

		anim.SetBool ("isHealed", false);
	}

	IEnumerator spriteBlinking()// 
	{
		int times_to_blink = 1;
		float deltaDistance = 0.15f;

		while (times_to_blink != 0) {

			if (hero.curHP / hero.baseHP < 0.3f) {
				spr_renderer.color = new Color (1f, 0f, 0f, 0.5f);
			} else {
				spr_renderer.color = new Color (1f, 1f, 1f, 0.5f);
			}
				

			//StartCoroutine (ShakeModel(deltaDistance));

			ShakeModel (deltaDistance);

			yield return new WaitForSeconds (0.5f);

			spr_renderer.color = new Color(1f, 1f, 1f, 1f);


			yield return new WaitForSeconds (0.5f);

			times_to_blink -= 1;
		}
	}


	void ShakeModel(float deltaDistance)
	{
		Vector3 targetPosR = new Vector3 (startPosition.x + deltaDistance, startPosition.y, startPosition.z);
		Vector3 targetPosL = new Vector3 (startPosition.x - deltaDistance, startPosition.y, startPosition.z); 

		while (MoveTowardsTarget (targetPosR)) {
			//yield return null;
		}
		while (MoveTowardsTarget (startPosition)) {
			//yield return null;
		}
		while (MoveTowardsTarget (targetPosL)) {
			//yield return null;
		}
		while (MoveTowardsTarget (startPosition)) {
			//yield return null;
		}
	}

}
