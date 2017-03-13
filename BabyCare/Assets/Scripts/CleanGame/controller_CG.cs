using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controller_CG : MonoBehaviour {

    public Text leftDrops_txt, leftTime_txt;
    public Image waveBox_UI;
    public Text waveText_UI;
    public Button returnButton_UI;

    public int leftDrops;
    private int enemiesToSpawn;
    private float timeLeft;

    public GameObject[] enemies;
    public GameObject drop;

    public bool gameOver;
    bool gameOverActive, initialStateActive, winStateActive;

	// Use this for initialization
	void Start () {
        StartCoroutine(InitialState());
        gameOver = false;
        leftDrops = Random.Range(10, 30);
        leftDrops_txt.text = leftDrops.ToString();
        enemiesToSpawn = leftDrops + (int)(leftDrops * 0.5f);
        timeLeft = leftDrops * 5;
        spawnDrops(leftDrops);
        spawnEnemies(enemiesToSpawn);
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameOver)
        {
            if (!initialStateActive)
            {
                timeLeft -= Time.deltaTime;
                leftTime_txt.text = timeLeft.ToString("0.0");
                if (timeLeft <= 10.0f) leftTime_txt.color = Color.red;

                leftDrops_txt.text = leftDrops.ToString();

                if (timeLeft <= 0.0f) StartCoroutine(gameOverState());
            }
        }
        else
        {
            if(!gameOverActive) StartCoroutine(gameOverState());
        }

        if (leftDrops <= 0)
            if(!winStateActive)
                StartCoroutine(winState());
    }

    void spawnDrops(int nDrops)
    {
        for (int i = 0; i < nDrops; i++)
        {
            float xPos = Random.Range(-12.1f, 12.1f);
            float yPos = Random.Range(-12.1f, 12.1f);
            while (xPos > -1f && xPos < 1f) xPos = Random.Range(-12.1f, 12.1f);
            while (yPos > -1f && yPos < 1f) yPos = Random.Range(-12.1f, 12.1f);

            Vector2 spawnPos = new Vector2(xPos, yPos);
            Instantiate(drop, spawnPos, Quaternion.identity);
        }
    }

    void spawnEnemies(int nEnemies)
    {
        for (int i = 0; i < nEnemies; i++)
        {
            int enemyType = Random.Range(0, 4);
            float xPos = Random.Range(-12.1f, 12.1f);
            float yPos = Random.Range(-12.1f, 12.1f);
            while(xPos > -1f && xPos < 1f) xPos = Random.Range(-12.1f, 12.1f);
            while (yPos > -1f && yPos < 1f) yPos = Random.Range(-12.1f, 12.1f);

            Vector2 spawnPos = new Vector2(xPos, yPos);
            Instantiate(enemies[enemyType], spawnPos, Quaternion.identity);
        }
    }

    IEnumerator InitialState()
    {
        initialStateActive = true;
        waveBox_UI.gameObject.SetActive(true);
        waveText_UI.text = "Ready?";
        yield return new WaitForSeconds(1);
        waveBox_UI.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        waveBox_UI.gameObject.SetActive(true);
        waveText_UI.text = "Go!";
        yield return new WaitForSeconds(1);
        waveBox_UI.gameObject.SetActive(false);
        initialStateActive = false;
    }

    IEnumerator gameOverState()
    {
        gameOverActive = true;
        waveBox_UI.GetComponent<Image>().color = new Color(1F, 0.0F, 0.0F, 0.5F);
        waveBox_UI.gameObject.SetActive(true);
        waveText_UI.text = "FIN DEL JUEGO";
        yield return new WaitForSeconds(1);
        returnButton_UI.gameObject.SetActive(true);
    }

    IEnumerator winState()
    {
        winStateActive = true;
        waveBox_UI.GetComponent<Image>().color = new Color(0.0F, 0.0F, 1F, 0.5F);
        waveBox_UI.gameObject.SetActive(true);
        waveText_UI.text = "GANASTE!";
        yield return new WaitForSeconds(1);
        returnButton_UI.gameObject.SetActive(true);
    }
}
