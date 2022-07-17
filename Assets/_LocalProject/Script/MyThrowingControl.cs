using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using RootMotion.FinalIK;

public class MyThrowingControl : MonoBehaviour //this script should located at PuppetMaster gameobject
{
    public PuppetMaster puppetMaster;
    public BehaviourPuppet puppet;
    public PuppetMaster otherPuppetMaster;
    public BehaviourPuppet otherPuppet;
    public MyInteractionControl myInteractionControl = null;

    private ConfigurableJoint joint;
    private const float massMlp = 5f;
    private const int solverIterationMlp = 10;
    private int ragdollLayer = 9;
    public Vector3 jointAnchor;

    Rigidbody r;
    Collider c;
    Collider otherCollider;

    [SerializeField] bool jointLocked;

    public bool JointLocked
    {
        get { return jointLocked; }
        set { jointLocked = value; }    
    }

    void Start()
    {
        puppet.OnCollision += GrabCollision;
        myInteractionControl = puppetMaster.targetAnimator.GetComponent<MyInteractionControl>();
    }
    void Update()
    {
        bool spacePressed = Input.GetKeyDown(KeyCode.Space);
        bool ePressed = Input.GetKeyDown(KeyCode.E);
    }
    void GrabCollision(MuscleCollision m)
    {
        if (myInteractionControl.IsGrabbing == true) return;   
        if (m.collision.gameObject.layer != ragdollLayer) return;
        if (m.collision.transform.root.name == this.transform.root.name) return;
        if (m.collision.collider.attachedRigidbody == null) return;

        var otherBroadcaster = m.collision.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>();
        if (otherBroadcaster == null) return;
        if (otherBroadcaster.transform.root.name == this.transform.root.name) return;

        if (puppet.puppetMaster.muscles[m.muscleIndex].props.group != Muscle.Group.Hand) return;

        Vector3 spineToCollision = puppet.puppetMaster.muscles[0].transform.position - puppet.puppetMaster.muscles[m.muscleIndex].transform.position;
        bool leftHand = spineToCollision.x > Mathf.Epsilon;

        if (leftHand)
        {
            Debug.Log("touching left Hand");
            var holderCollider = puppet.puppetMaster.muscles[m.muscleIndex].transform.GetComponent<Collider>();
            LockingJoint(holderCollider, otherBroadcaster, puppet, m.collision.collider);
            

        }
        else if (!leftHand)
        {
            Debug.Log("touching right hand");
            var holderCollider = puppet.puppetMaster.muscles[m.muscleIndex].transform.GetComponent<Collider>();
            //GettingHand(holderCollider, otherBroadcaster, puppet, m.collision.collider.attachedRigidbody, m.collision.collider);
        }
        myInteractionControl.IsGrabbing = true;
    }
    public void LockingJoint(Collider holderHandCollider, MuscleCollisionBroadcaster holdedBodyPart, BehaviourPuppet thisPuppet, Collider enemyCollider)
    {

        if (jointLocked) return;
        if (puppetMaster == null) puppetMaster = thisPuppet.puppetMaster;
        if (puppet == null) puppet = thisPuppet;


        // code
        Debug.Log("Locking Joint executed from " + this.transform.root.name);

        if (holdedBodyPart.puppetMaster == puppetMaster) return;

        foreach (BehaviourBase b in holdedBodyPart.puppetMaster.behaviours)
        {
            if (b is BehaviourPuppet)
            {
                otherPuppet = b as BehaviourPuppet;
                otherPuppet.SetState(BehaviourPuppet.State.Unpinned);
                otherPuppet.canGetUp = false;
            }
        }

        if (otherPuppet == null) return;

        c = holderHandCollider;
        joint = c.gameObject.AddComponent<ConfigurableJoint>();

        otherCollider = enemyCollider;
        joint.connectedBody = otherCollider.attachedRigidbody;
        joint.anchor = jointAnchor;

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;

        r = c.attachedRigidbody;
        r.mass *= massMlp;

        puppetMaster.solverIterationCount *= solverIterationMlp;

        Physics.IgnoreCollision(c, otherCollider, true);
        jointLocked = true;
        
    }
    public void UnlockingJoint()
    {
        Destroy(joint);
        r.mass /= massMlp;
        puppetMaster.solverIterationCount /= solverIterationMlp;
        Physics.IgnoreCollision(c, otherCollider, false);

        otherPuppet.canGetUp = true;
        otherCollider = null;
        jointLocked = false;
        otherPuppet = null;

        myInteractionControl.IsGrabbing = false;
    }
}
