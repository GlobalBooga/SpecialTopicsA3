using UnityEngine;
using UnityEngine.UI;

public class Pamphlet : MonoBehaviour
{
    public Image img;
    public Sprite[] images;

    public void SetImage(int index)
    {
        img.sprite = images[index];
    }
}
