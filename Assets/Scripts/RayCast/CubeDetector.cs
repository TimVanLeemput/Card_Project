using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDetector : MonoBehaviour
{
    public Action onCardLifted = null;
    public Action onCardDropped = null;

    Collision cardHit = null;
    private void LiftObject()
    {
        if (!cardHit.transform) return;
        Vector3 _newPos = cardHit.gameObject.transform.position + new Vector3(0.2f, 0, 0);
        cardHit.gameObject.transform.position = _newPos;

        onCardLifted?.Invoke();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        Debug.Log("Entered collision");
        cardHit = collision;
        LiftObject();

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision == null) return;
        Debug.Log("Exited collision");
        Vector3 _newPos = collision.gameObject.transform.position + new Vector3(-0.2f, 0, 0);
        collision.gameObject.transform.position = _newPos;
    }
}
