using System;
using UnityEngine;
using Valve.VR;
using UnityEngine.Splines;
using Valve.VR.InteractionSystem;
public class SimplePlayer : MonoBehaviour
{
    SteamVR_Action_Boolean trigger = SteamVR_Input.GetBooleanAction("GrabPinch");
    SteamVR_Action_Vector2 scroll = SteamVR_Input.GetVector2Action("Scroll");
    SteamVR_Action_Boolean reset = SteamVR_Input.GetBooleanAction("Reset");

    public SplineContainer s;

    float prog = 0;
    float speed = 0.2f;
    float direction = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trigger.onStateDown += Trigger_onStateDown;
        scroll.onUpdate += Scroll_onUpdate;
        reset.onStateDown += Reset_onStateDown;
    }

    private void Reset_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        prog = 0;
        gameObject.transform.position = s.EvaluatePosition(prog);
        gameObject.transform.forward = s.EvaluateTangent(prog);
    }

    private void Scroll_onUpdate(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        direction = Mathf.Clamp(axis.y * 1.1f, -1, 1);
        Debug.Log(direction);
        prog = Mathf.Clamp01(prog + direction * Time.deltaTime * speed);
        gameObject.transform.position = s.EvaluatePosition(prog);
        gameObject.transform.forward = s.EvaluateTangent(prog);
    }

    private void Trigger_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
