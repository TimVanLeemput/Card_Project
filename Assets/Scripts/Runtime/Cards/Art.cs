using UnityEngine;
using UnityEngine.UI;

public class Art : MonoBehaviour
{

    private Image image = null;
    private Sprite sprite = null;

    public Sprite SpriteRef
    {
        get { return sprite; }
        set { sprite = value; }
    }

    virtual public void Start()
    {
        image = GetComponent<Image>(); // No need for UnityEngine.UI.Image here
        if (image == null)
        {
            return;
        }
        else
        {
            if (!image.sprite) return;
            
            sprite = image.sprite;
    

        }
    }
    

    public void SetSprite(Sprite _sprite)
    { 
        image.sprite = _sprite;
    }
 
 
}
