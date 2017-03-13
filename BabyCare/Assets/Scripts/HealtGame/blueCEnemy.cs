using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blueCEnemy : MonoBehaviour {

    public float speedY, speedX, amplitud, xPos, xIniPos;

    public GameObject explosion;
    public bool infecting;
    int hits;
    float time;

    // Use this for initialization
    void Start()
    {
        speedY = 0.3f;
        speedX = 3f;
        amplitud = 1.5f;

        infecting = false;
        hits = 1;
        xIniPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * speedY);

        time += Time.deltaTime;
        xPos = Mathf.Sin(time*speedX);
        transform.position = new Vector2((xPos*amplitud) + xIniPos, transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            hits--;
            if (hits == 0)
            {
                GameObject.Find("gameController").GetComponent<controller>().enemyInField--;
                GameObject.Find("gameController").GetComponent<controller>().enemytxtN--;
                Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(other.gameObject);
                if (infecting) GameObject.Find("ProtectedArea").GetComponent<protectedArea>().enemyCount--;
            }
        }

        if (other.name == "ProtectedArea")
        {
            infecting = true;
            speedY = 0.1f;
        }
    }
}
