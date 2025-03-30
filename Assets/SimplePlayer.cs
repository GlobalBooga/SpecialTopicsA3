using UnityEngine;
using Valve.VR;
using UnityEngine.Splines;
using Valve.VR.Extras;

public class SimplePlayer : MonoBehaviour
{
    SteamVR_Action_Boolean trigger = SteamVR_Input.GetBooleanAction("GrabPinch");
    SteamVR_Action_Boolean reset = SteamVR_Input.GetBooleanAction("Reset");
    SteamVR_Action_Boolean fwd = SteamVR_Input.GetBooleanAction("Forward");
    SteamVR_Action_Boolean bwd = SteamVR_Input.GetBooleanAction("Backward");
    SteamVR_Action_Vector2 scroll = SteamVR_Input.GetVector2Action("Scroll");

    public SteamVR_LaserPointer laserPointerObj;
    public SplineContainer s;

    float prog = 0;
    float travelDelay = 0.25f;
    float steps = 10;
    float direction = 0;
    bool directionHeld = false;
    bool canSwipe = false;
    float swipeValue = 0.5f;
    Vector2 startPos;
    Vector3 p;
    Vector3 lastPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //trigger.onStateDown += Trigger_onStateDown;
        //trigger.onStateUp += Trigger_onStateUp;
        reset.onStateDown += Reset_onStateDown;
        fwd.onStateDown += Fwd_onStateDown; 
        fwd.onStateUp += Fwd_onStateUp;
        bwd.onStateDown += Bwd_onStateDown;
        bwd.onStateUp += Bwd_onStateUp;
        //trigger.onUpdate += Trigger_onUpdate;
        scroll.onUpdate += Scroll_onUpdate;
    }

    private void Scroll_onUpdate(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        swipeValue = Mathf.Clamp01(swipeValue + axis.x / 100);
        Debug.Log(swipeValue);
    }

    private void Trigger_onUpdate(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        if (canSwipe && laserPointerObj)
        {
            Vector3 pos = laserPointerObj.pointer.transform.position + laserPointerObj.pointer.transform.forward * 10f;
            p = Camera.main.WorldToScreenPoint(pos);
            float distx = p.x - startPos.x;

            float velocityx = p.x - lastPos.x;
            
            swipeValue = Mathf.Clamp01(distx/100 * velocityx);
            Debug.Log(swipeValue);

            lastPos = p;
        }
    }

    private void Trigger_onStateUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        canSwipe = false;
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
        gameObject.transform.forward = s.EvaluateTangent(prog);
    }

    private void Trigger_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        canSwipe = true;
        Vector3 pos = laserPointerObj.pointer.transform.position + laserPointerObj.pointer.transform.forward * 10f;
        startPos = Camera.main.WorldToScreenPoint(pos);
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
        prog = Mathf.Clamp01(prog + direction * 1 / steps);
        gameObject.transform.position = s.EvaluatePosition(prog);
        gameObject.transform.forward = s.EvaluateTangent(prog);
    }
}
