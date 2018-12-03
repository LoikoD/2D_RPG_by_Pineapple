using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleAnimationManager : MonoBehaviour {

	enum KindOfAnimation
	{
		DEATH,
		HURT,
		ATTACK
	}

	//идея передавать сюда сигналы для анимации
	//которые будут попадать в очередь с приоритетом
	//в апдейте будем очищать очередь и выполнять анимации
	Animator animator;
	Animation[] anim;
	// Use this for initialization

	PriorityQueque thePQ;

	void Start () {


		//анимацию получаем

		// так же нужно инициализировать аниматор
		animator = GetComponent<Animator>();

		//как получить доступ к времени моушонов?



		//нужно инициализировать float переменные аниматора временем на анимации
		animator.SetFloat("timeToAttack", 1f);
		animator.SetFloat("timeToTurnDead",1f);
		animator.SetFloat("timeToBeHurted",1f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}




	IEnumerator attackAnimation()
	{
		animator.SetBool ("isAttacking", true);

		//wait a bit
		yield return new WaitForSeconds(1f);

		animator.SetBool ("isAttacking", false);
	}

	IEnumerator hurtAnimation()
	{
		animator.SetBool ("isHurted", true);

		//wait a bit
		yield return new WaitForSeconds(1f);

		animator.SetBool ("isHurted", false);
	}

	IEnumerator deadAnimation()
	{
		animator.SetBool ("isDead", true);

		//wait a bit
		yield return new WaitForSeconds(1f);

	}
}
