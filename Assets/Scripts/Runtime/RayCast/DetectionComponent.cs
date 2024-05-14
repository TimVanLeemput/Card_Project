using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
/// <summary>
/// This component has to be used in conjunction with the CubeDetector component
/// This component uses a raycast from camera to a plane, the CubeDetector then does 
/// a raycast from the cube that is limited to the 2D plane, towards the card battlefield.
/// </summary>
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
  
    public void MouseDetect(InputAction _input)
    {
        if (_input == null)
        {
            Debug.Log("No valid inputs found");
            return;
        }

        Vector2 _mousePos = _input.ReadValue<Vector2>();
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
}

