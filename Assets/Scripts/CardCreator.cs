using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class CardCreator : MonoBehaviour
{
    [SerializeField] CardInfo cardInfo = null;
    [SerializeField] CardResourceIcons_SO allCardResourceIcons = null;
    //Art
    Art art = null;
    CardResourceIcon cardResourceIcon = null;

    CardName titleCardName = null;
    CardSkillDescription cardSkillDescription = null;
    ResourceCostText resourceCostText = null;
    CardAttackAmount cardAttackAmount = null;
    CardHealthAmount cardHealthAmount = null;

    ArtBackGround artBackGround = null;
    CardTitleBackGroundColor cardTitleBackGroundColor = null;
    CardTypeBackGroundColor cardTypeBackGroundColor = null;
    CardStatsBackGroundColor cardStatsBackGroundColor = null;


    private void Awake()
    {

    }
    void Start()
    {
        LateInit();
    }
    private void LateInit()
    {
        Invoke(nameof(Init), 0.1f);
        Invoke(nameof(InitCard), 0.11f);
    }

    public void Init()
    {
        artBackGround = GetComponentInChildren<ArtBackGround>();
        art = GetComponentInChildren<Art>();

        titleCardName = GetComponentInChildren<CardName>();

        cardSkillDescription = GetComponentInChildren<CardSkillDescription>();
        resourceCostText = GetComponentInChildren<ResourceCostText>();
        cardAttackAmount = GetComponentInChildren<CardAttackAmount>();
        cardHealthAmount = GetComponentInChildren<CardHealthAmount>();


        cardResourceIcon = GetComponentInChildren<CardResourceIcon>();
        cardTitleBackGroundColor = GetComponentInChildren<CardTitleBackGroundColor>();
        cardTypeBackGroundColor = GetComponentInChildren<CardTypeBackGroundColor>();
        cardStatsBackGroundColor = GetComponentInChildren<CardStatsBackGroundColor>();
    }

    void InitCard()
    {
        if (cardInfo == null)
        {
            Debug.Log("failed to find cardinfo");
            return;
        }

        SetCardResourceCost(cardInfo.CardResourceCostRef);  // int
        SetCardAttackAmount(cardInfo.CardAttackRef);
        SetCardHealthAmount(cardInfo.CardHealthRef);

        SetSprite(art, cardInfo.CardArtSpriteRef);
        SwitchCardColor();

        titleCardName.SetTitleName(cardInfo.CardTitle);
        //SetCardTitleName(cardInfo.CardTitle);


        SwitchCardSkills();

        //Debug.Log($"cardinfo sprite = {cardInfo.CardArtSpriteRef}");

    }

    private void SwitchCardSkills()
    {
        int _size = cardInfo.AllCardSkills.Count;
        for (int i = 0; i < _size; i++)
        {
            switch (cardInfo.AllCardSkills[i])
            {
                case API_CardSkills.CardSkill.Burn:
                    SetCardSkillText(API_CardSkills.burnString);
                    continue;
                case API_CardSkills.CardSkill.Fly:
                    SetCardSkillText(API_CardSkills.flyString);
                    continue;
                case API_CardSkills.CardSkill.Poison:
                    SetCardSkillText(API_CardSkills.poisonString);
                    continue;
                case API_CardSkills.CardSkill.DeathTouch:
                    SetCardSkillText(API_CardSkills.deathTouchString);
                    continue;
                case API_CardSkills.CardSkill.LifeTouch:
                    SetCardSkillText(API_CardSkills.lifeTouchString);
                    continue;
                case API_CardSkills.CardSkill.FirstAttack:
                    SetCardSkillText(API_CardSkills.firstAttackString);
                    continue;
                case API_CardSkills.CardSkill.NONE:
                    SetCardSkillText(API_CardSkills.none);
                    continue;

            }

        }
    }
    private void SwitchCardColor()
    {
        switch (cardInfo.ResourceTypeRef)
        {
            case ResourceType.None:
                break;
            case ResourceType.Fire:
                SetAllCardBackGroundColors(API_CardColors.fireColor);
                SetSprite(cardResourceIcon, allCardResourceIcons.fireIconSprite);
                break;
            case ResourceType.Air:
                SetAllCardBackGroundColors(API_CardColors.airColor);
                break;
            case ResourceType.Earth:
                SetAllCardBackGroundColors(API_CardColors.earthColor);
                SetSprite(cardResourceIcon, allCardResourceIcons.earthIconSprite);
                break;
            case ResourceType.Water:
                SetAllCardBackGroundColors(API_CardColors.waterColor);
                SetSprite(cardResourceIcon, allCardResourceIcons.waterIconSprite);
                break;
            case ResourceType.Darkness:
                SetAllCardBackGroundColors(API_CardColors.darknessColor);
                break;
            case ResourceType.Light:
                SetAllCardBackGroundColors(API_CardColors.lightColor);
                break;


        }


    }

    private void SetAllCardBackGroundColors(Color _color)
    {
        SetBackGroundColor(artBackGround, _color);
        SetBackGroundColor(cardTitleBackGroundColor, _color);
        SetBackGroundColor(cardStatsBackGroundColor, _color);
        SetBackGroundColor(cardTypeBackGroundColor, _color);
    }

    private void SetBackGroundColor(ArtBackGround _artBackGround, Color _color)
    {
        _artBackGround.ImageRef.color = _color;

    }

    private void SetCardTitleName(string _title)
    {

        titleCardName.SetTitleName(cardInfo.CardTitle);
    }

    private void SetCardResourceCost(int _cost)
    {
        resourceCostText.Tmp.text = _cost.ToString();
    }

    private void SetCardAttackAmount(int _attack)
    {
        cardAttackAmount.Tmp.SetText(_attack.ToString());
    }
    private void SetCardHealthAmount(int _health)
    {
        cardHealthAmount.Tmp.SetText(_health.ToString());
    }
    private void SetSprite(Art _art, Sprite _sprite)
    {
        _art.SetSprite(_sprite);

    }

    private void SetCardSkillText(string _cardSkill)
    {
        //string _skillDesc = cardSkillDescription.Tmp.text;
        //if (_skillDesc == cardSkillDescription.PlaceHolderText)
        //    _skillDesc = string.Empty;
        //_skillDesc += "";
        //_skillDesc += _cardSkill;
        bool _isEmpty = false;
        if (cardSkillDescription.Tmp.text == cardSkillDescription.PlaceHolderText)
        { 
            cardSkillDescription.Tmp.text = "";
            _isEmpty = true;
        }
        cardSkillDescription.Tmp.text += _isEmpty ? "" + _cardSkill : "<br>" + _cardSkill;
        //cardSkillDescription.Tmp.text += _cardSkill;


    }
}

