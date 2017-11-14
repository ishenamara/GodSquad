using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class TriggerController : MonoBehaviour
{
    public bool ignoreCollisions;
    public Object[] targets;
    public string[] triggerTags;
    // A multi-purpose script which causes an action to occur when
    // a trigger collider is entered.
    public enum Mode
    {
        Trigger = 0,    // Just broadcast the action on to the target
        Replace = 1,    // replace target with source
        Activate = 2,   // Activate the target GameObject
        Enable = 3,     // Enable a component
        Animate = 4,    // Start animation on target
        Deactivate = 5  // Decativate target GameObject
    }

    public Mode action = Mode.Trigger;         // The action to accomplish
    private Object target;                       // The game object to affect. If none, the trigger work on this game object
    public GameObject source;
    private int triggerCount = 1;
    public bool repeatTrigger = false;


    public void TripTriggerController()
    {
        triggerCount--;
        // Run the method on this gameObject
        if (targets.Length <= 0)
        {
            BroadcastTrigger();
        }
        else {
            // Loop through each target
            for (int i = 0; i < targets.Length; i++)
            {
                target = targets[i];
                BroadcastTrigger();

            }

        }
    }

    private void BroadcastTrigger()
    {
        
        if (triggerCount == 0 || repeatTrigger)
        {

            Object currentTarget = target ?? gameObject;
            Behaviour targetBehaviour = currentTarget as Behaviour;
            GameObject targetGameObject = currentTarget as GameObject;
            if (targetBehaviour != null)
            {
                targetGameObject = targetBehaviour.gameObject;
            }

            switch (action)
            {
                case Mode.Trigger:
                    if (targetGameObject != null)
                    {
                        targetGameObject.BroadcastMessage("ReceiveTrigger", SendMessageOptions.DontRequireReceiver);
                    }
                    break;
                case Mode.Replace:
                    if (source != null)
                    {
                        if (targetGameObject != null)
                        {
                            Instantiate(source, targetGameObject.transform.position,
                                        targetGameObject.transform.rotation);
                            DestroyObject(targetGameObject);
                        }
                    }
                    break;
                case Mode.Activate:
                    if (targetGameObject != null)
                    {
                        targetGameObject.SetActive(true);
                    }
                    break;
                case Mode.Enable:
                    if (targetBehaviour != null)
                    {
                        targetBehaviour.enabled = true;
                    }
                    break;
                case Mode.Animate:
                    if (targetGameObject != null)
                    {
                        targetGameObject.GetComponent<Animation>().Play();
                    }
                    break;
                case Mode.Deactivate:
                    if (targetGameObject != null)
                    {
                        targetGameObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Will check the tag of the object colliding, against the triggerTags array.
    /// If array is empty, will DoActivateTrigger() with any object collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!ignoreCollisions)
        {
            if (triggerTags.Length <= 0)
            {
                TripTriggerController();

            }
            else {
                for (int i = 0; i < triggerTags.Length; i++)
                {
                    if (other.gameObject.tag == triggerTags[i])
                    {
                        TripTriggerController();
                        break;
                    }

                }
            }
        }
    }

}