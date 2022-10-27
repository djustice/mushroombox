using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gyro : MonoBehaviour
{
	public float speed = 0.5f;
	public float offsetX;
    public float offsetY;
	public Material mat;

    void Start()
    {
	    Input.gyro.enabled = true;
		mat = GetComponent<Renderer>().material;
    }

    void Update()
	{
        float moveX = (Input.gyro.rotationRateUnbiased.x * 5);
        float moveY = (Input.gyro.rotationRateUnbiased.y * 5);
		offsetX += (Time.deltaTime * speed * moveX) / 10f;
        offsetY += (Time.deltaTime * speed * moveY) / 10f;
		mat.SetTextureOffset("_MainTex", new Vector2(-offsetY, offsetX));

    }
}
