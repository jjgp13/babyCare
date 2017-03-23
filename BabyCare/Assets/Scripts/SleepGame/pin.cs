using Assets.SimpleAndroidNotifications;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pin : MonoBehaviour {

    private bool isPin = false;

    public float speed = 30f;
    public Rigidbody2D rb;


    private void Start()
    {
        int randomColor = Random.Range(0, 8);
        switch (randomColor)
        {
            case 0:
                GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case 1:
                GetComponent<SpriteRenderer>().color = Color.gray;
                break;
            case 2:
                GetComponent<SpriteRenderer>().color = Color.cyan;
                break;
            case 3:
                GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case 4:
                GetComponent<SpriteRenderer>().color = Color.magenta;
                break;
            case 5:
                GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case 6:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case 7:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            default:
                break;
        }
    }


    private void FixedUpdate()
    {
        if (!isPin)
        {
            rb.MovePosition(rb.position + Vector2.up * speed * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Rotator")
        {
            GetComponent<AudioSource>().Play();
            //Aumentar la velocidad o cambiar de rotacion
            GameControllerSG.timeToPin = 5f;
            int rand = Random.Range(1, 3);
            if (rand == 1) FindObjectOfType<circleBehavior>().speed *= -1;
            else
            {
                if(FindObjectOfType<circleBehavior>().speed < 0) FindObjectOfType<circleBehavior>().speed -= 10f;
                else FindObjectOfType<circleBehavior>().speed += 10f;
            }

            GameControllerSG.pinNumberLeft--;

            transform.SetParent(other.transform);
            isPin = true;
            Spawner.pin = false;
        }

        if(other.tag == "Pin")
        {
            FindObjectOfType<GameControllerSG>().endGame();
        }
    }
}
