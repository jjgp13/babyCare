using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Assets.SimpleAndroidNotifications
{
    public class controller_FG : MonoBehaviour
    {

        public Text timeLeft_Text, foodLeft_Text, extraTime_Text;

        public Image waveBox_UI;
        public Text waveText_UI;
        public Button returnButton_UI;

        public GameObject baby, goodFods, badFoods;
        public Sprite[] babyFaces;
        public GameObject spawnFood;
        public bool FoodInField;
        public Sprite[] foods;
        public int[] goodFoodsArr;
        public int[] badFoodsArr;
        float timeLeft;
        int foodLeft;

        public bool gameOver;
        bool gameOverActive, initialStateActive, winStateActive;

        public GameObject bgSounds;
        public AudioClip cA, wA;

        public Button aceptarInsBtn;
        public GameObject panel;

        // Use this for initialization
        void Start()
        {
            foodLeft = UnityEngine.Random.Range(10, 31);
            timeLeft = (float)foodLeft * 2;
            setFoods();
            FoodInField = false;
            initialStateActive = true;
            gameOverActive = false;
            winStateActive = false;
            aceptarInsBtn.onClick.AddListener(delegate
            {
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
                    timeLeft_Text.text = timeLeft.ToString("0.0");
                    foodLeft_Text.text = foodLeft.ToString();
                    timeLeft -= Time.deltaTime;
                }
                if (timeLeft <= 0) StartCoroutine(gameOverState());
                if (foodLeft <= 0) StartCoroutine(winState());
            }
        }

        void setFoods()
        {
            int goodCount = 0, badCount = 0;
            for (int i = 0; i < 42; i++)
            {
                int rand = UnityEngine.Random.Range(0, 2);
                if (rand == 0)
                {
                    if (goodCount < 21)
                    {
                        goodFods.transform.GetChild(goodCount).GetComponent<SpriteRenderer>().sprite = foods[i];
                        goodFoodsArr[goodCount] = i;
                        goodCount++;
                    }
                    else
                    {
                        badFoods.transform.GetChild(badCount).GetComponent<SpriteRenderer>().sprite = foods[i];
                        badFoodsArr[badCount] = i;
                        badCount++;
                    }

                }

                if (rand == 1)
                {
                    if (badCount < 21)
                    {
                        badFoods.transform.GetChild(badCount).GetComponent<SpriteRenderer>().sprite = foods[i];
                        badFoodsArr[badCount] = i;
                        badCount++;
                    }
                    else
                    {
                        goodFods.transform.GetChild(goodCount).GetComponent<SpriteRenderer>().sprite = foods[i];
                        goodFoodsArr[goodCount] = i;
                        goodCount++;
                    }

                }
            }
        }

        public void checkAnswer(int foodType, string tag)
        {
            for (int i = 0; i < goodFoodsArr.Length; i++)
            {
                if (foodType == goodFoodsArr[i])
                {
                    if (tag == "Baby")
                    {
                        StartCoroutine(answer(0));
                        break;
                    }
                    if (tag == "Trash")
                    {
                        StartCoroutine(answer(1));
                        break;
                    }
                }

                if (foodType == badFoodsArr[i])
                {
                    if (tag == "Baby")
                    {
                        StartCoroutine(answer(1));
                        break;
                    }
                    if (tag == "Trash")
                    {
                        StartCoroutine(answer(0));
                        break;
                    }
                }
            }
        }

        IEnumerator answer(int type)
        {
            if (type == 0)
            {
                bgSounds.GetComponent<AudioSource>().clip = cA;
                bgSounds.GetComponent<AudioSource>().Play();
                timeLeft += 2f;
                timeLeft_Text.text = timeLeft.ToString("0.0");
                foodLeft--;
                foodLeft_Text.text = foodLeft.ToString();
                extraTime_Text.color = Color.blue;
                extraTime_Text.text = "+2";
                baby.GetComponent<SpriteRenderer>().sprite = babyFaces[UnityEngine.Random.Range(1, 3)];
                yield return new WaitForSeconds(1);
                extraTime_Text.text = "";
            }
            if (type == 1)
            {
                bgSounds.GetComponent<AudioSource>().clip = wA;
                bgSounds.GetComponent<AudioSource>().Play();
                timeLeft -= 2f;
                timeLeft_Text.text = timeLeft.ToString("0.0");
                extraTime_Text.color = Color.red;
                extraTime_Text.text = "-2";
                baby.GetComponent<SpriteRenderer>().sprite = babyFaces[UnityEngine.Random.Range(3, 5)];
                yield return new WaitForSeconds(1);
                extraTime_Text.text = "";
            }
        }

        IEnumerator spawnFoods()
        {
            while (!gameOver)
            {
                yield return new WaitForSeconds(0.1f);
                if (!FoodInField) Instantiate(spawnFood, new Vector2(3.7f, 0.0f), Quaternion.identity);
            }
            yield return new WaitForSeconds(3);
        }

        IEnumerator InitialState()
        {
            initialStateActive = true;
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
            StartCoroutine(spawnFoods());
        }

        IEnumerator gameOverState()
        {
            baby.GetComponent<SpriteRenderer>().sprite = babyFaces[4];
            timeLeft_Text.text = "0:0";
            gameOver = true;
            gameOverActive = true;
            waveBox_UI.GetComponent<Image>().color = new Color(1F, 0.0F, 0.0F, 0.5F);
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "Tu bebe no ha comido bien!";
            yield return new WaitForSeconds(1);
            returnButton_UI.gameObject.SetActive(true);
            //Si pierde enviar 6 notificaciones cada hora
            for (int i = 1; i <= 6; i++) NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(i * 3600), "Baby Oops!", "Tu bebe necesita de atención", new Color(0, 0.6f, 1), NotificationIcon.Clock);
            PlayerPrefs.SetInt("hasPlayed", 0);
        }

        IEnumerator winState()
        {
            baby.GetComponent<SpriteRenderer>().sprite = babyFaces[0];
            gameOver = true;
            winStateActive = true;
            waveBox_UI.GetComponent<Image>().color = new Color(0.0F, 0.0F, 1F, 0.5F);
            waveBox_UI.gameObject.SetActive(true);
            waveText_UI.text = "Tu bebe esta satisfecho!";
            yield return new WaitForSeconds(1);
            returnButton_UI.gameObject.SetActive(true);
            //Si gana, enviar 3 cada 3 horas y quitar la anteriores
            NotificationManager.CancelAll();
            for (int i = 1; i <= 3; i++) NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(i * 10800), "Baby Oops!", "Tu bebe necesita de atención", new Color(0, 0.6f, 1), NotificationIcon.Clock);
            PlayerPrefs.SetInt("hasPlayed", 1);
        }
    }
}
