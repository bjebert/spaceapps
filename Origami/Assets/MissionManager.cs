using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    // Use this for initialization
    void Start ()
    {
        GameObject sprites = GameObject.Find("SpaceModelsCollection");
        if(sprites != null)
        {
            Renderer[] spriteRenders = sprites.GetComponentsInChildren<Renderer>();
            foreach (Renderer spriteRender in spriteRenders)
            {
                spriteRender.enabled = false;
            }
        }

        DataModel myModel = this.gameObject.AddComponent<DataModel>();

        readData(myModel);
        plotMissionLines(myModel);

    }

    // Update is called once per frame
    void Update()
    {
        DataModel myModel = this.gameObject.GetComponent<DataModel>();

        double secs = myModel.playbackSpeed * Time.deltaTime * 1000;
        myModel.playbackTime = myModel.playbackTime.AddSeconds(secs);

        updateTrajectories(myModel);
    }

    void updateTrajectories(DataModel myModel)
    { 
        foreach (csvReader.Trajectory body in myModel.object_List)
        {
            int c = body.waypoints.Count;
            if (c > 0)
            {
                GameObject sprite = null;
                myModel.trajectoryGameObjects.TryGetValue(body.uid, out sprite);

                if(sprite != null)
                {
                    DateTime tsS = body.waypoints[0].zuluDate;
                    DateTime tsE = body.waypoints[c -1].zuluDate;

                    // Boundaries
                    Vector3 target = sprite.transform.position;

                    if (myModel.playbackTime <= tsS)
                    {
                        target = fromWayPoint(body.waypoints[0], myModel.galacticScale, body.size);
                    }
                    else if (myModel.playbackTime >= tsE)
                    {
                        target = fromWayPoint(body.waypoints[c - 1], myModel.galacticScale, body.size);
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
                                target = fromWayPoint(wp, myModel.galacticScale, body.size);
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

                                Vector3 p1 = fromWayPoint(wp1, myModel.galacticScale, body.size);
                                Vector3 p2 = fromWayPoint(wp2, myModel.galacticScale, body.size);

                                Vector3 v = p2 - p1;
                                Vector3 s = v * (float)mag;

                                target = p1 + s;
                            }
                        }
                    }
                    sprite.transform.position = target;
                    updateGameObjectSize(sprite, (float)body.size, myModel.galacticScale, myModel.planetScale);
                }
            }
        }
    }

    private void plotMissionLines(DataModel model)
    {
        foreach (csvReader.Trajectory trajectory in model.object_List)
        {
            LineRenderer lineRenderer = null;
            model.trajectoryLineRenders.TryGetValue(trajectory.uid, out lineRenderer);
            if(lineRenderer != null)
            {
                // Plot lines
                int index = 0;
                foreach (csvReader.Waypoint wp in trajectory.waypoints)
                {
                    Vector3 coords = fromWayPoint(wp, model.galacticScale, 0);
                    lineRenderer.SetPosition(index, coords);
                    index += 1;
                }
            }
        }
    }

    private void readData(DataModel model)
    {
        // Read from file
        List<String> fileLocations = new List<string>();
        fileLocations.Add(Application.dataPath + "/StreamingAssets/lunar_probe.txt");
        fileLocations.Add(Application.dataPath + "/StreamingAssets/lunar_orbit.txt");
        fileLocations.Add(Application.dataPath + "/StreamingAssets/earth_orbit.txt");
//        fileLocations.Add(Application.dataPath + "/StreamingAssets/earth_probe.txt");

        foreach (String fileLocation in fileLocations)
        {
            csvReader.Trajectory body = csvReader.createTrajectory(fileLocation);
            model.object_List.Add(body);
            GameObject sprite = GameObject.Instantiate(GameObject.Find(body.modelId));
            sprite.name = body.uid;
            Renderer[] spriteRenders = sprite.GetComponentsInChildren<Renderer>();
            foreach (Renderer spriteRender in spriteRenders)
            {
                spriteRender.enabled = true;
            }

            GameObject spriteTraj = new GameObject();
            spriteTraj.name = body.uid + "_lr";

            LineRenderer lineRenderer = spriteTraj.AddComponent<LineRenderer>();
            lineRenderer.name = body.uid + "_line";
            lineRenderer.positionCount = body.waypoints.Count;
            lineRenderer.widthMultiplier = 0.01f;
            lineRenderer.material = new Material(Shader.Find("Diffuse"));
            lineRenderer.material.color = body.colour;
            lineRenderer.startColor = body.colour;
            lineRenderer.endColor = body.colour;

            model.trajectoryGameObjects.Add(body.uid, sprite);
            model.trajectoryLineRenders.Add(body.uid, lineRenderer);
        }
    }

    private static Vector3 fromWayPoint(csvReader.Waypoint wp, float galacticScale, double size)
    {
        float x = (float)wp.X / galacticScale;
        float y = (float)wp.Y / galacticScale;
        float z = (float)wp.Z / galacticScale;

        return new Vector3(x, y, z);
    }

    private static void updateGameObjectSize(GameObject sprite, float size, float galacticScale, float planetScale)
    {
        Renderer[] spriteRender = sprite.GetComponentsInChildren<Renderer>();
        if (spriteRender.Length > 0)
        {
            Vector3 initScale = sprite.transform.localScale;
            Vector3 spriteSize = spriteRender[0].bounds.size * planetScale;

            float sizeMag = size / galacticScale;
            float spriteMag = spriteSize.magnitude;
            float scaleMag = sizeMag / spriteMag;

            sprite.transform.localScale = initScale * scaleMag;
        }
    }
}
