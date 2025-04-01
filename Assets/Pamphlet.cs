using UnityEngine;
using UnityEngine.UI;

public class Pamphlet : MonoBehaviour
{
    public MeshRenderer mr;
    public Texture2D[] images;

    public void SetImage(int index)
    {
        if (images[index] == null)
        {
            mr.enabled = false;
        }
        else
        {
            mr.enabled = true;
            mr.sharedMaterial.SetTexture("_BaseMap", images[index]);
            mr.sharedMaterial.SetTexture("_EmissionMap", images[index]);
        }
    }
}
