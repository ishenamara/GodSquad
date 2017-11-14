using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveRayCast : MonoBehaviour {

    [SerializeField]
    private float m_value;

    public void RayCastHit(float value )
    {
        Debug.Log("Got Hit by a Raycast");
        m_value -= value;
    }

}
