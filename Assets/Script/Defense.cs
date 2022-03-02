using RootMotion.Dynamics;
using UnityEngine;
using System.Collections;

public class Defense : MonoBehaviour
{
    public BehaviourPuppet puppet;
    public MyVRPuppet vrPuppet;
    
    [Header("LIFE")][SerializeField]
    int life = 100;
    float currentRegainPinSpeed = 1.5f;
    float currentUnpinWeightMlp = 0.5f;
    float currentKnockOutDistance = 1.5f;
    float blockingRegainPinSpeed = 10f;
    float blockingUnpinWeightMlp = 1f;
    float blockingKnockOutDistance = 2.5f;
    string stats = "Normal";

    [SerializeField] bool blockingOn = false;
    void Update()
    {
        Blocking();
    }
    public void HitBodyPart (Muscle.Group bodyGroup, float hitValue)
    {

        // methode for decrease life amount / called by hitdetection.cs
        //depending which body part got hit
        if (blockingOn) return;
        
        switch(bodyGroup)
        {
            case Muscle.Group.Head:
                Debug.Log(Muscle.Group.Head + " is hit!");
                Debug.Log("Life is decreased by " + Mathf.Round(hitValue).ToString());
                life -=  (int)Mathf.Round(hitValue) ;
                break;
            case Muscle.Group.Spine:
                Debug.Log(Muscle.Group.Spine + " is hit!");
                Debug.Log("Life is decreased by " + Mathf.Round(hitValue).ToString());
                life -= (int)Mathf.Round(hitValue);
                break;
        }
        UnpinPuppet();
    }
    void UnpinPuppet()
    {
        // methode adjusting PB variable depends on life amount
        if (life > 50)return;
        if (life <= 50 && life > 25)
        {
            stats = "Likely Unpin";
            currentRegainPinSpeed = 0.3f;
            currentUnpinWeightMlp = 0.3f;
            currentKnockOutDistance = 0.3f;
        }
        else if (life <=25 && life > 1)
        {
            stats = "Very Unpin";
            currentRegainPinSpeed = 0.25f;  
            currentUnpinWeightMlp = 0.2f;
            currentKnockOutDistance = 0.1f;
        }
        else if (life < 1)
        {
            stats = "Dead";
            currentRegainPinSpeed = 0.1f;
            currentUnpinWeightMlp = 0.05f;
            currentKnockOutDistance = 0.01f;
        }
        ApplyUnpinPuppet(currentRegainPinSpeed, currentUnpinWeightMlp, currentKnockOutDistance);
    }
    void ApplyUnpinPuppet(float newRegainPinSpeed, float newUnpinWeightMlp, float newKnockoutDistance)
    {   
        //methode modyfing PB variable
        puppet.regainPinSpeed = newRegainPinSpeed;
        puppet.unpinnedMuscleWeightMlp = newUnpinWeightMlp;
        puppet.knockOutDistance = newKnockoutDistance;  
    }
    #region BehaviourPuppetEvent
    public void OnLoseBalanceEvent()
    {
        // we can modify vr puppet variable here
        vrPuppet.blendOutTime = 0.05f;
        
        if (life > 1) return;
        if (life <= 1) puppet.puppetMaster.state = PuppetMaster.State.Dead; // this happen when life below 0 / change puppet state
        Debug.Log("Now is dead");
        // activate shader (soon)
        // destroy game object
        Destroy(this.transform.root.gameObject, 7f);
    }
    public void OnGetUpEvent() // local method 
    {
        // we can modify vr puppet variable here
    }
    public void OnRegainBalance()
    {
        // we can modify vr puppet variable here
        vrPuppet.blendOutTime = 0.75f;
    }
    #endregion
    void Blocking()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            blockingOn = true;
            // play blocking animation here
            ApplyUnpinPuppet(blockingRegainPinSpeed, blockingUnpinWeightMlp, blockingKnockOutDistance);
        }
        else
        {
            blockingOn = false;
            // blocking animator parameter false / transition to idle
            ApplyUnpinPuppet(currentKnockOutDistance, CurrentUnpinWeightMlp, currentKnockOutDistance);
        }
            
    }

    #region PublicProperties
    public int Life
    {
        get { return life; }
    }
    public float CurrentUnpinWeightMlp
    {
        get { return currentUnpinWeightMlp;  }
    }
    public float CurrentKnockOutDistance
    {
        get { return currentKnockOutDistance; } 
    }
    public string Stats
    {
        get { return stats; }
    }
    public bool BlockingOn
    {
        get { return blockingOn; }
    }
    #endregion

}
