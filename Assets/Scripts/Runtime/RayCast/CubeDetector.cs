using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InspectionState
{
    NONE,
    IS_INSPECTING
}
public class CubeDetector : MonoBehaviour
{
    public Action<Transform> onCardLifted = null;
    public Action<Transform> onCardDropped = null;

    private float inspectionRange = 4f;

    private Vector3 initialCardPosition = new Vector3(0,0,0);


    [SerializeField] InspectionState inspectionState = InspectionState.NONE;
    [SerializeField] public Transform liftedCard = null;

    private void Start()
    {
        onCardLifted += SetLiftedCard;
    }
    private void SetLiftedCard(Transform _card)
    {
        liftedCard = _card;
    }
    private void LiftObject()
    {
        if (!liftedCard) return;
        Vector3 _newPos = liftedCard.position + new Vector3(0.2f, 0, 0);
        liftedCard.position = _newPos;

        onCardLifted?.Invoke(liftedCard);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;

        liftedCard = collision.transform;
        LiftObject();

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision == null) return;
        Vector3 _newPos = collision.gameObject.transform.position + new Vector3(-0.2f, 0, 0);
        collision.gameObject.transform.position = _newPos;
        Transform _lastLiftedCard = liftedCard;
        onCardDropped?.Invoke(_lastLiftedCard);
    }


    public void HandleInspectCard(InputAction.CallbackContext _callback)
    {
        if (liftedCard == null)
        {
            Debug.Log("Inspection failed ");
            return;
        }
        if (inspectionState == InspectionState.NONE)
        {
            InspectCard(Camera.main.transform.position, liftedCard.position);
        }
        else if (inspectionState == InspectionState.IS_INSPECTING)
        {

            StopInspectCard();
        } 
    }

    private void InspectCard(Vector3 _targetPos, Vector3 _initialPos)
    {
        Vector3 _toCamera = _targetPos - _initialPos;
        Vector3 _newPos = liftedCard.position + _toCamera.normalized * inspectionRange;
        Vector3 _lerpedPos = Vector3.Lerp(liftedCard.position, _newPos, 1f);
        liftedCard.position = _lerpedPos;
        Debug.Log($"CURRENT LIFTED POSITION {liftedCard.localPosition}");
        inspectionState = InspectionState.IS_INSPECTING;
        SetCardInitialPosition(liftedCard.localPosition);
    }
    

    public void StopInspectCard()
    {
        //if (inspectionState == InspectionState.IS_INSPECTING)
        
            liftedCard.position = new Vector3(0, 0, 0);
            inspectionState = InspectionState.NONE;
            Debug.Log("Stop inspect called");
        

    }

    private void SetCardInitialPosition(Vector3 _pos)
    {
        initialCardPosition = _pos;
    }

}
