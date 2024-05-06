using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDetector : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        Debug.Log("Entered collision");
        Vector3 _newPos = collision.gameObject.transform.position + new Vector3(0.2f,0,0);
        collision.gameObject.transform.position = _newPos;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision == null) return;
        Debug.Log("Exited collision");
        Vector3 _newPos = collision.gameObject.transform.position + new Vector3(-0.2f, 0, 0);
        collision.gameObject.transform.position = _newPos;
    }
}
