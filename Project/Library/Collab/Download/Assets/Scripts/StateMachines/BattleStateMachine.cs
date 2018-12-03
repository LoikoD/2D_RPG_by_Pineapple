using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour {

    public enum PerformAction
    {
        WAIT,
        TAKE_ACTION,
        PERFORM_ACTION,
        CHECK_ALIVE,
        WIN,
        LOSE
    }

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
    public Transform Spacer;

    public GameObject EnemySelectPanel;

    //attacks of heroes
    public GameObject magicButton;
    public GameObject attackButton;

    //enemy buttons
    private List<GameObject> enemyBtns = new List<GameObject>();

    public int createdHeroBars;
    public int createdEnemyBars;

    public List<Transform> enemyPositions = new List<Transform>();

    void Awake()
    {
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
        HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        HeroInput = HeroGUI.ACTIVATE;
        attackButton.SetActive(false);
        EnemySelectPanel.SetActive(false);
        magicButton.SetActive(false);

        EnemyButtons();
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
                    HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                    HSM.currentState = HeroStateMachine.TurnState.ACTION;
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
                }
                break;
            case (PerformAction.WIN):
                for (int i = 0; i < HeroesInBattle.Count; ++i)
                {
                    HeroesInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.WAITING;
                }
                GameManager.instance.LoadSceneAfterBattle();
                GameManager.instance.enemiesToBattle.Clear();
                break;
            case (PerformAction.LOSE):

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
                    if (HeroesToManage[0].GetComponent<HeroStateMachine>().hero.magicAttacks.Count > 0)
                    {
                        magicButton.SetActive(true);
                    }
                    // CreateAttackButtons();
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
            newButton.transform.SetParent(Spacer, false);
            enemyBtns.Add(newButton);
        }
    }

    public void Input1() //attack button
    {
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttackersGameObject = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.choosenAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0];
        attackButton.GetComponent<Button>().interactable = false;
        magicButton.GetComponent<Button>().interactable = false;
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
        attackButton.SetActive(false);
        attackButton.GetComponent<Button>().interactable = true;
        magicButton.SetActive(false);
        magicButton.GetComponent<Button>().interactable = true;
    }

    /*void CreateAttackButtons()
    {
        if (HeroesToManage[0].GetComponent<HeroStateMachine>().hero.magicAttacks.Count > 0)
        {
            foreach (BaseAttack magicAtk in HeroesToManage[0].GetComponent<HeroStateMachine>().hero.magicAttacks)
            {
                GameObject MagicButton = Instantiate(magicButton) as GameObject;
                Text MagicButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent<Text>();
                MagicButtonText.text = magicAtk.attackName;
                AttackButton ATB = MagicButton.GetComponent<AttackButton>();
                ATB.magicAttackToPerform = magicAtk;
                MagicButton.transform.SetParent(magicSpacer, false);
                atkBtns.Add(MagicButton);
            }
        }
        else
        {
            MagicAttackButton.GetComponent<Button>().interactable = false;
        }
    }*/

    /*public void Input4(BaseAttack choosenMagic) //choosen magic attack
    {
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttackersGameObject = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.choosenAttack = choosenMagic;
        MagicPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }*/

    public void Input3() //switching to magic attacks
    {
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttackersGameObject = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.choosenAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.magicAttacks[0];
        attackButton.GetComponent<Button>().interactable = false;
        magicButton.GetComponent<Button>().interactable = false;
        RectTransform selectTransform = EnemySelectPanel.GetComponent<RectTransform>();
        selectTransform.anchorMin = new Vector2(0.364f, 0.023f);
        selectTransform.anchorMax = new Vector2(0.394f, 0.182f);
        EnemySelectPanel.SetActive(true);
    }
}
