using System;
using UnityEngine;
using Valve.VR;

public class SimplePlayer : MonoBehaviour
{
    SteamVR_Action_Boolean trigger = SteamVR_Input.GetBooleanAction("GrabPinch");
    SteamVR_Action_Vector2 scroll = SteamVR_Input.GetVector2Action("Scroll");

    float direction = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trigger.onStateDown += Trigger_onStateDown;
        scroll.onChange += Scroll_onChange;
    }

    private void Scroll_onChange(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
    {
        direction = Mathf.Clamp(axis.y * 1.1f, -1, 1);
        Debug.Log(direction);
    }

    private void Trigger_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("trigger");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
