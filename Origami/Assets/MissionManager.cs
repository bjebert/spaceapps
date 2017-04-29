using System;
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

        myModel.trajectoryGameObjects.Add("shut", GameObject.Find("SpaceModelsCollection/shut"));
//        myModel.trajectoryGameObjects.Add("moon", GameObject.Find("SpaceModelsCollection/moon"));

        csvReader.Trajectory shut = new csvReader.Trajectory("shut", new List<csvReader.Waypoint>() );
//        csvReader.Trajectory moon = new csvReader.Trajectory("moon", new List<csvReader.Waypoint>());

        shut.waypoints.Add(new csvReader.Waypoint(myModel.playbackTime, 0, 137633909.2680, 349792303.3080, 124291199.2880 ));
        shut.waypoints.Add(new csvReader.Waypoint(myModel.playbackTime.AddMinutes(1), 0, 356267828.1030, -60201234.2086, -8497196.120   ));

        myModel.object_List.Add(shut);

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

        double seconds = myModel.playbackSpeed * Time.deltaTime;
        myModel.playbackTime = myModel.playbackTime.AddSeconds(seconds);

        foreach (csvReader.Trajectory body in myModel.object_List)
        {
            int c = body.waypoints.Count;
            if (c > 0)
            {
                GameObject sprite = null;
                if (myModel.trajectoryGameObjects.TryGetValue(body.modelId, out sprite))
                {
                    DateTime tsS = body.waypoints[0].zuluDate;
                    DateTime tsE = body.waypoints[c -1].zuluDate;

                    // Boundaries
                    Vector3 target = sprite.transform.position;

                    if (myModel.playbackTime <= tsS)
                    {
                        target = fromWayPoint(body.waypoints[0]);
                    }
                    else if (myModel.playbackTime >= tsE)
                    {
                        target = fromWayPoint(body.waypoints[c - 1]);
                    }
                    else
                    {
                        csvReader.Waypoint wp1 = body.waypoints[0];
                        csvReader.Waypoint wp2 = body.waypoints[0];

                        bool skiped = false;
                        foreach (csvReader.Waypoint wp in body.waypoints)
                        {
                            if (myModel.playbackTime == wp.zuluDate)
                            {
                                target = fromWayPoint(wp);
                                skiped = true;
                            }
                            else
                            {
                                wp1 = wp2;
                                wp2 = wp;

                                if (wp.zuluDate > myModel.playbackTime)
                                {
                                    break;
                                }
                            }
                        }

                        if(!skiped)
                        {
                            TimeSpan diffA = myModel.playbackTime - wp1.zuluDate;
                            TimeSpan diffB = wp2.zuluDate - wp1.zuluDate;
                            if (diffB.TotalMilliseconds > 0)
                            {
                                double mag = diffA.TotalMilliseconds / diffB.TotalMilliseconds;

                                Vector3 p1 = fromWayPoint(wp1);
                                Vector3 p2 = fromWayPoint(wp2);

                                Vector3 v = p2 - p1;
                                Vector3 s = v * (float)mag;

                                target = p1 + s;
                            }
                        }
                    }
                    sprite.transform.position = target;
                }
            }
        }

        /*
        if(myModel != null && myModel.shuttle != null)
        {
            float step = SPEED * Time.deltaTime;
            myModel.shuttle.transform.position = Vector3.MoveTowards(myModel.shuttle.transform.position, target, step);
            myModel.moon.transform.position = Vector3.MoveTowards(myModel.moon.transform.position, target2, step);
        }
        */
    }

    private void readData(DataModel model)
    {
        // Read from file
        string fileLocation = Application.dataPath + "/StreamingAssets/lunar_probe.txt";
        model.object_List.Add(csvReader.createTrajectory("shuttle", fileLocation));

        fileLocation = Application.dataPath + "/StreamingAssets/lunar_orbit.txt";
        model.object_List.Add(csvReader.createTrajectory("moon", fileLocation));
    }

    private static Vector3 fromWayPoint(csvReader.Waypoint wp)
    {
        return new Vector3((float)wp.X / (float)100000000.0, (float)wp.Y / (float)100000000.0, (float)wp.Z / (float)100000000.0);
    }
}
