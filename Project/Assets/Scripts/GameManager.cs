using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Intro intro;
    public GameData gameData = new GameData();

    public static GameManager instance;

    //HERO
    public GameObject playerCharacter;

    //POSITIONS
    public Vector2 nextHeroPosition;
    public Vector2 lastHeroPosition;

    //SCENES
    public string sceneToLoad;
    public string lastScene;
    public string reloadStage;


    //ENUM
    public enum GameStates
    {
		MAIN_MENU_STATE,
		AFTER_MENU,
        WORLD_STATE,
        BATTLE_STATE,
        IDLE,
        AFTER_BATTLE,
        RELOADING_WORLD,
        INTRO
    }
    public GameStates gameState;

    // BATTLE
    public SquadsManager squadsManager;
    public EnemySquadData curSquad;
    private int squadNumber;
    public int enemyAmount;
    public List<GameObject> enemiesToBattle = new List<GameObject>();
    public List<GameObject> heroesToBattle = new List<GameObject>();
    public List<GameObject> heroesPrefabs = new List<GameObject>();

    //QUESTS
    public QuestManager questManager;

    //LOAD
    public bool HasSaveFile
    {
        get
        {
            return File.Exists(Application.persistentDataPath + "/lastSave.dat");
        }
    }

    //XP
    public XpPanelController xpPanelController;

    private SfxManager sfxManager;

    void Awake () {
		//check if instance exist
        if (instance == null)
        {
            //if not set the instance to this
            instance = this;
            nextHeroPosition = new Vector2(25.3f, -3.3f);
           // CreateEnemySquads();
        }
        //if it exists but is not this instance
        else if (instance != this)
        {
            //destroy it
            Destroy(gameObject);
        }
        //set this to be not destroyable
        DontDestroyOnLoad(gameObject);
        /*if (!GameObject.Find("Player"))
        {
            playerCharacter = Instantiate(playerCharacter, nextHeroPosition, Quaternion.identity) as GameObject;
            playerCharacter.name = "Player";
        }*/
	}

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Main_menu")
        {
            gameState = GameStates.MAIN_MENU_STATE;
        }
        else if (SceneManager.GetActiveScene().name == "WorldScene")
        {
           /* squadsManager = GameObject.Find("SquadsManager").GetComponent<SquadsManager>();
            squadsManager.DeleteEnemySquads(gameData.defeatedSquads);
            squadsManager.CreateEnemySquads();

            squadsManager.DeleteHeroNpcs(gameData.joinedHeroNpcs);
            squadsManager.CreateHeroNpcs();*/

            gameState = GameStates.INTRO;
        }
        else
        {
            Debug.Log("GameManagerStartError");
        }
    }

    void Update()
    {
        switch (gameState)
        {
            case (GameStates.AFTER_MENU):
                if (CheckSceneLoad(lastScene))
                {
                    LoadWorld();

                    gameState = GameStates.WORLD_STATE;
                }

                break;
            case (GameStates.MAIN_MENU_STATE):

                    break;
		    case (GameStates.WORLD_STATE):

                    break;
            case (GameStates.BATTLE_STATE):
                StartBattle();
                gameState = GameStates.IDLE;
                break;
            case (GameStates.IDLE):
                
                break;
            case (GameStates.AFTER_BATTLE):
                if (CheckSceneLoad(lastScene))
                {

                    LoadWorld();
                    PotionDrop();
                    XpGain(gameData.potentialXp);

                    gameState = GameStates.WORLD_STATE;
                }
                break;
            case (GameStates.RELOADING_WORLD):
                if (reloadStage == "temp")
                {
                    if (CheckSceneLoad("TempScene"))
                    {
                        reloadStage = "main";
                        SceneManager.LoadScene("WorldScene");
                    }
                }
                else if (reloadStage == "main")
                {
                    if (CheckSceneLoad("WorldScene"))
                    {
                        FromMenu(LoadOptions.LOAD_GAME);
                    }
                }
                break;
            case (GameStates.INTRO):
                if (CheckSceneLoad(lastScene))
                {
                    LoadWorld();
                    intro.StartIntro();
                    gameState = GameStates.WORLD_STATE;
                }
                break;
        }
    }

    void LoadWorld()
    {
        Time.timeScale = 1;
        playerCharacter = GameObject.FindGameObjectWithTag("Player");
        playerCharacter.transform.position = new Vector2(gameData.heroPositionX, gameData.heroPositionY);
        squadsManager = GameObject.Find("SquadsManager").GetComponent<SquadsManager>();
        squadsManager.DeleteEnemySquads(gameData.defeatedSquads);
        squadsManager.CreateEnemySquads();

        squadsManager.DeleteHeroNpcs(gameData.joinedHeroNpcs);
        squadsManager.CreateHeroNpcs();

        intro = GameObject.Find("IntroManager").GetComponent<Intro>();

        questManager = GameObject.Find("Quest Manager").GetComponent<QuestManager>();
        questManager.ActivateQuestsFromList(gameData.activeQuests);
        questManager.completedQuests = gameData.completedQuests;
        questManager.SetEnemiesKillCount(gameData.enemiesKillCountList);

        xpPanelController = FindObjectOfType<XpPanelController>();
        xpPanelController.UpdateXpBar();

        Camera.main.transform.position = GameObject.Find("Player").transform.position;
        BoxCollider2D bounds = GameObject.Find(gameData.mapName + "/Bounds").GetComponent<BoxCollider2D>();
        Camera.main.GetComponent<CameraFolow>().setNewBounds(bounds);
    }

    void StartBattle()
    {

        SaveGameData();
        gameData.mapName = curSquad.mapName;
        gameData.potentialXp = curSquad.xpGain;

        heroesToBattle.Clear();
        for (int i = 0; i < heroesPrefabs.Count; ++i)
        {
            if (gameData.heroSquad[i])
            {
                heroesToBattle.Add(heroesPrefabs[i]);
            }
        }

        enemyAmount = curSquad.enemies.Count;
        squadNumber = curSquad.orderInList;
        enemiesToBattle.Clear();
        for (int i = 0; i < enemyAmount; ++i)
        {
            enemiesToBattle.Add(curSquad.enemies[i]);
        }
        lastHeroPosition = GameObject.Find("Player").gameObject.transform.position;
        nextHeroPosition = lastHeroPosition;
        lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(curSquad.BattleScene);
    }
    
    public void LoadSceneAfterBattle(bool win)
    {
        if (win)
        {
            SceneManager.LoadScene(lastScene);
            gameData.defeatedSquads.Add(squadNumber);
            gameState = GameStates.AFTER_BATTLE;
        }
        else
        {
            SceneManager.LoadScene("Main_menu");
            gameState = GameStates.MAIN_MENU_STATE;
        }
    }
    
    bool CheckSceneLoad(string sceneName)
    {
         if (SceneManager.GetActiveScene().name == sceneName)
         {
             return true;
         }
         return false;
    }

	public void ToMenu()
    {
        //lastScene = SceneManager.GetActiveScene().name;
        gameState = GameStates.MAIN_MENU_STATE;
	}

	public void FromMenu(LoadOptions loadOption)// from start
	{
        switch(loadOption)
        {
            case LoadOptions.NEW_GAME:
                gameData.NewGame();
                gameState = GameStates.INTRO;
                break;
            case LoadOptions.LOAD_GAME:
                LoadGameDataFromFile();
                gameState = GameStates.AFTER_MENU;
                break;
        }

    }

    public void SaveToFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/lastSave.dat"))
        {
            file = File.OpenWrite(Application.persistentDataPath + "/lastSave.dat");
        } else
        {
            file = File.Create(Application.persistentDataPath + "/lastSave.dat");
        }

        SaveGameData();

        bf.Serialize(file, gameData);
        file.Close();

    }

    public void SaveGameData()
    {
        gameData.activeQuests = questManager.GetListOfActiveQuests();
        gameData.completedQuests = questManager.completedQuests;
        gameData.enemiesKillCountList = questManager.EnemiesCountList();
        gameData.heroPositionX = playerCharacter.transform.position.x;
        gameData.heroPositionY = playerCharacter.transform.position.y;
    }

    public void LoadGameDataFromFile()
    {
        if (File.Exists(Application.persistentDataPath + "/lastSave.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/lastSave.dat", FileMode.Open);

            gameData = (GameData)bf.Deserialize(file);
            file.Close();

        }
        else
        {
            Debug.Log("Cant find save file!");
            D
        }
    }

    private void PotionDrop()
    {
        int potionDrop = Random.Range(0, 3);
        if (potionDrop > 0)
        {
            MessageController messageController = FindObjectOfType<MessageController>();
            messageController.Show(MessageController.MessageModes.DROP, potionDrop);
            gameData.potionQuantity += potionDrop;
        }
    }

    public void XpGain(int xpAmount)
    {
        if (sfxManager == null)
            sfxManager = FindObjectOfType<SfxManager>();
        gameData.currentXp += xpAmount;
        while (gameData.currentXp >= gameData.maxXp)
        {
            gameData.currentXp = gameData.currentXp - gameData.maxXp;
            gameData.unusedAttributesPoints += 5;
            sfxManager.levelUpSound.PlayDelayed(0.5f);
            xpPanelController.StartBlinking();
        }
        xpPanelController.UpdateXpBar();

    }
}