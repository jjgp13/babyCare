using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets.SimpleAndroidNotifications
{
    public class GameControllerSG : MonoBehaviour
    {

        private bool gameEnded = false;

        public circleBehavior rotator;
        public Spawner spawner;

        public Animator animator;

        public static int pinNumberLeft;
        public Text pinTextLeft;

        public Image waveBox_UI;
        public Text waveText_UI;
        public Button returnButton_UI;
        public int enemyWaves;
        public static bool gameOver;

        public GameObject babyFace;
        public Sprite[] faces;
        public static float timeToPin = 5f;
        public Text timeToPinText;
        public AudioClip babyCrying;

        private bool waveUIStateActive = false;

        public Button aceptarInsBtn;
        public GameObject panel;
        int pinBefore;

        private void Start()
        {
            pinNumberLeft = UnityEngine.Random.Range(3, 6);
            pinBefore = pinNumberLeft;
            pinTextLeft.text = pinNumberLeft.ToString();
            enemyWaves = UnityEngine.Random.Range(3, 6);
            gameOver = true;
            rotator.enabled = false;
            spawner.enabled = false;
            aceptarInsBtn.onClick.AddListener(delegate
            {
                StartCoroutine(InitialState());
                panel.SetActive(false);
            });
        }

        private void Update()
        {
            if (!gameOver)
            {
                pinTextLeft.text = pinNumberLeft.ToString();
                if (pinNumberLeft <= 0 && enemyWaves > 0)
                {
                    enemyWaves--;
                    StartCoroutine(waveUI());
                    pinNumberLeft = pinBefore + 2;
                    pinBefore = pinNumberLeft;
                }

                if (enemyWaves <= 2 && timeToPin > 2) babyFace.GetComponent<SpriteRenderer>().sprite = faces[1];

                if (timeToPin <= 2)
                {
                    babyFace.GetComponent<AudioSource>().Pause();
                    babyFace.GetComponent<SpriteRenderer>().sprite = faces[0];
                }
                else
                {
                    if (!babyFace.GetComponent<AudioSource>().isPlaying) babyFace.GetComponent<AudioSource>().Play();
                    babyFace.GetComponent<SpriteRenderer>().sprite = faces[1];
                }

                timeToPinText.text = timeToPin.ToString("0");
                if (!waveUIStateActive) timeToPin -= Time.deltaTime;

                if (timeToPin <= 0) endGame();
                if (enemyWaves == 0) StartCoroutine(winState());
            }
        }

        public void endGame()
        {
            gameOver = true;
            if (gameEnded)
                return;

            rotator.enabled = false;
            spawner.enabled = false;

            animator.SetTrigger("EndGame");
            StartCoroutine(gameOverState());

            gameEnded = true;

        }

        IEnumerator waveUI()
        {
            rotator.enabled = false;
            spawner.enabled = false;
            waveUIStateActive = true;
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "Nivel " + enemyWaves.ToString();
            yield return new WaitForSeconds(3);
            foreach (var pin in GameObject.FindGameObjectsWithTag("Pin")) Destroy(pin);
            waveBox_UI.gameObject.SetActive(false);
            rotator.enabled = true;
            spawner.enabled = true;
            waveUIStateActive = false;
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
            rotator.enabled = true;
            spawner.enabled = true;
            gameOver = false;
        }

        IEnumerator gameOverState()
        {
            gameOver = true;
            babyFace.GetComponent<SpriteRenderer>().sprite = faces[2];
            babyFace.GetComponent<AudioSource>().Stop();
            babyFace.GetComponent<AudioSource>().clip = babyCrying;
            babyFace.GetComponent<AudioSource>().Play();
            waveBox_UI.GetComponent<Image>().color = new Color(1F, 0.0F, 0.0F, 0.5F);
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "Perdiste!";
            yield return new WaitForSeconds(1);
            waveText_UI.text = "Tu bebe no se ha dormido!";
            returnButton_UI.gameObject.SetActive(true);
            //Si pierde enviar 6 notificaciones cada hora
            for (int i = 1; i <= 6; i++) NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(i * 3600), "Baby Oops!", "Tu bebe necesita de atención", new Color(0, 0.6f, 1), NotificationIcon.Clock);
            PlayerPrefs.SetInt("hasPlayed", 0);
        }

        IEnumerator winState()
        {
            gameOver = true;
            waveBox_UI.GetComponent<Image>().color = new Color(0.0F, 0.0F, 1F, 0.5F);
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "Tu bebe se ha dormido!";
            yield return new WaitForSeconds(1);
            returnButton_UI.gameObject.SetActive(true);
            //Si gana, enviar 3 cada 3 horas y quitar la anteriores
            NotificationManager.CancelAll();
            for (int i = 1; i <= 3; i++) NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(i * 10800), "Baby Oops!", "Tu bebe necesita de atención", new Color(0, 0.6f, 1), NotificationIcon.Clock);
            PlayerPrefs.SetInt("hasPlayed", 1);
        }
    }
}
