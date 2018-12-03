using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour {

    Rigidbody2D rbody;
    
    // Use this for initialization
	void Start () {

        rbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


        rbody.MovePosition(rbody.position + movementVector * Time.deltaTime);
	}
}
