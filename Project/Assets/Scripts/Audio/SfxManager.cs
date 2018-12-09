using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour {

    public static bool sfxmExists;

    public AudioSource levelUpSound;
    public AudioSource questCompletedSound;

    // Use this for initialization
    void Start()
    {
        if (!sfxmExists)
        {
            sfxmExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
