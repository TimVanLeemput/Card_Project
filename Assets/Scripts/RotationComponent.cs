using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RotationComponent : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 50;
    [SerializeField] bool canRotate = true;

    [SerializeField] GameObject pivotPoint = null;
    void Start()
    {
        
    }


    void SetCanRotate(bool _value)
    { 
        canRotate = _value;
    }
    public void Rotate()
    {
        Vector3 point = pivotPoint.transform.position; 
        Vector3 axis = pivotPoint.transform.up;
        transform.RotateAround(point, axis, rotationSpeed * Time.deltaTime);

       // transform.eulerAngles += transform.up * rotationSpeed * Time.deltaTime;
    }

    void Update()
    {
        if (!canRotate) return;
        Rotate();
        
    }
}
