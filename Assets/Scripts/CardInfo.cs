using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

[CreateAssetMenu(menuName = "CardInfoService")]
public class CardInfo : ScriptableObject
{
    [SerializeField] private string cardName = "";
    [SerializeField] private string cardType = string.Empty;

    [SerializeField] private int cardAttack = 0;
    [SerializeField] private int cardHealth = 0;
    [SerializeField] private int cardResourceCost = 0;
    [SerializeField] private ResourceType resourceType;

    [ContextMenu("CardInfo")]
    void SetCardInfo(string _cardName, string _cardType, int _cardAttack, int _cardHealth, int _cardResourceCost, ResourceType _resourceType)
    { 
        _cardName = cardName;
        _cardType = cardType;
        _cardAttack = cardAttack;
        _cardHealth = cardHealth;
        _cardResourceCost = cardResourceCost;
        _resourceType = resourceType;

    }



}
