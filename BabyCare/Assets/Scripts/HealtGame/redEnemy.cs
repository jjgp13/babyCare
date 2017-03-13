using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redEnemy : MonoBehaviour {

    public float speed, xIniPos, random, rangeMov;
    public GameObject explosion;
    public bool infecting;
    int hits;
    float time;

    // Use this for initialization
    void Start()
    {
        speed = 0.3f;
        rangeMov = 1f;
        xIniPos = transform.position.x;
        hits = 3;
        infecting = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * speed);
    }

    void moveBehavior()
    {
        random = Random.Range(-rangeMov, rangeMov);
        transform.position = new Vector2((xIniPos + random), transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(other.gameObject);
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
            moveBehavior();
            speed += 0.3f;
            rangeMov += 0.3f;
            if (hits == 2) gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            if (hits == 1) gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }

        if (other.name == "ProtectedArea")
        {
            infecting = true;
            speed = 0.1f;
        }
    }
}
