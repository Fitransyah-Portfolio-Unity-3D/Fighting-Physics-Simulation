using UnityEngine;
using RootMotion.Dynamics;
public class HitDetection : MonoBehaviour
{
    //public FullBodyBipedIK ik;
    //public AimIK aim;
    public BehaviourPuppet puppet;


    [SerializeField] int ragdollLayer = 9;
    [Range(10,500)] [SerializeField]
    float minimumImpulse = 100f;
    private const float headDamage = 7f;
    private const float spineDamage = 5f;

    Defense defense;

    #region RemoveLater
    Vector3 targetAnimatorStartingPosition;
    Quaternion targetAnimatorStartingRotation;
    Transform targetAnimatorTransform;
    #endregion


    void Start()
    {
        puppet.OnCollisionImpulse += OnCollisionImpulse;
        defense = GetComponent<Defense>();
        #region RemoveLater
        targetAnimatorTransform = puppet.puppetMaster.targetAnimator.gameObject.GetComponent<Transform>();
        targetAnimatorStartingPosition = targetAnimatorTransform.position;
        targetAnimatorStartingRotation = targetAnimatorTransform.rotation;
        #endregion
    }

    void Update()
    {
        RemoveLater();        
    }
    void OnCollisionImpulse (MuscleCollision m, float impulse)
    {
        if (m.collision.collider.attachedRigidbody == null) return;
        if (m.collision.gameObject.layer != ragdollLayer) return;

        var otherBroadcaster = m.collision.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>();
        if (otherBroadcaster == null) return;

        if (impulse < minimumImpulse) return;
        if (defense.Life < 1) return;
        Muscle muscle = puppet.puppetMaster.muscles[m.muscleIndex];
        switch(muscle.props.group)
        {
            case Muscle.Group.Head:
                //call methode with hit value
                defense.HitBodyPart(muscle.props.group, headDamage);
                break;
            case Muscle.Group.Spine:
                //call methode with hit value
                defense.HitBodyPart(muscle.props.group, spineDamage);
                break;
        }
    }
    #region RemoveLater
    void RemoveLater()
    {
        // resetting position for testing purpose
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            targetAnimatorTransform.position = targetAnimatorStartingPosition;
            targetAnimatorTransform.rotation = targetAnimatorStartingRotation;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) puppet.puppetMaster.state = PuppetMaster.State.Frozen;
        if (Input.GetKeyDown(KeyCode.Alpha2)) puppet.SetState(BehaviourPuppet.State.Unpinned);
    }
    #endregion
}
