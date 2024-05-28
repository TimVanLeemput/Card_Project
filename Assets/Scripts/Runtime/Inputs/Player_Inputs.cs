using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Inputs : MonoBehaviour
{
    MyInputs controls = null;
    public InputAction mousePos = null;
    public InputAction mouseLeftClick = null;

    private DetectionComponent detectionComponent = null;
    private CubeDetector cubeDetector = null;

    private void Start()
    {
        if(cubeDetector)
        mouseLeftClick.performed += cubeDetector.HandleInspectCard;
    }

    private void Awake()
    {
        controls = new MyInputs();
        detectionComponent = GetComponent<DetectionComponent>();
        cubeDetector = GetComponentInChildren<CubeDetector>();
    }
    private void OnEnable()
    {
        mousePos = controls.Player.MousePos;
        mousePos.Enable();
        mouseLeftClick = controls.Player.MouseLeftClick;
        mouseLeftClick.Enable();
    }

    private void FixedUpdate()
    {
        if (!detectionComponent) return;
        detectionComponent.MouseDetect(mousePos);
    }


}
