using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greenEnemy : MonoBehaviour {

    public float speed, yPos, xPos;
    public GameObject explosion;
    public bool infecting;

    // Use this for initialization
    void Start()
    {
        speed = 2f;
        infecting = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GameObject.Find("gameController").GetComponent<controller>().enemyInField--;
            GameObject.Find("gameController").GetComponent<controller>().enemytxtN--;
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(other.gameObject);
            if (infecting) GameObject.Find("ProtectedArea").GetComponent<protectedArea>().enemyCount--;
        }

        if (other.name == "ProtectedArea")
        {
            infecting = true;
            speed = 0.1f;
        }
    }
}
