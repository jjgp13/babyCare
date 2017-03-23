using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuController : MonoBehaviour {

    public Image babyFace;
    public Sprite[] faces;
    public Button[] playButtons;
    public Button exitButton;
    public Text babyStatusText;
    public Button infoBtn;
    public GameObject infoPanel;

    // Use this for initialization
    void Start () {
        if(PlayerPrefs.GetInt("hasPlayed") == 0){
            int randGame = Random.Range(0, 4);
            switch (randGame)
            {
                case 0:
                    babyStatusText.text = "Tu bebe necesita una limpieza!";
                    babyFace.sprite = faces[0];
                    playButtons[0].animator.SetBool("move", true);
                    break;
                case 1:
                    babyStatusText.text = "Tu bebe se puede enfermar!";
                    babyFace.sprite = faces[1];
                    playButtons[1].animator.SetBool("move", true);
                    break;
                case 2:
                    babyStatusText.text = "Tu bebe tiene hambre!";
                    babyFace.sprite = faces[2];
                    playButtons[2].animator.SetBool("move", true);
                    break;
                case 3:
                    babyStatusText.text = "Tu bebe quiere dormir!";
                    babyFace.sprite = faces[3];
                    playButtons[3].animator.SetBool("move", true);
                    break;
            }
        } else{
            babyStatusText.text = "Tu bebe esta bien por el momento!";
            babyFace.sprite = faces[4];
            exitButton.animator.SetBool("move", true);
        }
	}

    public void showInfo()
    {
        if(!infoPanel.activeSelf) infoPanel.SetActive(true);
        else infoPanel.SetActive(false);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("hasPlayed", 0);
    }

    public void closeApp()
    {
        PlayerPrefs.SetInt("hasPlayed", 0);
        Application.Quit();
    }
}
