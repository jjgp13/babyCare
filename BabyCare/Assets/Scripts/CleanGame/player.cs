using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

    public float speed;       
    private Rigidbody2D rb2d;
    GameObject controller;
    public GameObject explosion;
    public AudioClip pickUpSound;
    public AudioSource aud;
    
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        controller = GameObject.Find("gameController");
        aud = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (!controller.GetComponent<controller_CG>().gameOver)
        {
            float moveHorizontal = Input.acceleration.x;
            float moveVertical = Input.acceleration.y;
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            rb2d.AddForce(movement * speed);
        }
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Drop"))
        {
            Destroy(other.gameObject);
            controller.GetComponent<controller_CG>().leftDrops--;
            aud.clip = pickUpSound;
            aud.Play();
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            GameObject.Find("Camera").GetComponent<camera>().enabled = false;
            Destroy(gameObject);
            controller.GetComponent<controller_CG>().gameOver = true;
        }
    }
}
