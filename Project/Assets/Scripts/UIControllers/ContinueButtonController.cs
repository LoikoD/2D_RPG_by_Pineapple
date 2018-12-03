using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonController : MonoBehaviour {
    
    private bool hasSaveFile;

	// Use this for initialization
	void Start () {
        hasSaveFile = GameManager.instance.HasSaveFile;
    }
	
	// Update is called once per frame
	void Update () {
        if (hasSaveFile)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
