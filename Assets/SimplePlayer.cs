using UnityEngine;
using Valve.VR;
using UnityEngine.Splines;
using System.Linq;

public class SimplePlayer : MonoBehaviour
{
    SteamVR_Action_Boolean reset = SteamVR_Input.GetBooleanAction("Reset");
    SteamVR_Action_Boolean fwd = SteamVR_Input.GetBooleanAction("Forward");
    SteamVR_Action_Boolean bwd = SteamVR_Input.GetBooleanAction("Backward");
    SteamVR_Action_Boolean snapLeftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    SteamVR_Action_Boolean snapRightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");

    //public SteamVR_LaserPointer laserPointerObj;
    public SplineContainer s;

    float prog = 0;
    float travelDelay = 0.25f;
    float steps = 7;
    float direction = 0;
    bool directionHeld = false;
    bool turnHeld = false;
    float turnDirection = 0;
    float turnDelay = 0.5f;
    bool cycleKnots = true;
    int i= 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        reset.onStateDown += Reset_onStateDown;
        fwd.onStateDown += Fwd_onStateDown; 
        fwd.onStateUp += Fwd_onStateUp;
        bwd.onStateDown += Bwd_onStateDown;
        bwd.onStateUp += Bwd_onStateUp;
        snapLeftAction.onStateDown += SnapLeftAction_onStateDown;
        snapLeftAction.onStateUp += SnapLeftAction_onStateUp;
        snapRightAction.onStateDown += SnapRightAction_onStateDown;
        snapRightAction.onStateUp += SnapRightAction_onStateUp;
        gameObject.transform.position = s.EvaluatePosition(0);
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

    private void Reset_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        prog = 0;
        gameObject.transform.position = s.EvaluatePosition(prog);
        //gameObject.transform.forward = s.EvaluateTangent(prog);
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
                i = Mathf.Clamp(i + (int)direction, 0, (int)steps);
                gameObject.transform.position = (Vector3)s.Splines[0].Knots.ElementAt(i).Position + s.transform.position;
            }
            else
            {
                prog = Mathf.Clamp01(prog + direction * 1 / steps);
                gameObject.transform.position = s.EvaluatePosition(prog);
                //gameObject.transform.forward = s.EvaluateTangent(prog);
            }
        }
    }
}
