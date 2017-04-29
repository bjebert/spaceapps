using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shuttleMove : MonoBehaviour {

    public const float SPEED = 2.0f; // Speed weighting
    public Transform target;
    
    void Start()
    {
        target = this.gameObject.GetComponentInChildren<MeshRenderer>().transform;
        target.position += Vector3.up * 10.0f;


    }

    void Update()
    {
        
        float step = SPEED * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }

}
