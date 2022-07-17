using System.Collections;
using UnityEngine;
using RootMotion.FinalIK;
using RootMotion.Dynamics;

public class MyInteractionControl : MonoBehaviour
{
    public PuppetMaster puppetMaster;
    public BehaviourPuppet puppet;
    public InteractionSystem interactionSystem;
    public InteractionObject interactionObject;
    public MyThrowingControl myThrowingControl;

    public int ragdollLayer = 9;

    bool interupt;
    [SerializeField] bool isGrabbing;
    public bool IsGrabbing
    {
        get { return isGrabbing; }
        set { isGrabbing = value; }
    }

    void Awake()
    {
        puppet = puppetMaster.transform.parent.GetComponentInChildren<BehaviourPuppet>();
        myThrowingControl = puppetMaster.GetComponent<MyThrowingControl>();
    }
    void Start()
    {

    }
    void Update()
    {
        // where to put this code for start interaction, maybe in other place / script?
        bool spacePressed = Input.GetKeyDown(KeyCode.Space);
        bool ePressed = Input.GetKeyDown(KeyCode.E);
        if (spacePressed && !isGrabbing)
        {
            Grabbing();
        }

        if (spacePressed && myThrowingControl.JointLocked == true)
        {
            puppetMaster.targetAnimator.CrossFadeInFixedTime("Goalie Throw", 0.25f);
        }
        
    }
    void Grabbing()
    {
        
        interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, interactionObject, interupt = false);
    }
    public void Releasing()
    {
        
        myThrowingControl.UnlockingJoint();
        
    }
    public void OnGrabbing()
    {
        Debug.Log("OnGrabbing is executed from " + transform.name );
    } 
    #region Fader
    IEnumerator FadeInEffector()
    {      
        yield return null;
    }
    IEnumerator FadeOutEffector()
    {
        yield return null;
    }
    #endregion
}
