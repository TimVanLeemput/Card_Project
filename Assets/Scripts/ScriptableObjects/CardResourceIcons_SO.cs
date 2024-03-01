using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Resource Icon")]
public class CardResourceIcons_SO : ScriptableObject
{

    [SerializeField] public Sprite fireIconSprite = null;
    [SerializeField] public Sprite airIconSprite = null;
    [SerializeField] public Sprite waterIconSprite = null;
    [SerializeField] public Sprite earthIconSprite = null;
    [SerializeField] public Sprite darknessIconSprite = null;
    [SerializeField] public Sprite lightIconSprite = null;

    //public Sprite FireIconSprite
    //{
    //    get { return fireIconSprite; }
    //    set { fireIconSprite = value; }
    //}
    //public Sprite WaterIconSprite
    //{
    //    get { return waterIconSprite; }
    //    set { waterIconSprite = value; }
    //}
    //public  Sprite LoadFireIconSprite()
    //{
    //    fireIconSprite = Resources.Load<Sprite>("fire_icon.png");
    //    return fireIconSprite;
    //}
    //public  Sprite LoadWaterIconSprite(string _spriteName)
    //{
    //    waterIconSprite = Resources.Load<Sprite>(_spriteName);
    //    if (!waterIconSprite)
    //        Debug.Log($"not found sprite : {waterIconSprite}");
    //    return waterIconSprite;
    //}

}
