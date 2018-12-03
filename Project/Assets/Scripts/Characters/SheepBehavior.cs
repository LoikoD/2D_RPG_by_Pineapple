using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBehavior : MonoBehaviour {

	public Collider2D walkZone;
	Vector2 minZonePoint;
	Vector2 maxZonePoint;
	bool hasWalkZone = false;


	public enum Directions{
		UP,
		DOWN,
		LEFT,
		RIGHT
	}

	Directions MyDir;

	Rigidbody2D rbody;
	Animator anim;

	private int counter;
	private Vector2 movement_vector;

	private int waitingNum;

    public bool canMove;

	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();

		counter = 0;
		movement_vector = new Vector2 ();

        canMove = true;

		if (walkZone != null) {

			hasWalkZone = true;
			minZonePoint = walkZone.bounds.min;
			maxZonePoint = walkZone.bounds.max;
		}
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale != 0)
            Move();
	}

    void Move()
    {

        if (!canMove)
        {
            anim.SetBool("isWalking", false);
            counter = 0;
            return;
        }

        if (counter == 0)
        {
            randomizeDirection();

			if (Random.Range(0,2) == 0)//50% что стоит 50% что идет
            {

                anim.SetBool("isWalking", true);

                anim.SetFloat("input_x", movement_vector.x);
                anim.SetFloat("input_y", movement_vector.y);

                waitingNum = 50;
            }
            else
            {
                anim.SetBool("isWalking", false);

                waitingNum = 100;
            }
        }
			

		if (checkWalkZone() && anim.GetBool("isWalking"))
            rbody.MovePosition(rbody.position + movement_vector * Time.deltaTime);

        counter++;

        if (counter == waitingNum)
        {
            counter = 0;
        }
    }
	bool checkWalkZone()
	{
		switch (MyDir) {
		case Directions.UP:
			if (hasWalkZone && transform.position.y > maxZonePoint.y) {
				anim.SetBool("isWalking", false);
				return false;
			} else {
				return true;
			}
		case Directions.DOWN:
			if (hasWalkZone && transform.position.y < minZonePoint.y) {
				anim.SetBool("isWalking", false);
				return false;
			} else {
				return true;
			}
		case Directions.LEFT:
			if (hasWalkZone && transform.position.x < minZonePoint.x) {
				anim.SetBool("isWalking", false);
				return false;
			} else {
				return true;
			}
		case Directions.RIGHT:
			if (hasWalkZone && transform.position.x > maxZonePoint.x) {
				anim.SetBool("isWalking", false);
				return false;
			} else {
				return true;
			}
		default:
			return true;
		}

	}
	void randomizeDirection()
	{
		MyDir = (Directions)Random.Range (0, 4);


		switch (MyDir) {
		case Directions.UP:
			movement_vector.x = 0f;
			movement_vector.y = 0.1f;
			break;
		case Directions.DOWN:
			movement_vector.x = 0f;
			movement_vector.y = -0.1f;
			break;
		case Directions.LEFT:
			movement_vector.x = -0.1f;
			movement_vector.y = 0f;
			break;
		case Directions.RIGHT:
			movement_vector.x = 0.1f;
			movement_vector.y = 0f;
			break;
		}
			
	}
}
