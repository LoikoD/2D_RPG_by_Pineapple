using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadsManager : MonoBehaviour {

    public List<GameObject> squads = new List<GameObject>();
    public List<GameObject> heroNpcs = new List<GameObject>();

    public List<GameObject> createdHeroNpcs = new List<GameObject>();

	public QuestManager theQM;

	void Start()
	{
		theQM = FindObjectOfType<QuestManager> ();
	}
		

	public void DeleteEnemySquads(List<int> squadsToDelete)
    {
        foreach (int num in squadsToDelete)
        {
			theQM.enemyKilled = squads [num].name.TrimEnd(new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'});

            squads.RemoveAt(num);

			//squads [num].SetActive (false);

        }
    }

    public void CreateEnemySquads()
    {
        for (int i = 0; i < squads.Count; ++i)
        {
			GameObject NewSquad = Instantiate (squads [i]) as GameObject;
			NewSquad.transform.SetParent (GameObject.Find ("/" + NewSquad.GetComponent<EnemySquadData> ().mapName).transform, false);
			NewSquad.GetComponent<EnemySquadData> ().orderInList = i;
        }
    }

    public void CreateHeroNpcs()
    {
        for (int i = 0; i < heroNpcs.Count; ++i)
        {
            GameObject NewHeroNpc = Instantiate(heroNpcs[i]) as GameObject;
            NewHeroNpc.transform.SetParent(GameObject.Find("/" + NewHeroNpc.GetComponent<HeroNpc>().mapName).transform, false);
            createdHeroNpcs.Add(NewHeroNpc);
        }
    }

    public void DeleteHeroNpcs(List<int> heroNpcsToDelete)
    {
        foreach (int num in heroNpcsToDelete)
        {
            heroNpcs.RemoveAt(num);
        }
    }

    public int FindIndexByNameInHeroNpcs(string heroName)
    {
        for (int i = 0; i < createdHeroNpcs.Count; ++i)
        {
            if (createdHeroNpcs[i].GetComponent<HeroNpc>().heroName == heroName)
            {
                return i;
            }
        }
        Debug.Log("Can't find object with this name.");
        return -1;
    }
}
