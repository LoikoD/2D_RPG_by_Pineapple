using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class BattleMenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject settingsMenu;
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider soundSlider;

    enum MenuStates
    {
        MENU,
        SETTINGS,
        NONE
    }

    private MenuStates menuState;

    // Use this for initialization
    void Start()
    {
        menuState = MenuStates.NONE;
        GetVolumes();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeMenuState();
        }
    }

    public void Continue()
    {
        ChangeMenuState();
    }

    public void Settings()
    {
        menu.SetActive(false);
        settingsMenu.SetActive(true);
        menuState = MenuStates.SETTINGS;
    }

    private void ChangeMenuState()
    {
        switch (menuState)
        {
            case MenuStates.NONE:
                Time.timeScale = 0;
                transform.GetComponent<Image>().raycastTarget = true;
                menu.SetActive(true);
                menuState = MenuStates.MENU;
                break;
            case MenuStates.MENU:
                Time.timeScale = 1;
                transform.GetComponent<Image>().raycastTarget = false;
                menu.SetActive(false);
                menuState = MenuStates.NONE;
                break;
            case MenuStates.SETTINGS:
                menu.SetActive(true);
                settingsMenu.SetActive(false);
                menuState = MenuStates.MENU;
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
}
