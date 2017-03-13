using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

    public Sprite[] typeFood;
    public int type;

    public float maxTime;
    public float minSwipeDist;

    float startTime;
    float endTime;

    Vector3 startPos;
    Vector3 endPos;
    float swipeDistance;
    float swipeTime;

    bool up, down;

	// Use this for initialization
	void Start () {
        type = Random.Range(0, 42);
        GetComponent<SpriteRenderer>().sprite = typeFood[type];
        up = down = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(transform.position.x >= 0.0f)
            GetComponent<Rigidbody2D>().velocity = Vector3.left * 10;
        else
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        if(up)
            GetComponent<Rigidbody2D>().velocity = Vector3.up * 20;

        if (down)
            GetComponent<Rigidbody2D>().velocity = Vector3.down * 20;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                startPos = touch.position;
            } else if (touch.phase == TouchPhase.Ended)
            {
                endTime = Time.time;
                endPos = touch.position;

                swipeDistance = (endPos - startPos).magnitude;
                swipeTime = endTime - startTime;

                if(swipeTime < maxTime && swipeDistance > minSwipeDist)
                {
                    swipe();
                }
            }
        }
    }

    void swipe()
    {
        Vector2 distance = endPos - startPos;
        if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
        {
            if (distance.y > 0) up = true;
            if (distance.y < 0) down = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Baby" || other.tag == "Trash")
        {
            GameObject.Find("gameController").GetComponent<controller_FG>().checkAnswer(type, other.tag);
            GameObject.Find("gameController").GetComponent<controller_FG>().FoodInField = false;
            Destroy(gameObject);
        }
    }
}
