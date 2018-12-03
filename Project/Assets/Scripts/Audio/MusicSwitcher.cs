using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitcher : MonoBehaviour {

    private MusicManager musicManager;

    public int newTrack;
    public bool switchOnStart;

	// Use this for initialization
	void Start () {
        musicManager = FindObjectOfType<MusicManager>();

        if(switchOnStart)
        {
            musicManager.SwitchTrack(newTrack);
            gameObject.SetActive(false);

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            musicManager.SwitchTrack(newTrack);
            gameObject.SetActive(false);
        }
    }
}
