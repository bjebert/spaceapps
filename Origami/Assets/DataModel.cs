using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataModel : MonoBehaviour
{ 
    public List<csvReader.Trajectory> object_List = new List<csvReader.Trajectory>();
    public Dictionary<string, GameObject>   trajectoryGameObjects = new Dictionary<string, GameObject>();
    public Dictionary<string, LineRenderer> trajectoryLineRenders = new Dictionary<string, LineRenderer>();

    public DateTime playbackTime = new DateTime(2014, 07, 22, 11, 00, 00);
    public double playbackSpeed = 10;

    public float  galacticScale = (float)100000000.0;

    public float planetScale = 1;

    public Vector3 cameraOffset = new Vector3(0, 0, 0);
    public Boolean lockCamera = false;
}
