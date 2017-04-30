
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

    public float RotateAroundYSpeed = 2.0f;
    public float RotateAroundXSpeed = 2.0f;
    public float RotateAroundZSpeed = 2.0f;

    public float MoveHorizontalSpeed = 0.01f;
    public float MoveVerticalSpeed = 0.01f;

    public float ScaleSpeed = 1f;

    public Text AxisInputText;
    public Text ButtonInputText;
    public Text JoyStickNamesText;

    private string lastButtonDown = string.Empty;
    private string lastButtonUp = string.Empty;

    private void translateRotateScale()
    {

    }

    private void setAxisInputText()
    {

    }

    private void setStateApp(DataModel model)
    {
        setLastButtonDown();
        setLastButtonUp();

        model.playbackSpeed = (20 * controllerInput.GetAxisRightThumbstickX());


        if (lastButtonDown == "X")
        {
            model.planetScale += (float)0.1;
        }
        else if (lastButtonDown == "B")
        {
            model.planetScale -= (float)0.1;
        }
        else if (lastButtonDown == "A")
        {
            model.galacticScale += 10;
        }
        else if (lastButtonDown == "Y")
        {
            model.galacticScale -= 10;
        }
        else if (lastButtonDown == "LB")
        {
            model.playbackSpeed = 0;
        }
        else if (lastButtonDown == "RB")
        {
            model.playbackSpeed = 1;
        }

    }

    private void setLastButtonDown()
    {
        if (controllerInput.GetButtonDown(ControllerButton.A))
            lastButtonDown = "A";

        if (controllerInput.GetButtonDown(ControllerButton.B))
            lastButtonDown = "B";

        if (controllerInput.GetButtonDown(ControllerButton.X))
            lastButtonDown = "X";

        if (controllerInput.GetButtonDown(ControllerButton.Y))
            lastButtonDown = "Y";

        if (controllerInput.GetButtonDown(ControllerButton.LeftShoulder))
            lastButtonDown = "LB";

        if (controllerInput.GetButtonDown(ControllerButton.RightShoulder))
            lastButtonDown = "RB";

        if (controllerInput.GetButtonDown(ControllerButton.View))
            lastButtonDown = "SHOW ADDRESS";

        if (controllerInput.GetButtonDown(ControllerButton.Menu))
            lastButtonDown = "SHOW MENU";

        if (controllerInput.GetButtonDown(ControllerButton.LeftThumbstick))
            lastButtonDown = "LEFT STICK CLICK";

        if (controllerInput.GetButtonDown(ControllerButton.RightThumbstick))
            lastButtonDown = "RIGHT STICK CLICK";

        if (controllerInput.GetButtonDown(ControllerButton.DPadDown))
            lastButtonDown = "DPadDown";

        if (controllerInput.GetButtonDown(ControllerButton.DPadUp))
            lastButtonDown = "DPadUp";

        if (controllerInput.GetButtonDown(ControllerButton.DPadLeft))
            lastButtonDown = "DPadLeft";

        if (controllerInput.GetButtonDown(ControllerButton.DPadRight))
            lastButtonDown = "DPadRight";
    }

    private void setLastButtonUp()
    {
        if (controllerInput.GetButtonUp(ControllerButton.A))
            lastButtonUp = "A";

        if (controllerInput.GetButtonUp(ControllerButton.B))
            lastButtonUp = "B";

        if (controllerInput.GetButtonUp(ControllerButton.X))
            lastButtonUp = "X";

        if (controllerInput.GetButtonUp(ControllerButton.Y))
            lastButtonUp = "Y";

        if (controllerInput.GetButtonUp(ControllerButton.LeftShoulder))
            lastButtonUp = "LB";

        if (controllerInput.GetButtonUp(ControllerButton.RightShoulder))
            lastButtonUp = "RB";

        if (controllerInput.GetButtonUp(ControllerButton.View))
            lastButtonUp = "SHOW ADDRESS";

        if (controllerInput.GetButtonUp(ControllerButton.Menu))
            lastButtonUp = "SHOW MENU";

        if (controllerInput.GetButtonUp(ControllerButton.LeftThumbstick))
            lastButtonUp = "LEFT STICK CLICK";

        if (controllerInput.GetButtonUp(ControllerButton.RightThumbstick))
            lastButtonUp = "RIGHT STICK CLICK";

        if (controllerInput.GetButtonUp(ControllerButton.DPadDown))
            lastButtonUp = "DPadDown";

        if (controllerInput.GetButtonUp(ControllerButton.DPadUp))
            lastButtonUp = "DPadUp";

        if (controllerInput.GetButtonUp(ControllerButton.DPadLeft))
            lastButtonUp = "DPadLeft";

        if (controllerInput.GetButtonUp(ControllerButton.DPadRight))
            lastButtonUp = "DPadRight";
    }



}
