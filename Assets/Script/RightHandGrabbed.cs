using System.Collections;
using UnityEngine;
using RootMotion.FinalIK;

public class RightHandGrabbed : MonoBehaviour
{
    public InteractionSystem interactionSystem;
    public InteractionObject interactionObject;

    void Start()
    {
        interactionObject = transform.parent.GetComponent<InteractionObject>();
    }
    void BeingTouched()
    {
        interactionSystem = interactionObject.lastUsedInteractionSystem;
        // from interaction system we can access the grabber other component such final IK and the intercation control script
        // call interaction control methode.cs OnGrabbing();
        MyInteractionControl ic = interactionSystem.GetComponent<MyInteractionControl>();
        ic.OnGrabbing(); // can use parameter here later if needed
        Debug.Log("Being touched is executed from "+ transform.name);

        // put some code here if needed
    }
}
