using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneOnClick : MonoBehaviour {
    
    public LoadOptions loadOption;

	public void LoadByIndex(int sceneIndex)
	{
		SceneManager.LoadScene (sceneIndex);
	}

	public void FromMenuFlag()
	{
        GameManager.instance.FromMenu(loadOption);
    }

    public void ToMenuFlag(bool save)
    {
        if (save)
        {
            GameManager.instance.SaveToFile();
        }
        GameManager.instance.ToMenu();
    }

		
}
