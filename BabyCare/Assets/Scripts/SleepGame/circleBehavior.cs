using Assets.SimpleAndroidNotifications;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleBehavior : MonoBehaviour {

    public float speed = 10f;

	void FixedUpdate () {
        if(!GameControllerSG.gameOver)
            transform.Rotate(0f, 0f, speed * Time.deltaTime);
	}
}
