using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Rigidbody2D rbody;
    Animator anim;
    public List<AudioSource> stepSounds = new List<AudioSource>();

    public bool canMove;
    private int stepSoundsCount;

	// Use this for initialization
	void Start () {
        transform.position = GameManager.instance.nextHeroPosition;
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMove = true;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    void Move()
    {
        if (!canMove)
        {
            anim.SetBool("isWalking", false);
            return;
        }

        Vector2 movement_vector = new Vector2();
        if (Input.GetKey(KeybindsManager.Instance.KeyBinds["LEFT"]))
        {
            movement_vector += Vector2.left;
        }
        if (Input.GetKey(KeybindsManager.Instance.KeyBinds["RIGHT"]))
        {
            movement_vector += Vector2.right;
        }
        if (Input.GetKey(KeybindsManager.Instance.KeyBinds["UP"]))
        {
            movement_vector += Vector2.up;
        }
        if (Input.GetKey(KeybindsManager.Instance.KeyBinds["DOWN"]))
        {
            movement_vector += Vector2.down;
        }
        
        if (movement_vector != Vector2.zero)
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("input_x", movement_vector.x);
            anim.SetFloat("input_y", movement_vector.y);
            if (!stepSounds[stepSoundsCount].isPlaying)
            {
                stepSoundsCount++;
                if (stepSoundsCount == stepSounds.Count)
                {
                    stepSoundsCount = 0;
                }
                stepSounds[stepSoundsCount].Play();
            }
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        if ((movement_vector.x != 0) && (movement_vector.y != 0))
        {
            rbody.MovePosition(rbody.position + movement_vector * Time.deltaTime * 0.7f);
        }
        else
        {

            rbody.MovePosition(rbody.position + movement_vector * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "BattleTrigger")
        {
            GameManager.instance.curSquad = other.gameObject.GetComponent<EnemySquadData>();
            GameManager.instance.gameState = GameManager.GameStates.BATTLE_STATE;
        }
    }
}
