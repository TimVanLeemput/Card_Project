using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
[ExecuteAlways]
public class CardManager : MonoBehaviour
{
	public static List<CardCreator> allCards = new List<CardCreator>();
	[Range(0f, 1f)] public float curveHeight = 0.5f;
	[Range(0f, 5f)] public float curveWidth = 1f;
	public Color curveColor = Color.white;

	private void Start()
	{
		allCards.ForEach(s => Debug.Log("All Cards contains = " + s.name));
	}
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		foreach (CardCreator _card in allCards)
		{
			Gizmos.color = Color.cyan;
			// Gizmos.DrawLine(_card.gameObject.transform.position, transform.position);
			// Handles.DrawAAPolyLine(transform.position, _card.transform.position);
			Vector3 _managerPos = transform.position;
			Vector3 _cardPos = _card.transform.position;
			float _halfHeight = (_managerPos.y - _cardPos.y) * curveHeight;

			Vector3 _offSet = Vector3.up * _halfHeight;
			Vector3 _controlPointA = _managerPos - _offSet;
			Vector3 _controlPointB = _cardPos + _offSet;
			Vector3 _centerOfCurve = (_managerPos + _cardPos + _controlPointA + _controlPointB) / 4;
			float _zLabelOffset = 0.2f;
			Handles.DrawBezier(_managerPos, _cardPos,
			 _controlPointA,
			  _controlPointB,
			  curveColor,
			  EditorGUIUtility.whiteTexture,
			  curveWidth);

			string _cardName = _card.transform.name;
			Vector3 _zOffset = new Vector3(0,0,_zLabelOffset);
			Handles.Label(_centerOfCurve - _zOffset, _cardName);
			Gizmos.color = Color.white;
		}
	}
#endif
}