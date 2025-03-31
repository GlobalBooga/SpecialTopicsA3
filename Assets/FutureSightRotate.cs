using UnityEngine;

public class FutureSightRotate : MonoBehaviour
{
    public Transform lookAtObj;
    public Material glassMat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(lookAtObj,Vector3.up);
        //float d = Vector3.Distance(transform.position, lookAtObj.position);
        //Debug.Log(d);
        //glassMat.SetFloat("dist", Mathf.Clamp01(1/d*10));
    }
}
