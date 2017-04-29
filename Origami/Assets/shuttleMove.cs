using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shuttleMove : MonoBehaviour {

    public const float SPEED = 0.1f; // Speed weighting
    public Vector3 target;
    
    void Start()
    {
        target = transform.position;
        target += Vector3.up * 100000.0f;


    }

    void Update()
    {

        float step = SPEED * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

}
