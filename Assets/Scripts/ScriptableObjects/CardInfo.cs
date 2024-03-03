using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum ResourceType
{
    None,
    Fire,
    Air,
    Earth,
    Water,
    Darkness,
    Light
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
public class CardInfo : ScriptableObject
{
    //Strings
    [Header("Text")]

    [SerializeField] private ResourceType resourceType;
    [SerializeField] private string cardTitle = "";
    [SerializeField] List<API_CardSkills.CardSkill> allCardSkills = null;

    //Colors
    [Header("Art & Colors")]

    [SerializeField] Sprite cardArtSprite = null;


    [Header("Stats")]

    [SerializeField] private int cardAttack = 0;
    [SerializeField] private int cardHealth = 0;

    [Header("Type")]
    [SerializeField] private int cardResourceCost = 0;
    [SerializeField] private CardType cardType;


    //Accessors
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


    public string CardTitle
    {
        get { return cardTitle; }
        set { cardTitle = value; }
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
        get { return resourceType; }
        set { resourceType = value; }
    }

    public CardType CardTypeRef
    {
        get { return cardType; }
        set { cardType = value; }
    }
 





}
