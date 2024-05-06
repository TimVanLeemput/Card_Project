using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class DetectionComponent : MonoBehaviour
{
    Player_Inputs inputs = null;

    float detectionDistance = 100;

    bool cardHitBool = false;
    bool cardMoved = false;

    Ray screenRay;
    RaycastHit cardHit;

    [SerializeField] float cardLiftDistance = 0.2f;
    [SerializeField] LayerMask cardMask = 0;
    [SerializeField] GameObject cardDetectionCube = null;

    

    void Start()
    {
        Init();

    }
    private void FixedUpdate()
    {
        MouseDetect();
    }
    void Init()
    {
        inputs = GetComponent<Player_Inputs>();
    }
    public void MouseDetect()
    {
        if (!inputs)
        {
            Debug.Log("No valid inputs found");
            return;
        }

        Vector2 _mousePos = inputs.mousePos.ReadValue<Vector2>();
        Vector3 _pos = new Vector3(_mousePos.x, _mousePos.y, detectionDistance);
        screenRay = Camera.main.ScreenPointToRay(_pos);

        bool _hit = Physics.Raycast(screenRay, out RaycastHit _cardHitResult, detectionDistance, cardMask);

        Debug.DrawRay(screenRay.origin, screenRay.direction * 20);
        if (_hit)
        {
            
            Color _rayColor = Color.green;
            Debug.DrawRay(screenRay.origin, screenRay.direction * 20, _rayColor);
            
            cardHitBool = _hit;
            cardHit = _cardHitResult;
            cardDetectionCube.transform.position = _cardHitResult.point;
        }
    }
    private void OnDrawGizmos()
    {
        //if (cardHitBool && cardHit.transform != null)
        //{
        //    Gizmos.color = TVL_Colors.Colors.BurlyWood;
        //    Gizmos.DrawCube(cardHit.point, Vector3.one * 0.5f);
        //    Gizmos.color = Color.white;
        //}
    }

}

