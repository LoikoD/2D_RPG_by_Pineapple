using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject settingsMenu;
    public GameObject keyBindsMenu;
    public AttributesController attributesController;
    public QuestsWindowController questsWindowController;
    public Button loadButton;
    public MessageController messageController;
    public AudioMixer audioMixer;
    public GameObject[] keybindButtons;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider soundSlider;
    private bool inputEnabled;

    enum MenuStates
    {
        MENU,
        SETTINGS,
        KEY_BINDS,
        NONE
    }

    private MenuStates menuState;
    
    // Use this for initialization
    void Start()
    {
        inputEnabled = true;
        menuState = MenuStates.NONE;
        GetVolumes();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputEnabled)
        {
            if (Input.GetKeyDown(KeybindsManager.Instance.KeyBinds["Cancel"]))
            {
                ChangeMenuState();
            }
        }
    }

    public void Continue()
    {
        ChangeMenuState();
    }

    public void Save()
    {
        GameManager.instance.SaveToFile();
        ChangeMenuState();
    }

    public void Load()
    {
        ChangeMenuState();
        SceneManager.LoadScene("TempScene");
        GameManager.instance.reloadStage = "temp";
        GameManager.instance.gameState = GameManager.GameStates.RELOADING_WORLD;
    }

    public void Settings()
    {
        menu.SetActive(false);
        settingsMenu.SetActive(true);
        menuState = MenuStates.SETTINGS;
    }

    public void KeyBinds()
    {
        settingsMenu.SetActive(false);
        keyBindsMenu.SetActive(true);
        menuState = MenuStates.KEY_BINDS;
    }

    public void MainMenu()
    {
        ChangeMenuState();
        messageController.Show(MessageController.MessageModes.SAVE);
    }

    private void ChangeMenuState()
    {
        switch (menuState)
        {
            case MenuStates.NONE:
                Time.timeScale = 0;
                transform.GetComponent<Image>().raycastTarget = true;
                GameManager.instance.playerCharacter.GetComponent<PlayerMovement>().canMove = false;
                if (GameManager.instance.HasSaveFile)
                {
                    loadButton.interactable = true;
                }
                else
                {
                    loadButton.interactable = false;
                }
                menu.SetActive(true);
                questsWindowController.menu = true;
                attributesController.menu = true;
                menuState = MenuStates.MENU;
                break;
            case MenuStates.MENU:
                Time.timeScale = 1;
                transform.GetComponent<Image>().raycastTarget = false;
                GameManager.instance.playerCharacter.GetComponent<PlayerMovement>().canMove = true;
                menu.SetActive(false);
                attributesController.menu = false;
                questsWindowController.menu = false;
                menuState = MenuStates.NONE;
                break;
            case MenuStates.SETTINGS:
                if (GameManager.instance.HasSaveFile)
                {
                    loadButton.interactable = true;
                }
                else
                {
                    loadButton.interactable = false;
                }
                menu.SetActive(true);
                settingsMenu.SetActive(false);
                menuState = MenuStates.MENU;
                break;
            case MenuStates.KEY_BINDS:
                settingsMenu.SetActive(true);
                keyBindsMenu.SetActive(false);
                menuState = MenuStates.SETTINGS;
                break;
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("soundVolume", volume);
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    private void GetVolumes()
    {
        float temp;
        audioMixer.GetFloat("masterVolume", out temp);
        masterSlider.value = temp;
        audioMixer.GetFloat("musicVolume", out temp);
        musicSlider.value = temp;
        audioMixer.GetFloat("soundVolume", out temp);
        soundSlider.value = temp;
    }

    public void DisableInput()
    {
        attributesController.menu = true;
        questsWindowController.menu = true;
        inputEnabled = false;

    }

    public void EnableInput()
    {
        attributesController.menu = false;
        questsWindowController.menu = false;
        inputEnabled = true;
    }
}
