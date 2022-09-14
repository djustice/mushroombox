using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gyro : MonoBehaviour
{
    void Start()
    {
	    Input.gyro.enabled = true;
    }

    void Update()
	{
	    Quaternion q = Input.gyro.attitude;
		float moveX = q.x * 100;
		float moveY = q.y * 100;
		moveY = moveY - 25;
		transform.Translate(new Vector3(moveX, moveY, 0));
		if (transform.position.x < 1830)
			transform.position = new Vector3(1830, transform.position.y, 0);
		if (transform.position.x > 1900)
			transform.position = new Vector3(1900, transform.position.y, 0);
		if (transform.position.y > 1640)
			transform.position = new Vector3(transform.position.x, 1640, 0);
		if (transform.position.y < 1550)
			transform.position = new Vector3(transform.position.x, 1550, 0);

    }
}
