using UnityEngine;
using UnityEngine.UI;
using RootMotion.Dynamics;
using System.Collections;

public class Stats : MonoBehaviour
{
    public Text[] stats;
    public PuppetMaster puppetMaster;
    public BehaviourPuppet puppet;
    int ragdollLayer = 9;
    public Defense defense;
    void Start()
    {
        puppet.OnCollision += OnCollision;
        UpdateStats();
    }
    private void Update()
    {
        StartCoroutine(UpdateStatsPerSecond());

        if (defense.Life <= 0) StopAllCoroutines();
    }
    void OnCollision(MuscleCollision m)
    {
        if (m.collision.collider.attachedRigidbody == null) return;
        if (m.collision.gameObject.layer != ragdollLayer) return;
        UpdateStats();
    }
    void UpdateStats()
    {
        stats[0].text = defense.Life.ToString();
        stats[1].text = defense.CurrentUnpinWeightMlp.ToString();
        stats[2].text = defense.CurrentKnockOutDistance.ToString();
        stats[3].text = defense.Stats;
        if (defense.BlockingOn == true) stats[4].text = BlockingStatus(true);
        else stats[4].text = BlockingStatus(false);
    }
    IEnumerator UpdateStatsPerSecond()
    {
        UpdateStats();  
        yield return new WaitForSeconds(1f) ;
    }
    string BlockingStatus (bool blockingStatus)
    {
        if (blockingStatus == true)
        {
            return "On";
        }
        else return "Off";
        
    }


}
