using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


	private Shake shake;

	Vector3 myStartPos;
	Vector3 myTargetPos;

	float animSpeed = 0.5f;

	public bool isActive = false;


	// Use this for initialization
	void Start () {


		shake = GameObject.FindGameObjectWithTag ("ScreenShake").GetComponent<Shake> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive) {
			StartCoroutine (TimeForAction());
		}
	}
	public void Launch()
	{

		transform.position = myStartPos;
		isActive = true;
	}

	public void SetStartPos(Vector3 startPos)
	{
		this.myStartPos = new Vector3(startPos.x, startPos.y);
	}

	public void SetTargetPos(Vector3 targetPos)
	{
		this.myTargetPos = new Vector3(targetPos.x, targetPos.y);
	}

	bool MoveTowardsTarget()
	{
		return myTargetPos != (transform.position = Vector3.MoveTowards(transform.position, myTargetPos, animSpeed * Time.deltaTime));
	}

	IEnumerator TimeForAction()
	{
		while (MoveTowardsTarget ()) {
			yield return null;
		}

		shake.CamShake ();

		Destroy(gameObject);//здесь дестрой


	}
}
