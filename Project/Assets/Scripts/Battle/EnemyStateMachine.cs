using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : MonoBehaviour {


    public AudioSource attackSound;
    public AudioSource hurtSound;

    public bool hasAttackAnim = false;






    private BattleStateMachine BSM;
    public BaseEnemy enemy;

    public enum TurnState
    {
        PROCESSING,
        CHOOSE_ACTION,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    // for the EnemyBar
    private float curCooldown = 0f;
    private Image ProgressBar;
    private Image HPBar;
    public Sprite EnemyIcon;

    // this gameobject
    private Vector3 startPosition;
    public GameObject Selector;

    // timeforaction stuff
    private bool actionStarted = false;
    public GameObject HeroToAttack;
    private float animSpeed = 5f;

    //alive
    private bool alive = true;

    //enemyPanel
    private EnemyPanelStats stats;
    public GameObject EnemyBar;
    private Transform EnemyPanelSpacer1;
    private Transform EnemyPanelSpacer2;

	private Animator anim;

	SpriteRenderer spr_renderer;

	float height;


    void Start () {


        EnemyPanelSpacer1 = GameObject.Find("BattleCanvas").transform.Find("EnemyPanel").transform.Find("EnemyPanelSpacer1");
        EnemyPanelSpacer2 = GameObject.Find("BattleCanvas").transform.Find("EnemyPanel").transform.Find("EnemyPanelSpacer2");
        startPosition = transform.position;
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        CreateEnemyPanel();
        currentState = TurnState.PROCESSING;

		anim = GetComponent<Animator> ();

		spr_renderer = GetComponent<SpriteRenderer> ();

		height = GetComponent<SpriteRenderer>().bounds.size.y;

    }
    void Update () {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpgradeProgressBar();
                break;
            case (TurnState.CHOOSE_ACTION):
                ChooseAction();
                currentState = TurnState.WAITING;
                break;
            case (TurnState.WAITING):
                //idle state
                break;
            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;
            case (TurnState.DEAD):
                if (!alive)
                {
					
					
                    return;
                }
                else
                {
				anim.SetBool ("isDead",true);
				this.gameObject.transform.position -= new Vector3 (0, height, 0);


				//dead animation
				if (enemy.afterDeathFlipV) {
					this.gameObject.transform.localScale = new Vector3 (
						this.gameObject.transform.localScale.x,
						this.gameObject.transform.localScale.y * -1,
						this.gameObject.transform.localScale.z);
				} else {
					this.gameObject.transform.Rotate(0, 0, 90);
				}


                    //change tag
                    this.gameObject.tag = "DeadEnemy";
                    //not attackable by enemy
                    BSM.EnemysInBattle.Remove(this.gameObject);
                    //deactivate the selector
                    Selector.SetActive(false);
                    //remove item from performlist
                    if (BSM.EnemysInBattle.Count > 0)
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
                                    BSM.PerformList[i].AttackersTarget = BSM.EnemysInBattle[Random.Range(0, BSM.EnemysInBattle.Count)];
                                }
                            }
                        }
                    }





                   
                    //reset enemybuttons
                    BSM.EnemyButtons();
                    
                    alive = false;
                    BSM.battleStates = BattleStateMachine.PerformAction.CHECK_ALIVE;
                }
                break;

        }
    }

    void UpgradeProgressBar()
    {
        curCooldown = curCooldown + Time.deltaTime;
        float calcCooldown = curCooldown / enemy.coolDownTime;
        ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calcCooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if (curCooldown >= enemy.coolDownTime)
        {
            currentState = TurnState.CHOOSE_ACTION;
        }
    }

    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = enemy.theName;
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;
        if (BSM.HeroesInBattle.Count == 0)
        {
            currentState = TurnState.WAITING;
        }
        int randomTarget = Random.Range(0, BSM.HeroesInBattle.Count);
        myAttack.AttackersTarget = BSM.HeroesInBattle[randomTarget];
        int randomAttack = Random.Range(0, enemy.attacks.Count);
        myAttack.choosenAttack = enemy.attacks[randomAttack];
        BSM.CollectActions(myAttack);
    }

    void UpgradeHpBar()
    {
        float calcHpBar = enemy.curHP / enemy.baseHP;
        HPBar.transform.localScale = new Vector3(Mathf.Clamp(calcHpBar, 0, 1), HPBar.transform.localScale.y, HPBar.transform.localScale.z);
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        //animate the enemy near the hero to attack
		if (enemy.isMelee == true) {
			Vector3 heroPosition = new Vector3 (HeroToAttack.transform.position.x + 0.5f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z);
			while (MoveTowardsEnemy (heroPosition)) {
				if (checkDeath ()) {
					
			
					BSM.PerformList.RemoveAt (0);

					//reset BSM -> Wait
					BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
					//reset this enemy state
					curCooldown = 0f;


					actionStarted = false;
					yield break;
				} else {
	
					yield return null;
				}
			}
		}

		if (hasAttackAnim) {
			anim.SetBool ("isAttacking", true);
            //wait a bit
            yield return new WaitForSeconds (anim.GetFloat("timeToAttackAnim"));

			anim.SetBool ("isAttacking", false);
            attackSound.Play();

            if (enemy.myProj != null) {

				GameObject proj = Instantiate (enemy.myProj) as GameObject;

				Projectile MyPr = proj.GetComponent<Projectile> ();

				MyPr.SetStartPos (startPosition);
				MyPr.SetTargetPos (new Vector3 (HeroToAttack.transform.position.x + 0.5f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z));
				MyPr.Launch ();

				yield return new WaitForSeconds (0.5f);

			}
		}
        else
        {
            attackSound.Play();
            yield return new WaitForSeconds (0.5f);
		}

        //do damage
        DoDamage();

        //animate back to startposition
		if (enemy.isMelee == true) {
			Vector3 firstPosition = startPosition;
			while (MoveTowardsStart (firstPosition)) {
				yield return null;
			}
		}
        //remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);

        if (BSM.battleStates != BattleStateMachine.PerformAction.WIN && BSM.battleStates != BattleStateMachine.PerformAction.LOSE)
        {
            //reset BSM -> Wait
            BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
            //reset this enemy state
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

    void DoDamage()
    {
        float calcDamage = enemy.curATK + BSM.PerformList[0].choosenAttack.attackDamage;
        HeroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calcDamage);

    }

    public void TakeDamage(float getDamageAmount)
    {
        hurtSound.Play();
		StartCoroutine (spriteBlinking());

        float calcDamage = getDamageAmount * (1 - (0.05f * enemy.curDEF) / (1 + 0.05f * Mathf.Abs(enemy.curDEF)));
        enemy.curHP -= calcDamage;
	    checkDeath();
        UpgradeHpBar();
    }
    
    void CreateEnemyPanel()
    {
        EnemyBar = Instantiate(EnemyBar) as GameObject;
        stats = EnemyBar.GetComponent<EnemyPanelStats>();
        HPBar = stats.HPBar;
        ProgressBar = stats.ProgressBar;
        stats.EnemyIcon.sprite = EnemyIcon;
        if (BSM.createdEnemyBars >= 2)
        {
            EnemyBar.transform.SetParent(EnemyPanelSpacer2, false);
            ++BSM.createdEnemyBars;
        }
        else
        {
            EnemyBar.transform.SetParent(EnemyPanelSpacer1, false);
            ++BSM.createdEnemyBars;
        }
        UpgradeHpBar();
    }

	bool checkDeath()
	{
		if (enemy.curHP <= 0) {
			enemy.curHP = 0;
			currentState = TurnState.DEAD;
			return true;
		} else {
			return false;
		}

	}

	IEnumerator spriteBlinking()// 
	{
		int times_to_blink = 1;

		float deltaDistance = 0.15f;


		while (times_to_blink != 0) {

			if (enemy.curHP / enemy.baseHP < 0.3f) {
				spr_renderer.color = new Color (1f, 0f, 0f, 0.5f);
			} else {
				spr_renderer.color = new Color (1f, 1f, 1f, 0.5f);
			}


			StartCoroutine (ShakeModel(deltaDistance));

			yield return new WaitForSeconds (0.5f);
	

			spr_renderer.color = new Color(1f, 1f, 1f, 1f);
			yield return new WaitForSeconds (0.5f);

			times_to_blink -= 1;
		}
	}

	IEnumerator ShakeModel(float deltaDistance)
	{
		Vector3 targetPosR = new Vector3 (startPosition.x + deltaDistance, startPosition.y, startPosition.z);
		Vector3 targetPosL = new Vector3 (startPosition.x - deltaDistance, startPosition.y, startPosition.z); 

		while (MoveTowardsTarget (targetPosR)) {
			yield return null;
		}
		while (MoveTowardsTarget (startPosition)) {
			yield return null;
		}
		while (MoveTowardsTarget (targetPosL)) {
			yield return null;
		}
		while (MoveTowardsTarget (startPosition)) {
			yield return null;
		}
	}
}
