using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRayCast : MonoBehaviour {
    [SerializeField]
    private bool m_fromCamera;
    [SerializeField]
    private Transform m_camTransform;
    [SerializeField]
    private bool m_debug;
    [SerializeField]
    private Vector3 m_ofset;
    [SerializeField]
    private float m_maxDistance;
    [SerializeField]
    private float m_sendValue;
  
    

    void Update()
    {
        if (m_debug)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Fire();
            }
        }
    }

    public void Fire()
    {
        Vector3 direction = new Vector3();
        Vector3 startPos = new Vector3();
        RaycastHit hit;
        

        if (m_fromCamera)
        {
            if (!m_camTransform)
            {
                m_camTransform = Camera.main.transform;
            }
            direction = m_camTransform.forward;
            startPos = m_camTransform.position + m_ofset;
        }
        else
        {
            direction = transform.TransformDirection(Vector3.forward);
            startPos = transform.position + m_ofset;
        }
        
        if (Physics.Raycast(startPos, direction, out hit, m_maxDistance))
        {
           hit.collider.gameObject.BroadcastMessage("RayCastHit", m_sendValue, SendMessageOptions.DontRequireReceiver);
        }
    }

#if UNITY_EDITOR
    
    [SerializeField]
    private Color m_gizmoColour;

    void OnDrawGizmos()
    {
        // Display the path of the raycast when selected
        Gizmos.color = m_gizmoColour;

        //
        Vector3 startPos = new Vector3();
        Vector3 direction = new Vector3();

        if (m_fromCamera)
        {
            if (!m_camTransform)
            {
                m_camTransform = Camera.main.transform;
            }
            startPos = m_camTransform.position + m_ofset;
            direction = m_camTransform.forward;

        }
        else
        {
            startPos = transform.position + m_ofset;
            direction = transform.TransformDirection(Vector3.forward);
        }

        Vector3 endPos = GetPositionAtDistanceAlongDirectionVector(startPos, direction, m_maxDistance);
  
        Gizmos.DrawLine(startPos, endPos);
    }


    private Vector3 GetPositionAtDistanceAlongDirectionVector(Vector3 startPos, Vector3 direction, float distance)
    {
        Vector3 value = startPos + (distance * direction);
        return value;
    }

#endif

}
