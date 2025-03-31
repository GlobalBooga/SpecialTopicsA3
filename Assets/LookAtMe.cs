using UnityEngine;

public class LookAtMe : MonoBehaviour
{
    public Transform lookAt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).LookAt(lookAt, Vector3.up);
        }
    }
}
