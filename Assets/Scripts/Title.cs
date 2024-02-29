using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Title : MonoBehaviour
{
    protected string placeHolderText = "Skill Text Holder";
    protected TextMeshProUGUI tmp = null;
    protected string tmpString = null;

    public event Action<TextMeshProUGUI> onInit = null;
    //
    public virtual TextMeshProUGUI Tmp
    {
        get { return tmp; }
        set { tmp = value; }
    }

    public string TmpString
    { get { return tmpString; } set { tmpString = value; } }

    public string PlaceHolderText
    {
        get { return placeHolderText; }
        set { placeHolderText = value; }
    }
    protected void Awake()
    {
        Init();
    }

    protected void Start()
    {
    }

    protected void Init()
    {
        tmp = GetComponent<TextMeshProUGUI>();

    }

    public void SetTitleName(string _string)
    {
        if (!tmp) return;
        tmp.text = _string;
    }

}
