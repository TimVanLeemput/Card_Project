using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[Flags]
public enum ResourceType
{
    None = 0,
    Fire = 1,
    Air = 2,
    Earth = 4,
    Water = 8,
    Darkness = 16,
    Light = 32
}

public enum CardType
{
    None,
    Creature,
    Instant,
    Sorcery,
    Resource
}
[CreateAssetMenu(menuName = "New Card")]
[Serializable]
public class CardInfo : ScriptableObject
{
    //Strings
    [Header("Text")]

    [SerializeField] private ResourceType cardResourceType;
    [SerializeField] private string cardTitle = "";
    [SerializeField] public string cardFlavorText = "";
    [SerializeField] public List<API_CardSkills.CardSkill> allCardSkills = null;

    //Colors
    [Header("Art & Colors")]

    [SerializeField] Sprite cardArtSprite = null;
    [SerializeField] Material cardArtMaterial = null;


    [Header("Stats")]

    [SerializeField] private int cardAttack = 0;
    [SerializeField] private int cardHealth = 0;

    [Header("Type")]
    [SerializeField] private int cardResourceCost = 0;
    [SerializeField] private CardType cardType;

    [Header("Optional")]
    [SerializeField][OverrideLabel("Display Resource Color")] private bool canDisplayResourceTypeColorIndicator = false;
    [SerializeField][ConditionalField(nameof(canDisplayResourceTypeColorIndicator))] private Color resourceTypeColorIndicator = Color.white;

    //Accessors
#if UNITY_EDITOR

    private void OnEnable()
    {

        cardTitle = System.IO.Path.GetFileNameWithoutExtension(UnityEditor.AssetDatabase.GetAssetPath(this));
    }
#endif
    public List<API_CardSkills.CardSkill> AllCardSkills
    {
        get { return allCardSkills; }
        set { allCardSkills = value; }
    }

    public Sprite CardArtSpriteRef
    {
        get { return cardArtSprite; }
        set { cardArtSprite = value; }
    }
    public Material CardArtMaterial
    {
        get { return cardArtMaterial; }
        set { cardArtMaterial = value; }
    }


    public string CardTitle
    {
        get { return cardTitle; }
        set { cardTitle = value; }
    }

    public string CardFlavorText
    {

        get { return cardFlavorText; }
        set { cardFlavorText = value; }
    }

    public int CardAttackRef
    {
        get { return cardAttack; }
        set { cardAttack = value; }
    }

    public int CardHealthRef
    {

        get { return cardHealth; }
        set { cardHealth = value; }
    }

    public int CardResourceCostRef
    {
        get { return cardResourceCost; }
        set { cardResourceCost = value; }
    }

    public ResourceType ResourceTypeRef
    {
        get { return cardResourceType; }
        set { cardResourceType = value; }
    }

    public CardType CardTypeRef
    {
        get { return cardType; }
        set { cardType = value; }
    }

    public Color ResourceTypeColorIndicator
    {
        get { return resourceTypeColorIndicator; }
        set { resourceTypeColorIndicator = value; }
    }

    public bool CanDisplayResourceTypeColorIndicator
    {
        get { return canDisplayResourceTypeColorIndicator; }
        set { canDisplayResourceTypeColorIndicator = value; }
    }

    public void SetCardFlavorText(string _flavorText)
    {
        cardFlavorText = _flavorText;
    }



}
