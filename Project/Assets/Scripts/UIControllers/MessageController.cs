using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour {

    private GameObject message;
    private Text messageText;
    private GameObject confirmButton;
    private GameObject declineButton;

	void Start () {
        message = transform.Find("Message").gameObject;
        messageText = message.transform.Find("Text").GetComponent<Text>();
        confirmButton = message.transform.Find("ConfirmButton").gameObject;
        declineButton = message.transform.Find("DeclineButton").gameObject;
    }
	
	void Update () {
        if (message.activeSelf && (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel")))
        {
            Hide();
        }
	}

    public void Show(MessageModes messageMode, int potionQuantity = 0)
    {
        switch (messageMode)
        {
            case (MessageModes.DROP):
                messageText.text = "Вы нашли " + potionQuantity.ToString() + " x Health Potion";
                confirmButton.SetActive(false);
                declineButton.SetActive(false);
                break;
            case (MessageModes.SAVE):
                messageText.text = "Хотите сохранить игру?";
                confirmButton.SetActive(true);
                declineButton.SetActive(true);
                break;
        }
        message.SetActive(true);
        GameManager.instance.playerCharacter.GetComponent<PlayerMovement>().canMove = false;

    }

    private void Hide()
    {
        GameManager.instance.playerCharacter.GetComponent<PlayerMovement>().canMove = true;
        message.SetActive(false);
        confirmButton.SetActive(false);
        declineButton.SetActive(false);
    }

    public enum MessageModes
    {
        DROP,
        SAVE
    }
}
