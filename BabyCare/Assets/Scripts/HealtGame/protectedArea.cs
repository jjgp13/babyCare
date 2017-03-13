using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class protectedArea : MonoBehaviour {

    public GameObject babyFace;
    public Sprite[] faces;
    public float timeInArea;
    public Text timeText;
    public int enemyCount;
    public GameObject controller;

    private void Start()
    {
        controller = GameObject.Find("gameController");
        timeInArea = 10f;
        timeText.text = timeInArea.ToString();
        enemyCount = 0;
    }

    private void Update()
    {
        if (!controller.GetComponent<controller>().gameOver)
        {
            if (enemyCount == 0)
            {
                if (timeInArea < 10f)
                {
                    int time;
                    timeInArea += Time.deltaTime / 3;
                    time = (int)timeInArea;
                    timeText.text = time.ToString();
                }
            }

            if (timeInArea >= 10f) babyFace.GetComponent<SpriteRenderer>().sprite = faces[0];
            if (timeInArea >= 6f && timeInArea < 10f) babyFace.GetComponent<SpriteRenderer>().sprite = faces[1];
            if (timeInArea >= 3.0f && timeInArea < 6f) babyFace.GetComponent<SpriteRenderer>().sprite = faces[2];
            if (timeInArea >= 0f && timeInArea < 3.0f) babyFace.GetComponent<SpriteRenderer>().sprite = faces[3];

            if (timeInArea >= 5f) timeText.color = Color.black;
            if (timeInArea < 5f) timeText.color = Color.red;

            if (enemyCount > 0)
            {
                int time;
                timeInArea -= Time.deltaTime;
                time = (int)timeInArea;
                timeText.text = time.ToString();
            }
        }      
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            enemyCount++;
        }
    }
}
