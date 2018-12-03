using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpPanelController : MonoBehaviour {

    public GameObject xpBar;
    public Text xpText;
    public Image attributesPlus;
    public GameObject attributesButton;

    private bool isBlinking;
    private bool blinked;
    private int curXp;
    private int maxXp;
    private float calcXp;

	// Use this for initialization
	void Start () {
        if (GameManager.instance.gameData.unusedAttributesPoints > 0)
        {
            StartBlinking();
        }
        else
        {
            attributesButton.SetActive(false);
        }
        UpdateXpBar();

    }
	
	// Update is called once per frame
	void Update () {

	}

    public void UpdateXpBar()
    {
        maxXp = GameManager.instance.gameData.maxXp;
        curXp = GameManager.instance.gameData.currentXp;
        xpText.text = curXp + "/" + maxXp;
        calcXp = (float)curXp / maxXp;
        xpBar.transform.localScale = new Vector3(Mathf.Clamp(calcXp, 0, 1), xpBar.transform.localScale.y, xpBar.transform.localScale.z);
    }


    private void ToggleButtonState()
    {
        if (!blinked)
        {
            blinked = true;
            attributesPlus.color = new Color(0.42f, 0.427f, 0.31f);
        }
        else
        {
            blinked = false;
            attributesPlus.color = new Color(0.941f, 0.957f, 0.702f);
        }
    }

    public void StartBlinking()
    {
        if (isBlinking)
            return;

        if (attributesPlus != null)
        {
            attributesButton.SetActive(true);
            isBlinking = true;
            InvokeRepeating("ToggleButtonState", 1f, 0.7f);
        }
    }

    public void StopBlinking()
    {
        CancelInvoke("ToggleButtonState");
        isBlinking = false;
        attributesPlus.color = new Color(0.941f, 0.957f, 0.702f);
        attributesButton.SetActive(false);
    }
}
