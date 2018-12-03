using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public static bool mmExists;

    public List<AudioSource> musicTracks;
    public int currnetTrack;
    public bool musicCanPlay;

	// Use this for initialization
	void Start () {
		if (!mmExists)
        {
            mmExists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (musicCanPlay)
        {
            if (!musicTracks[currnetTrack].isPlaying)
            {
                musicTracks[currnetTrack].Play();   
            }
        } else
        {
            musicTracks[currnetTrack].Stop();
        }
	}

    public void SwitchTrack(int newTrack)
    {
        musicTracks[currnetTrack].Stop();
        currnetTrack = newTrack;
        musicTracks[currnetTrack].Play();
    }
}
