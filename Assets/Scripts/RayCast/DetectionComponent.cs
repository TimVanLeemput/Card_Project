using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class DetectionComponent : MonoBehaviour
{
    Player_Inputs inputs = null;
    Ray screenRay;
    float detectionDistance = 100;
    RaycastHit cardHit;
    bool cardHitBool = false;
    bool cardMoved = false;
    [SerializeField] LayerMask cardMask = 0;
    [SerializeField] float cardLiftDistance = 0.2f;
    [SerializeField] GameObject cardDetectionCube = null;

    void Start()
    {
        Init();

    }

    void Update()
    {

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

        float _clampedDistance = Math.Clamp(cardLiftDistance, 0, 0.2f);

        Debug.DrawRay(screenRay.origin, screenRay.direction * 20);
        if (_hit)
        {
            
            Color _rayColor = Color.green;
            Debug.DrawRay(screenRay.origin, screenRay.direction * 20, _rayColor);
            
            cardHitBool = _hit;
            cardHit = _cardHitResult;
            cardDetectionCube.transform.position = _cardHitResult.point;
            Debug.Log("Card hit with RAYCAST");

            //    if (!cardMoved)
            //    {
            //        Vector3 _newPos1 = new Vector3(_clampedDistance, 0, 0);
            //        Vector3 _newPos = _cardHitResult.transform.position + _newPos1;
            //        _cardHitResult.transform.position = _newPos;
            //        cardMoved = true;
            //    }

            //}
            //else
            //{
            //    //_rayColor = Color.red;
            //    if (!cardHit.transform || !cardMoved) return;
            //    Vector3 _newPos = cardHit.transform.position + new Vector3(-_clampedDistance, 0, 0);
            //    cardHit.transform.position = _newPos;
            //    cardHitBool = false;
            //    cardMoved = false;
            //}


        }
    }
    private void OnDrawGizmos()
    {
        if (cardHitBool && cardHit.transform != null)
        {
            Gizmos.color = TVL_Colors.Colors.BurlyWood;
            Gizmos.DrawCube(cardHit.point, Vector3.one * 0.5f);
            Gizmos.color = Color.white;
        }
    }

}

