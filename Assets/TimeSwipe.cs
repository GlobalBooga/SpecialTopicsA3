using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeSwipe : MonoBehaviour
{
    public RawImage image;
    Material swiperMaterial;
    [Range(0,1)] public float swipe = 0;
    public Transform txtFuture;
    public Transform txtPresent;

    public Vector3 topCenterOffset = new Vector3(20,0,0);

    public float screenSizeX = 1920;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        swiperMaterial = image.material;
        screenSizeX = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (swiperMaterial != null)
        {
            swiperMaterial.SetFloat("_Swipe", swipe);
        }
        
        txtFuture.position = Vector3.right * (screenSizeX * swipe) - topCenterOffset + Vector3.up * txtFuture.position.y;
        txtPresent.position = Vector3.right * (screenSizeX * swipe) + topCenterOffset + Vector3.up * txtFuture.position.y;
        
    }
}
