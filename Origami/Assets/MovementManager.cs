
using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
// Needed //////////////////////////////////////////////////
using HoloLensXboxController;
using System;
///////////////////////////////////////////////////////////

public class MovementManager : MonoBehaviour
{

    // Needed //////////////////////////////////////////////////
    private ControllerInput controllerInput;
    ///////////////////////////////////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Needed //////////////////////////////////////////////////
        controllerInput = new ControllerInput(0, 0.19f);
        // First parameter is the number, starting at zero, of the controller you want to follow.
        // Second parameter is the default “dead” value; meaning all stick readings less than this value will be set to 0.0.
        ///////////////////////////////////////////////////////////
    }

    void Update()
    {
        // Needed //////////////////////////////////////////////////
        try
        {
            controllerInput.Update();
        }
        catch(Exception e)
        {  }
        ///////////////////////////////////////////////////////////


        GameObject codeMan = GameObject.Find("CodeManager");
        DataModel myModel = codeMan.GetComponent<DataModel>();

        setStateApp(myModel);
    }

    private void setStateApp(DataModel model)
    {
        if (controllerInput.GetButtonDown(ControllerButton.LeftThumbstick))
        {
            model.lockCamera = false;
        }
        if (controllerInput.GetButtonUp(ControllerButton.LeftThumbstick))
        {
            model.lockCamera = true;
        }

        if(!model.lockCamera)
        {
            model.playbackSpeed += (5 * controllerInput.GetAxisRightThumbstickX());
        }
        else
        {
            model.cameraOffset.x += ((float)0.1 * controllerInput.GetAxisLeftThumbstickX());
            model.cameraOffset.y += ((float)0.1 * controllerInput.GetAxisLeftThumbstickY());
            model.cameraOffset.z += ((float)0.1 * controllerInput.GetAxisRightThumbstickY());
        }

        if (controllerInput.GetButtonDown(ControllerButton.X))
        {
            model.planetScale *= (float)0.90;
        }

        if (controllerInput.GetButtonDown(ControllerButton.B))
        {
            model.planetScale *= (float)1.10;
        }

        if (controllerInput.GetButtonDown(ControllerButton.A))
        {
            model.galacticScale *= (float)0.90;
        }

        if (controllerInput.GetButtonDown(ControllerButton.Y))
        {
            model.galacticScale *= (float)1.10;
        }

        if (controllerInput.GetButtonDown(ControllerButton.LeftShoulder))
        {
            model.playbackSpeed = 0;
        }

        if (controllerInput.GetButtonDown(ControllerButton.RightShoulder))
        {
            model.playbackSpeed = 1;
        }

    }
}
