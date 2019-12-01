using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour {

	BattleCamera battleCam;

	private GameManager theGM;

	public GameObject[] battleMaps;



    public enum PerformAction
    {
        WAIT,
        TAKE_ACTION,
        PERFORM_ACTION,
        CHECK_ALIVE,
        WIN,
        LOSE
    }

	public enum BattleMaps
	{
		ForestBM,
		PrisonBM
	}

	BattleMaps curBattleMap;

    public PerformAction battleStates;

    public List<HandleTurn> PerformList = new List<HandleTurn>();

    public List<GameObject> HeroesInBattle = new List<GameObject>();
    public List<GameObject> EnemysInBattle = new List<GameObject>();

    public enum HeroGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }

    public HeroGUI HeroInput;

    public List<GameObject> HeroesToManage = new List<GameObject>();
    private HandleTurn HeroChoice;

    //enemy selection stuff
    public GameObject enemyButton;
    public Transform EnemySpacer;
    public GameObject EnemySelectPanel;

   /* //hero selection stuff
    public GameObject heroButton;
    public Transform HeroSpacer;
    public GameObject HeroSelectPanel; */

    //actions of heroes
    public GameObject magicButton;
    public GameObject attackButton;
    public GameObject potionButton;

    public GameObject potionQuantityImage;

    //enemy buttons
    private List<GameObject> enemyBtns = new List<GameObject>();
   // private List<GameObject> heroBtns = new List<GameObject>();

    public int createdHeroBars;
    public int createdEnemyBars;

    public List<Transform> enemyPositions = new List<Transform>();
    public List<Transform> heroPositions = new List<Transform>();

    void Awake()
    {
        for (int i = 0; i < GameManager.instance.heroesToBattle.Count; ++i)
        {
            GameObject NewHero = Instantiate(GameManager.instance.heroesToBattle[i], heroPositions[i].position, Quaternion.identity) as GameObject;
            BaseHero curHero = NewHero.GetComponent<HeroStateMachine>().hero;
            if (curHero.theName == "Main Hero")
            {
                curHero.Health = GameManager.instance.gameData.attributes[0];
                curHero.Dexterity = GameManager.instance.gameData.attributes[1];
                curHero.Damage = GameManager.instance.gameData.attributes[2];
                curHero.Defense = GameManager.instance.gameData.attributes[3];
                curHero.UpdateStats();
            }
            NewHero.name = curHero.theName + "_" + (i + 1);
            curHero.theName = NewHero.name;
            HeroesInBattle.Add(NewHero);
        }
        for (int i = 0; i < GameManager.instance.enemyAmount; ++i)
        {
            GameObject NewEnemy = Instantiate(GameManager.instance.enemiesToBattle[i], enemyPositions[i].position, Quaternion.identity) as GameObject;
            NewEnemy.name = NewEnemy.GetComponent<EnemyStateMachine>().enemy.theName + "_" + (i + 1);
            NewEnemy.GetComponent<EnemyStateMachine>().enemy.theName = NewEnemy.name;
            EnemysInBattle.Add(NewEnemy);
        }
    }

    void Start () {
        battleStates = PerformAction.WAIT;
        //EnemysInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        //HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        HeroInput = HeroGUI.ACTIVATE;
        attackButton.SetActive(false);
        EnemySelectPanel.SetActive(false);
       // HeroSelectPanel.SetActive(false);
        magicButton.SetActive(false);
        potionButton.SetActive(false);
        potionQuantityImage.SetActive(false);

        EnemyButtons();
        //HeroButtons();

        theGM = FindObjectOfType<GameManager> ();

		battleCam = FindObjectOfType<BattleCamera> ();

		activateBattleMap ();
    }
	
	void Update () {
		switch (battleStates)
        {
            case (PerformAction.WAIT):
                if(PerformList.Count > 0)
                {
                    battleStates = PerformAction.TAKE_ACTION;
                }
                break;
            case (PerformAction.TAKE_ACTION):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if (PerformList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.HeroToAttack = PerformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                }

                if (PerformList[0].Type == "Hero")
                {
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                    if (PerformList[0].turnType == TurnType.ATTACK)
                    {
                        HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                        HSM.currentState = HeroStateMachine.TurnState.ATTACK;
                    }
                    else if (PerformList[0].turnType == TurnType.SPECIAL_ATTACK)
                    {
                        HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                        HSM.currentState = HeroStateMachine.TurnState.SPECIAL_ATTACK;
                    }
                    else if (PerformList[0].turnType == TurnType.HEAL)
                    {
                        HSM.currentState = HeroStateMachine.TurnState.HEAL;
                    }
                }
                battleStates = PerformAction.PERFORM_ACTION;
                break;
            case (PerformAction.PERFORM_ACTION):
                //idle
                break;
            case (PerformAction.CHECK_ALIVE):
                if (HeroesInBattle.Count < 1)
                {
                    battleStates = PerformAction.LOSE;
                }
                else if (EnemysInBattle.Count < 1)
                {
                    battleStates = PerformAction.WIN;
                }
                else
                {
                    ClearAttackPanel();
                    HeroInput = HeroGUI.ACTIVATE;
                    battleStates = PerformAction.WAIT;
                }
                break;
		    case (PerformAction.WIN):
			    for (int i = 0; i < HeroesInBattle.Count; ++i) {
				    HeroesInBattle [i].GetComponent<HeroStateMachine> ().currentState = HeroStateMachine.TurnState.WAITING;
			    }
			    GameManager.instance.LoadSceneAfterBattle (true);
			    GameManager.instance.enemiesToBattle.Clear ();
			    GameManager.instance.heroesToBattle.Clear ();

                break;
		    case (PerformAction.LOSE):
                for (int i = 0; i < EnemysInBattle.Count; ++i)
                {
                    EnemysInBattle[i].GetComponent<EnemyStateMachine>().currentState = EnemyStateMachine.TurnState.WAITING;
                }
                battleStates = PerformAction.PERFORM_ACTION;
                StartCoroutine(LostBattle());

                break;
        }

        switch (HeroInput)
        {
            case HeroGUI.ACTIVATE:
                if (HeroesToManage.Count > 0)
                {
                    HeroesToManage[0].transform.Find("HeroSelector").gameObject.SetActive(true);
                    HeroChoice = new HandleTurn();
                    attackButton.SetActive(true);
                    if (GameManager.instance.gameData.potionQuantity > 0)
                    {
                        potionButton.SetActive(true);
                        potionQuantityImage.SetActive(true);
                        potionQuantityImage.transform.Find("PotionQuantityText").GetComponent<Text>().text = (GameManager.instance.gameData.potionQuantity.ToString()) + " left";
                    }
                    if (HeroesToManage[0].GetComponent<HeroStateMachine>().hero.magicAttacks.Count > 0)
                    {
                        magicButton.SetActive(true);
                    }
                    HeroInput = HeroGUI.WAITING;
                }
                break;
            case HeroGUI.WAITING:
                //idle state
                break;
            case HeroGUI.DONE:
                HeroInputDone();
                break;
        }
	}


    IEnumerator LostBattle()
    {
        GameObject lostScreen = GameObject.Find("BattleCanvas").transform.Find("LostScreen").gameObject;
        lostScreen.SetActive(true);

        yield return new WaitForSeconds(3f);

        //lostScreen.SetActive(false);

        GameManager.instance.LoadSceneAfterBattle(false);
        GameManager.instance.enemiesToBattle.Clear();
        GameManager.instance.heroesToBattle.Clear();
    }

    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }

    public void EnemyButtons()
    {
        foreach (GameObject enemyBtn in enemyBtns)
        {
            Destroy(enemyBtn);
        }
        enemyBtns.Clear();
        foreach (GameObject enemy in EnemysInBattle)
        {
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = (enemyBtns.Count+1).ToString();

            button.EnemyPrefab = enemy;
            newButton.transform.SetParent(EnemySpacer, false);
            enemyBtns.Add(newButton);
        }
    }

    /*public void HeroButtons()
    {
        foreach (GameObject heroBtn in heroBtns)
        {
            Destroy(heroBtn);
        }
        heroBtns.Clear();
        foreach (GameObject hero in HeroesInBattle)
        {
            GameObject newButton = Instantiate(heroButton) as GameObject;
            HeroSelectButton button = newButton.GetComponent<HeroSelectButton>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = (heroBtns.Count + 1).ToString();

            button.HeroPrefab = hero;
            newButton.transform.SetParent(HeroSpacer, false);
            heroBtns.Add(newButton);
        }
    }*/

    public void Input1() //attack button
    {
        HeroChoice.turnType = TurnType.ATTACK;
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttackersGameObject = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.choosenAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0];
        attackButton.GetComponent<Button>().interactable = false;
        magicButton.GetComponent<Button>().interactable = false;
        potionButton.GetComponent<Button>().interactable = false;
        RectTransform selectTransform = EnemySelectPanel.GetComponent<RectTransform>();
        selectTransform.anchorMin = new Vector2(0.182f, 0.023f);
        selectTransform.anchorMax = new Vector2(0.212f, 0.182f);
        EnemySelectPanel.SetActive(true);
    }

    public void Input2(GameObject choosenEnemy) //enemy selection
    {
        HeroChoice.AttackersTarget = choosenEnemy;
        HeroInput = HeroGUI.DONE;
    }

    public void Input3() //switching to magic attacks
    {
        HeroChoice.turnType = TurnType.SPECIAL_ATTACK;
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttackersGameObject = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.choosenAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0];
        attackButton.GetComponent<Button>().interactable = false;
        magicButton.GetComponent<Button>().interactable = false;
        potionButton.GetComponent<Button>().interactable = false;
        RectTransform selectTransform = EnemySelectPanel.GetComponent<RectTransform>();
        selectTransform.anchorMin = new Vector2(0.364f, 0.023f);
        selectTransform.anchorMax = new Vector2(0.394f, 0.182f);
        EnemySelectPanel.SetActive(true);
    }

    public void PotionButtonInput()
    {
        HeroChoice.turnType = TurnType.HEAL;
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttackersGameObject = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroInput = HeroGUI.DONE;
    }

   /* public void HeroSelectInput(GameObject choosenHero) //hero selection
    {
        HeroChoice.AttackersTarget = choosenHero;
        HeroInput = HeroGUI.DONE;
    }*/

    void HeroInputDone()
    {
        PerformList.Add(HeroChoice);
        ClearAttackPanel();
        
        HeroesToManage[0].transform.Find("HeroSelector").gameObject.SetActive(false);
        HeroesToManage.RemoveAt(0);
        HeroInput = HeroGUI.ACTIVATE;
    }

    void ClearAttackPanel()
    {
        EnemySelectPanel.SetActive(false);
       // HeroSelectPanel.SetActive(false);
        attackButton.SetActive(false);
        attackButton.GetComponent<Button>().interactable = true;
        magicButton.SetActive(false);
        magicButton.GetComponent<Button>().interactable = true;
        potionButton.SetActive(false);
        potionButton.GetComponent<Button>().interactable = true;
        potionQuantityImage.SetActive(false);
    }
		


	void activateBattleMap()
	{
		if (theGM.gameData.mapName == "Forest") {

			curBattleMap = BattleMaps.ForestBM;

		}else 	if (theGM.gameData.mapName == "Prison") {

			curBattleMap = BattleMaps.PrisonBM;
		}
		  
		battleCam.SetBGColor (curBattleMap);

		battleMaps [(int)curBattleMap].SetActive (true);

	}

	void deactivateBattleMap()
	{

		battleMaps [(int)curBattleMap].SetActive (false);

	}

	void onDisable()
	{
		deactivateBattleMap ();
	}
}
