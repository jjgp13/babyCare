using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

    public float speed;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
    }
}
