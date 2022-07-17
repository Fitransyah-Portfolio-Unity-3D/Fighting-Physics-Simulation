using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

public class PuppetCollisionHandler : MonoBehaviour {

    public BehaviourPuppet puppet;
    public int ragdollLayer = 9;

    private void Start()
    {
        puppet.OnCollision += OnCollision;
    }

    private void OnCollision(MuscleCollision m)
    {
        var muscle = puppet.puppetMaster.muscles[m.muscleIndex];
        Debug.Log("Collision with muscle: " + muscle.props.group);

        // Handle collisions depending on which muscle group was hit.
        switch(muscle.props.group)
        {
            // Play animation when there is a collision between the head of this puppet and any body part of another puppet
            case Muscle.Group.Head:
                if (m.collision.collider.attachedRigidbody == null) return;
                if (m.collision.gameObject.layer != ragdollLayer) return;

                // Make sure the other rigidbody belongs to a puppet
                var otherBroadcaster = m.collision.collider.attachedRigidbody.GetComponent<MuscleCollisionBroadcaster>();
                if (otherBroadcaster == null) return;

                // Discard if collision too weak
                if (m.collision.impulse.sqrMagnitude < 1f) return;

                // If you need to access the other muscle
                //var otherMuscle = otherBroadcaster.puppetMaster.muscles[otherBroadcaster.muscleIndex];

                // Play different hit reaction animation for hits from left and right side
                bool hitFromRight = Vector3.Dot(puppet.puppetMaster.targetRoot.right, m.collision.contacts[0].normal) < 0f; // Use GetContact(0) instead of contacts[0] in newer Unity versions (GC Alloc issue)
                if (hitFromRight)
                {
                    Debug.Log("Puppet " + puppet.puppetMaster.name + "'s head was hit from the right side.");
                    // Play head hit reaction from right animation (need to have an animation state with the same name in the Animator)
                    puppet.puppetMaster.targetAnimator.CrossFadeInFixedTime("HitHeadRight", 0.2f);
                } else
                {
                    Debug.Log("Puppet " + puppet.puppetMaster.name + "'s head was hit from the left side.");
                    // Play head hit reaction from left animation (need to have an animation state with the same name in the Animator)
                    puppet.puppetMaster.targetAnimator.CrossFadeInFixedTime("HitHeadLeft", 0.2f);
                }

                break;
        }

        


        
    }
    
}
