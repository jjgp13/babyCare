using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

	public void returnMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void openHealtGame()
    {
        SceneManager.LoadScene("HealtMiniGame");
    }

    public void openFoodGame()
    {
        SceneManager.LoadScene("FoodMiniGame");
    }

    public void openCleanGame()
    {
        SceneManager.LoadScene("CleanMiniGame");
    }

    public void openSleepGame()
    {
        SceneManager.LoadScene("SleepMiniGame");
    }
}
