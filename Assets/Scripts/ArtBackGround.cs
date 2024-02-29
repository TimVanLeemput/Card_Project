using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtBackGround : MonoBehaviour
{
    private Image image = null;
    public Image ImageRef
    {
        get { return image; }
        set { image = value; }
    }

    virtual protected void Start()
    {
        image = GetComponent<Image>();

        if (!image)
        {

            Debug.Log("No image found in ArtBackGroundColor");
        }
    }

    void Update()
    {

    }
}
