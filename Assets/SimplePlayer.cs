using UnityEngine;
using Valve.VR;
using UnityEngine.Splines;
using System.Linq;

public class SimplePlayer : MonoBehaviour
{
    SteamVR_Action_Boolean fwd = SteamVR_Input.GetBooleanAction("Forward");
    SteamVR_Action_Boolean bwd = SteamVR_Input.GetBooleanAction("Backward");
    SteamVR_Action_Boolean snapLeftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    SteamVR_Action_Boolean snapRightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");

    public SplineContainer s;

    float travelDelay = 0.25f;
    int steps = 7;
    int direction = 0;
    bool directionHeld = false;
    bool turnHeld = false;
    float turnDirection = 0;
    float turnDelay = 0.5f;
    bool cycleKnots = true;
    int i= 0;
    public Transform[] pointsOfInterests;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fwd.onStateDown += Fwd_onStateDown; 
        fwd.onStateUp += Fwd_onStateUp;
        bwd.onStateDown += Bwd_onStateDown;
        bwd.onStateUp += Bwd_onStateUp;
        snapLeftAction.onStateDown += SnapLeftAction_onStateDown;
        snapLeftAction.onStateUp += SnapLeftAction_onStateUp;
        snapRightAction.onStateDown += SnapRightAction_onStateDown;
        snapRightAction.onStateUp += SnapRightAction_onStateUp;
        gameObject.transform.position = s.EvaluatePosition(0);
        gameObject.transform.LookAt(pointsOfInterests[0]);
    }

    private void SnapRightAction_onStateUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        turnDirection = 0;
        turnHeld = false;
        CancelInvoke("TurnInterval");
    }

    private void SnapRightAction_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        turnDirection = 1;
        turnHeld = true;
        gameObject.transform.forward = gameObject.transform.right * turnDirection;
        InvokeRepeating("TurnInterval", turnDelay, turnDelay);
    }

    private void SnapLeftAction_onStateUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        turnDirection = 0;
        turnHeld = false;
        CancelInvoke("TurnInterval");
    }

    private void SnapLeftAction_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        turnDirection = -1;
        turnHeld = true;
        gameObject.transform.forward = gameObject.transform.right * turnDirection;
        InvokeRepeating("TurnInterval", turnDelay, turnDelay);
    }

    private void Bwd_onStateUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        direction = 0;
        directionHeld = false;
        CancelInvoke("UpdateInterval");
    }

    private void Fwd_onStateUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        direction = 0;
        directionHeld = false;
        CancelInvoke("UpdateInterval");
    }

    private void Bwd_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        direction = -1;
        directionHeld = true;
        TPNext();
        InvokeRepeating("UpdateInterval", travelDelay, travelDelay);
    }

    private void Fwd_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        direction = 1;
        directionHeld = true;
        TPNext();
        InvokeRepeating("UpdateInterval", travelDelay, travelDelay);
    }

    void TurnInterval()
    {
        if (turnHeld)
        {
            gameObject.transform.forward = gameObject.transform.right * turnDirection;
        }
    }

    void UpdateInterval()
    {
        if (directionHeld)
        {
            TPNext();
        }
    }

    void TPNext()
    {
        if (s)
        {
            if (cycleKnots)
            {
                i = i + direction;
                
                if (i < 0) i = steps - 1;
                else if (i > steps - 1) i = 0;

                gameObject.transform.position = (Vector3)s.Splines[0].Knots.ElementAt(i).Position + s.transform.position;
                gameObject.transform.LookAt(pointsOfInterests[i]);
            }
        }
    }
}
