using Assets.SimpleAndroidNotifications;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject pinPrefab;
    public static bool pin = false;

    private void Update()
    {
        if (!GameControllerSG.gameOver)
        {
            if (Input.GetButtonDown("Fire1") && !pin)
            {
                pin = true;
                spawnPin();
            }
        }
    }

    void spawnPin()
    {
        Instantiate(pinPrefab, transform.position, transform.rotation);
    }
}
