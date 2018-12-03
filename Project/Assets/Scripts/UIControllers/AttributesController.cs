using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributesController : MonoBehaviour {

    public GameObject attributesWindow;
    public Text pointsText;
    public XpPanelController xpPanelController;
    public List<Button> plusButtons = new List<Button>();
    public List<Button> minusButtons = new List<Button>();
    public List<Text> attributesText = new List<Text>();

    private List<int> attributes = new List<int>();
    private List<int> oldAttributes = new List<int>();
    private int points;
    public bool menu;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeybindsManager.Instance.KeyBinds["AttributesWindow"]) && !menu)
        {
            ChangeWindowState();
        }
	}

    public void ChangeWindowState()
    {
        if (!attributesWindow.activeSelf)
        {
            UpdateInfo();
            foreach (Button minusBtn in minusButtons)
            {
                minusBtn.interactable = false;
            }
            UpdateWindow();
        }
        GameManager.instance.playerCharacter.GetComponent<PlayerMovement>().canMove = attributesWindow.activeSelf;
        attributesWindow.SetActive(!attributesWindow.activeSelf);
    }

    public void PlusButton(int statIndex)
    {
        attributes[statIndex]++;
        points--;
        UpdateWindow();
        minusButtons[statIndex].interactable = true;
    }

    public void MinusButton(int statIndex)
    {
        attributes[statIndex]--;
        points++;
        UpdateWindow();
        if(attributes[statIndex] == oldAttributes[statIndex])
            minusButtons[statIndex].interactable = false;
    }

    public void ConfirmButton()
    {
        if (points == 0)
        {
            xpPanelController.StopBlinking();
        }
        GameManager.instance.gameData.unusedAttributesPoints = points;
        GameManager.instance.gameData.attributes.Clear();
        foreach (int attributeAmount in attributes)
        {
            GameManager.instance.gameData.attributes.Add(attributeAmount);
        }
        ChangeWindowState();
    }

    private void UpdateInfo()
    {
        points = GameManager.instance.gameData.unusedAttributesPoints;
        attributes.Clear();
        foreach (int attributeAmount in GameManager.instance.gameData.attributes)
        {
            attributes.Add(attributeAmount);
        }
        oldAttributes = GameManager.instance.gameData.attributes;
    }

    private void UpdateWindow()
    {
        SetPointsText(points);
        if (points > 0)
        {
            foreach (Button plusBtn in plusButtons)
            {
                plusBtn.interactable = true;
            }
        }
        else
        {
            foreach (Button plusBtn in plusButtons)
            {
                plusBtn.interactable = false;
            }
        }
        for (int i = 0; i < attributes.Count; ++i)
        {
            attributesText[i].text = attributes[i].ToString();
        }
    }

    private void SetPointsText(int points)
    {
        if (points == 0)
        {
            pointsText.text = "У вас нет непотраченных очков";
            return;
        }
        int remaining = points % 100;
        if (remaining >= 11 && remaining <= 19)
        {
            pointsText.text = "У вас " + points + " непотраченных очков";
        }
        else
        {
            remaining = remaining % 10;
            switch (remaining)
            {
                case (1):
                    pointsText.text = "У вас " + points + " непотраченное очко";
                    break;
                case (2):

                case (3):

                case (4):
                    pointsText.text = "У вас " + points + " непотраченных очка";
                    break;
                default:
                    pointsText.text = "У вас " + points + " непотраченных очков";
                    break;
            }
        }
    }
}
