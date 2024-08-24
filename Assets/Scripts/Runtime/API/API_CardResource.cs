using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static API_CardResource;

public class API_CardResource
{
    public enum CardResourceColors
    {
        None = 0,
        FireColor = 1,
        AirColor = 2,
        WaterColor = 4,
        EarthColor = 8,
        DarknessColor = 16,
        LigtColor = 32
    }

    private static CardResourceColors cardResourceColors = CardResourceColors.None;

    public Color GetCardResourceColor(CardInfo _cardInfo)
    {
        switch (_cardInfo.ResourceTypeRef)
        {
            case ResourceType.None:
                return Color.white;
        }

        if (_cardInfo.ResourceTypeRef == ResourceType.None)
            return _cardInfo.ResourceTypeColorIndicator = Color.white;

        if (_cardInfo.ResourceTypeRef == ResourceType.Fire)
            return _cardInfo.ResourceTypeColorIndicator = TVL_Colors.Colors.DeepRed;

        return Color.white;
        //switch (_cardInfo.ResourceTypeRef)
        //{
        //    case _cardInfo.ResourceTypeRef.HasFlag("None"):

        //}

        //    switch (_cardInfo.ResourceTypeRef)
        //    {
        //        case _cardInfo.ResourceTypeRef.HasFlag(0):
        //            return Color.white;
        //        case CardResourceColors.FireColor:
        //            return TVL_Colors.Colors.DeepRed;
        //        case CardResourceColors.AirColor:
        //            return TVL_Colors.Colors.SkyBlue;
        //        case CardResourceColors.WaterColor:
        //            return TVL_Colors.Colors.RoyalBlue1;
        //        case CardResourceColors.EarthColor:
        //            return TVL_Colors.Colors.BurlyWood;
        //        case CardResourceColors.DarknessColor:
        //            return TVL_Colors.Colors.DarkOrchid1;
        //        case CardResourceColors.LigtColor:
        //            return TVL_Colors.Colors.LightGoldenrod;
        //    }

        //    return Color.white;
        //}
    }
}
//if (_cardInfo.ResourceTypeRef.HasFlag(ResourceType.Fire))
//    return _cardInfo.ResourceTypeColorIndicator = TVL_Colors.Colors.DeepRed;