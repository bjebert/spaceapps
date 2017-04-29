using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{

    public const float SPEED = 0.1f; // Speed weighting
    public Vector3 target;
    public Vector3 target2;


    // Use this for initialization
    void Start ()
    {
        DataModel myModel = this.gameObject.AddComponent<DataModel>();
        readData(myModel);

        myModel.shuttle = GameObject.Find("SpaceModelsCollection/shut");
        myModel.moon = GameObject.Find("SpaceModelsCollection/moon");

        if (myModel.shuttle != null)
        {
            target = myModel.shuttle.transform.position;
            target += Vector3.up * 100000.0f;

            target2 = myModel.moon.transform.position;
            target2 += Vector3.left * 100000.0f;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        DataModel myModel = this.gameObject.GetComponent<DataModel>();
        if(myModel != null && myModel.shuttle != null)
        {
            float step = SPEED * Time.deltaTime;
            myModel.shuttle.transform.position = Vector3.MoveTowards(myModel.shuttle.transform.position, target, step);
            myModel.moon.transform.position = Vector3.MoveTowards(myModel.moon.transform.position, target2, step);
        }
    }

    private void readData(DataModel model)
    {
        // Read from file

    }
}
