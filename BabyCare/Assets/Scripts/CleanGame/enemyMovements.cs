using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMovements : MonoBehaviour {

    int randomMove;
    float min, max, t;
    float iniPosX, iniPosY;

	// Use this for initialization
	void Start () {
        randomMove = Random.Range(0, 4);
        min = -1.0f;
        min = 1.0f;
        t = 0.0f;
        iniPosX = transform.position.x;
        iniPosY = transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        switch (randomMove)
        {
            case 0:
                transform.position = new Vector3(Mathf.Lerp(min + iniPosX, max + iniPosX, t), transform.position.y, 0);
                break;
            case 1:
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(min + iniPosY, max+iniPosY, t), 0);
                break;
            case 2:
                transform.position = new Vector3(Mathf.Lerp(min + iniPosX, max + iniPosX, t), Mathf.Lerp(min + iniPosY, max + iniPosY, t), 0);
                break;
            case 3:
                transform.position = new Vector3(-Mathf.Lerp(min + iniPosX, max + iniPosX, t), Mathf.Lerp(min + iniPosY, max + iniPosY, t), 0);
                break;
            default:
                break;
        }
        
        t += 0.5f * Time.deltaTime;

        if (t > 1.0f)
        {
            float temp = max;
            max = min;
            min = temp;
            t = 0.0f;
        }
    }
}
