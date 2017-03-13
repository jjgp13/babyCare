using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controller_FG : MonoBehaviour {

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

    // Use this for initialization
    void Start () {
        foodLeft = Random.Range(10, 31);
        timeLeft = (float) foodLeft * 2;
        setFoods();
        StartCoroutine(InitialState());
        FoodInField = false;
        StartCoroutine(spawnFoods());
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameOver)
        {
            if (!initialStateActive)
            {
                timeLeft_Text.text = timeLeft.ToString("0.0");
                foodLeft_Text.text = foodLeft.ToString();
                timeLeft -= Time.deltaTime;
            }
        }

        if (timeLeft <= 0) StartCoroutine(gameOverState());
        if (foodLeft <= 0) StartCoroutine(winState());
	}

    void setFoods()
    {
        int goodCount = 0, badCount = 0;
        for (int i = 0; i < 42; i++)
        {
            int rand = Random.Range(0, 2);
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

            if(rand == 1)
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
            if(foodType == goodFoodsArr[i])
            {
                if(tag == "Baby")
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
        if(type == 0)
        {
            bgSounds.GetComponent<AudioSource>().clip = cA;
            bgSounds.GetComponent<AudioSource>().Play();
            timeLeft += 2f;
            timeLeft_Text.text = timeLeft.ToString("0.0");
            foodLeft--;
            foodLeft_Text.text = foodLeft.ToString();
            extraTime_Text.color = Color.blue;
            extraTime_Text.text = "+2";
            baby.GetComponent<SpriteRenderer>().sprite = babyFaces[Random.Range(1, 3)];
            yield return new WaitForSeconds(1);
            baby.GetComponent<SpriteRenderer>().sprite = babyFaces[0];
            extraTime_Text.text = "";
        }
        if(type == 1)
        {
            bgSounds.GetComponent<AudioSource>().clip = wA;
            bgSounds.GetComponent<AudioSource>().Play();
            timeLeft -= 2f;
            timeLeft_Text.text = timeLeft.ToString("0.0");
            extraTime_Text.color = Color.red;
            extraTime_Text.text = "-2";
            baby.GetComponent<SpriteRenderer>().sprite = babyFaces[Random.Range(3, 5)];
            yield return new WaitForSeconds(1);
            baby.GetComponent<SpriteRenderer>().sprite = babyFaces[0];
            extraTime_Text.text = "";
        }
    }

    IEnumerator spawnFoods()
    {
        yield return new WaitForSeconds(3);
        while (!gameOver)
        {
            yield return new WaitForSeconds(0.1f);
            if (!FoodInField) Instantiate(spawnFood, new Vector2(3.7f, 0.0f), Quaternion.identity);
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
        gameOver = true;
        gameOverActive = true;
        waveBox_UI.GetComponent<Image>().color = new Color(1F, 0.0F, 0.0F, 0.5F);
        waveBox_UI.gameObject.SetActive(true);
        waveText_UI.text = "FIN DEL JUEGO";
        yield return new WaitForSeconds(1);
        returnButton_UI.gameObject.SetActive(true);
    }

    IEnumerator winState()
    {
        gameOver = true;
        winStateActive = true;
        waveBox_UI.GetComponent<Image>().color = new Color(0.0F, 0.0F, 1F, 0.5F);
        waveBox_UI.gameObject.SetActive(true);
        waveText_UI.text = "GANASTE!";
        yield return new WaitForSeconds(1);
        returnButton_UI.gameObject.SetActive(true);
    }
}
