using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bg : MonoBehaviour {

    public GameObject controller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        controller.GetComponent<controller_FG>().FoodInField = true;
    }
}
