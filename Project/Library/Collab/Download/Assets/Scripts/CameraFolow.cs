using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFolow : MonoBehaviour {

    public Transform target;
    public float movement_speed = 0.1f;
    Camera myCam;


	public BoxCollider2D boundBox;
	private Vector3 minBounds;
	private Vector3 maxBounds;


	private float halfHeight;
	private float halfWidth;
	// Use this for initialization
	void Start () {
        myCam = GetComponent<Camera>();

		minBounds = boundBox.bounds.min;
		maxBounds = boundBox.bounds.max;


		halfHeight = myCam.orthographicSize;
		halfWidth = halfHeight * Screen.width / Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
        myCam.orthographicSize = (Screen.height / 100f) / 4f;


        if(target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, movement_speed) + new Vector3(0, 0, -10);
        }

		float clampedX = Mathf.Clamp (transform.position.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
		float clampedY = Mathf.Clamp (transform.position.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

		transform.position = new Vector3(clampedX, clampedY, transform.position.z);
	}

	public void setNewBounds(BoxCollider2D newBounds)
	{
		boundBox = newBounds;

		minBounds = boundBox.bounds.min;
		maxBounds = boundBox.bounds.max;
	}
}
