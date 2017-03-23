using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets.SimpleAndroidNotifications
{
    public class controller_CG : MonoBehaviour
    {

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

        public Button aceptarInsBtn;
        public GameObject panel;
        public static bool playerActive;
        public GameObject player;

        // Use this for initialization
        void Start()
        {
            playerActive = false;
            initialStateActive = true;
            leftDrops = UnityEngine.Random.Range(10, 30);
            leftDrops_txt.text = leftDrops.ToString();
            enemiesToSpawn = leftDrops + (int)(leftDrops * 0.5f);
            timeLeft = leftDrops * 12;
            spawnDrops(leftDrops);
            spawnEnemies(enemiesToSpawn);
            aceptarInsBtn.onClick.AddListener(delegate {
                StartCoroutine(InitialState());
                panel.SetActive(false);
            });
        }

        // Update is called once per frame
        void Update()
        {
            if (!gameOver)
            {
                if (!initialStateActive)
                {
                    timeLeft -= Time.deltaTime;
                    leftTime_txt.text = timeLeft.ToString("0.0");
                    if (timeLeft <= 10.0f) leftTime_txt.color = Color.red;

                    leftDrops_txt.text = leftDrops.ToString();

                    if(timeLeft <= 0.0f || player == null) StartCoroutine(gameOverState());
                    if(leftDrops <= 0 ) StartCoroutine(winState());
                }
            }
                    
        }

        void spawnDrops(int nDrops)
        {
            for (int i = 0; i < nDrops; i++)
            {
                float xPos = UnityEngine.Random.Range(-12.1f, 12.1f);
                float yPos = UnityEngine.Random.Range(-12.1f, 12.1f);
                while (xPos > -1f && xPos < 1f) xPos = UnityEngine.Random.Range(-12.1f, 12.1f);
                while (yPos > -1f && yPos < 1f) yPos = UnityEngine.Random.Range(-12.1f, 12.1f);

                Vector2 spawnPos = new Vector2(xPos, yPos);
                Instantiate(drop, spawnPos, Quaternion.identity);
            }
        }

        void spawnEnemies(int nEnemies)
        {
            for (int i = 0; i < nEnemies; i++)
            {
                int enemyType = UnityEngine.Random.Range(0, 4);
                float xPos = UnityEngine.Random.Range(-12.1f, 12.1f);
                float yPos = UnityEngine.Random.Range(-12.1f, 12.1f);
                while (xPos > -1f && xPos < 1f) xPos = UnityEngine.Random.Range(-12.1f, 12.1f);
                while (yPos > -1f && yPos < 1f) yPos = UnityEngine.Random.Range(-12.1f, 12.1f);

                Vector2 spawnPos = new Vector2(xPos, yPos);
                Instantiate(enemies[enemyType], spawnPos, Quaternion.identity);
            }
        }

        IEnumerator InitialState()
        {
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "Listo?";
            yield return new WaitForSeconds(1);
            waveBox_UI.gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "Vamos!";
            yield return new WaitForSeconds(1);
            waveBox_UI.gameObject.SetActive(false);
            initialStateActive = false;
            playerActive = true;
        }

        public IEnumerator gameOverState()
        {
            gameOver = true;
            playerActive = false;
            waveBox_UI.GetComponent<Image>().color = new Color(1F, 0.0F, 0.0F, 0.5F);
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "Bebe sucio!";
            yield return new WaitForSeconds(1);
            returnButton_UI.gameObject.SetActive(true);
            //Si pierde enviar 6 notificaciones cada hora
            for (int i = 1; i <= 6; i++) NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(i*3600), "Baby Oops!", "Tu bebe necesita de atención", new Color(0, 0.6f, 1), NotificationIcon.Clock);
            PlayerPrefs.SetInt("hasPlayed", 0);
        }

        IEnumerator winState()
        {
            gameOver = true;
            playerActive = false;
            waveBox_UI.GetComponent<Image>().color = new Color(0.0F, 0.0F, 1F, 0.5F);
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "Bebe limpio!";
            yield return new WaitForSeconds(1);
            returnButton_UI.gameObject.SetActive(true);
            //Si gana, enviar 3 cada 3 horas y quitar la anteriores
            NotificationManager.CancelAll();
            for (int i = 1; i <= 3; i++) NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(i * 10800), "Baby Oops!", "Tu bebe necesita de atención", new Color(0, 0.6f, 1), NotificationIcon.Clock);
            PlayerPrefs.SetInt("hasPlayed", 1);
        }
    }

}
