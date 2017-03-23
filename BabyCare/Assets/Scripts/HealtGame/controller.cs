using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets.SimpleAndroidNotifications
{
    public class controller : MonoBehaviour
    {

        public GameObject[] enemies;
        public float spawnTime;
        public Vector2 spawnPosition;

        //Enemeigos
        public int enemyCount, enemyWaves, enemyInField, enemytxtN;
        public GameObject pArea;
        public Text enemyText;

        public bool gameOver;

        public Image waveBox_UI;
        public Text waveText_UI;
        public Button returnButton_UI;

        public Button aceptarInsBtn;
        public GameObject panel;

        // Use this for initialization
        void Start()
        {
            spawnTime = 2;
            enemyWaves = UnityEngine.Random.Range(3, 6);
            enemyText.text = "Enemigos: " + enemyCount.ToString();
            //StartCoroutine(InitialState());
            enemyInField = 0;
            enemytxtN = 0;
            gameOver = false;
            aceptarInsBtn.onClick.AddListener(delegate
            {
                StartCoroutine(InitialState());
                panel.SetActive(false);
            });
            gameOver = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!gameOver)
            {
                enemyText.text = "Enemigos: " + enemytxtN.ToString();
                if (pArea.GetComponent<protectedArea>().timeInArea <= 0) StartCoroutine(gameOverState());
            }
        }

        IEnumerator spawnEnemies()
        {
            int min = 5, max = 10;
            while (enemyWaves > 0)
            {
                enemyCount = UnityEngine.Random.Range(min, max);
                enemytxtN = enemyCount;
                min += 5;
                max += 5;

                StartCoroutine(waveUI());
                yield return new WaitForSeconds(3);

                while (enemyCount > 0)
                {
                    float randomPos;

                    int typeEnemie = UnityEngine.Random.Range(0, 4);
                    if (typeEnemie == 0 || typeEnemie == 1) randomPos = UnityEngine.Random.Range(-1.2f, 1.2f);
                    else if (typeEnemie == 3) randomPos = UnityEngine.Random.Range(-1.3f, 1.3f);
                    else randomPos = UnityEngine.Random.Range(-2.7f, 2.7f);

                    spawnPosition = new Vector2(randomPos, 5.2f);
                    Instantiate(enemies[typeEnemie], spawnPosition, Quaternion.identity);
                    enemyCount--;
                    enemyInField++;

                    yield return new WaitForSeconds(spawnTime);
                }
                while (enemyInField > 0) yield return new WaitForSeconds(3);

                if (spawnTime > 0) spawnTime -= 0.5f;
                enemyWaves--;
            }
            StartCoroutine(winState());
        }

        IEnumerator waveUI()
        {
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "Nivel " + enemyWaves.ToString();
            yield return new WaitForSeconds(3);
            waveBox_UI.gameObject.SetActive(false);
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
            StartCoroutine(spawnEnemies());
            gameOver = false;
        }

        IEnumerator gameOverState()
        {
            gameOver = true;
            StopCoroutine(spawnEnemies());
            destroyEnemies();
            waveBox_UI.GetComponent<Image>().color = new Color(1F, 0.0F, 0.0F, 0.5F);
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "¡Tu bebe ahora esta enfermo!";
            yield return new WaitForSeconds(1);
            returnButton_UI.gameObject.SetActive(true);
            //Si pierde enviar 6 notificaciones cada hora
            for (int i = 1; i <= 6; i++) NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(i * 3600), "Baby Oops!", "Tu bebe necesita de atención", new Color(0, 0.6f, 1), NotificationIcon.Clock);
            PlayerPrefs.SetInt("hasPlayed", 0);
        }

        void destroyEnemies()
        {
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy")) Destroy(enemy);
        }

        IEnumerator winState()
        {
            gameOver = true;
            StopCoroutine(spawnEnemies());
            waveBox_UI.GetComponent<Image>().color = new Color(0.0F, 0.0F, 1F, 0.5F);
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "Tu bebe esta a salvo!";
            yield return new WaitForSeconds(1);
            returnButton_UI.gameObject.SetActive(true);
            //Si gana, enviar 3 cada 3 horas y quitar la anteriores
            NotificationManager.CancelAll();
            for (int i = 1; i <= 3; i++) NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(i * 10800), "Baby Oops!", "Tu bebe necesita de atención", new Color(0, 0.6f, 1), NotificationIcon.Clock);
            PlayerPrefs.SetInt("hasPlayed", 1);
        }
    }
}