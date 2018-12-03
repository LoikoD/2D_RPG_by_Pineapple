using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindsManager : MonoBehaviour {

    private static KeybindsManager instance;
    private MenuController menuController;

    public static KeybindsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindsManager>();
            }

            return instance;
        }
    }

    public Dictionary<string, KeyCode> KeyBinds { get; private set; }

    private string bindName;

	// Use this for initialization
	void Start () {
        KeyBinds = new Dictionary<string, KeyCode>();
        /*
         BindKey("UP", KeyCode.W);
         BindKey("LEFT", KeyCode.A);
         BindKey("DOWN", KeyCode.S);
         BindKey("RIGHT", KeyCode.D);

         BindKey("AttributesWindow", KeyCode.Tab);
         BindKey("Submit", KeyCode.Return);
         BindKey("Cancel", KeyCode.Escape);*/
        int keyCount = GameManager.instance.gameData.keyNames.Count;
        for (int i = 0; i < keyCount; ++i)
        {
            BindKey(GameManager.instance.gameData.keyNames[i], GameManager.instance.gameData.keyCodes[i]);
        }
      //  KeyBinds = GameManager.instance.gameData.keyBinds;
    }
	
    public void BindKey(string key, KeyCode keyBind)
    {
        if (menuController == null)
        {
            menuController = FindObjectOfType<MenuController>();
        }

        Dictionary<string, KeyCode> currentDictionary = KeyBinds;

        if (!currentDictionary.ContainsKey(key))
        {
            currentDictionary.Add(key, keyBind);
            menuController.UpdateKeyText(key, keyBind);
            bindName = string.Empty;
            return;
        }
        else if (currentDictionary.ContainsValue(keyBind))
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            currentDictionary[myKey] = KeyCode.None;
            menuController.UpdateKeyText(myKey, KeyCode.None);
            int myIndex = GameManager.instance.gameData.keyNames.FindIndex(x => x == myKey);
            GameManager.instance.gameData.keyCodes[myIndex] = KeyCode.None;
        }

        currentDictionary[key] = keyBind;
        menuController.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
        int index = GameManager.instance.gameData.keyNames.FindIndex(x => x == key);
        GameManager.instance.gameData.keyCodes[index] = keyBind;


    }

    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }

    private void OnGUI()
    {
        if (bindName != string.Empty)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
