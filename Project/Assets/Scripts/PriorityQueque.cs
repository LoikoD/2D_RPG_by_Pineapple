using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueque : MonoBehaviour {
	//идея
	//есть класс перечислитель с некоторым числом эл-тов
	//есть эл-т терминатор
	//эл-ты с приоритетом выше должны выполняться раньше

	Queue thePQ;
	// Use this for initialization
	void Start () {
		thePQ = new Queue ();
	}



	// Update is called once per frame
	void Update () {
		
	}
}
