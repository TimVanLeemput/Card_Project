using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Inputs : MonoBehaviour
{
    MyInputs controls = null;
    public InputAction mousePos = null;
    private void Awake()
    {
        controls = new MyInputs();
    }
    private void OnEnable()
    {
        mousePos = controls.Player.MousePos;
        mousePos.Enable();
    }


}
