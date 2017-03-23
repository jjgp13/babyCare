using Assets.SimpleAndroidNotifications;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spaceShip : MonoBehaviour {
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}
    public float speed;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;
    public GameObject controller;

    private void Start()
    {
        controller = GameObject.Find("gameController");
    }

    void Update()
    {
        if (!controller.GetComponent<controller>().gameOver)
        {
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            }
        }
    }

    void FixedUpdate()
    {
            Vector2 movement = new Vector2(Input.acceleration.x, Input.acceleration.y * 2);
            GetComponent<Rigidbody2D>().velocity = movement * speed;

            GetComponent<Rigidbody2D>().position = new Vector2
            (
                Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax),
                Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.yMin, boundary.yMax)
            );
        
    }
}
