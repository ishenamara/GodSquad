using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectExample : MonoBehaviour {

    [SerializeField]
    private GameObject m_target;

    public void EconomyUsed(int id)
    {

        switch (id)
        {
            case 0:

                Debug.Log("Got ID zero");

                break;

            case 1:

                m_target.SetActive(true);

                break;

            default:

                Debug.Log("Got an unhandled case");

                break;
        }


    }

}
