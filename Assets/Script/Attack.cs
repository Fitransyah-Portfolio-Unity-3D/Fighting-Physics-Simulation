using UnityEngine;
using System.Collections;
using RootMotion.Dynamics;

public class Attack : MonoBehaviour
{

    // general caching
    public PuppetMaster puppetMaster;
    public BehaviourPuppet puppet;
    private MyInput myInput;
    Animator animator;

    #region Combo
    //field
    int comboOneA, comboOneB, comboOneC, comboTwoA, comboTwoB, comboTwoC;
    //state
    [SerializeField] bool secondCombo,thirdCombo;
    #endregion

    #region Dying
    //field
    [Tooltip("The speed of fading out PuppetMaster.pinWeight.")][SerializeField] [Range(0, 10f)] 
    float fadeOutPinWeightSpeed = 5f;
    [Tooltip("The speed of fading out PuppetMaster.muscleWeight.")][SerializeField][Range(0,10f)]
    float fadeOutMuscleWeightSpeed = 5f;
    [Tooltip("The muscle weight to fade out to.")][SerializeField][Range(0,1f)] 
    float deadMuscleWeight = 0.3f;
    int deadAnimation, mainStanceIdle;
    //caching
    private Vector3 defaultPosition;
    private Quaternion defaultRotation = Quaternion.identity;
    //state
    [SerializeField] bool isDead;
    [SerializeField] bool attacking;
    #endregion
    void Awake()
    {
        comboOneA = Animator.StringToHash("C1 A-Kick M-M");
        comboOneB = Animator.StringToHash("C1 Sabit M-M");
        comboOneC = Animator.StringToHash("C1 TKick M-M");
        comboTwoA = Animator.StringToHash("C2 AKick S-M");
        comboTwoB = Animator.StringToHash("C2 Putar S-M");
        comboTwoC = Animator.StringToHash("C2 Sabit S-M");
        deadAnimation = Animator.StringToHash("Die Backwards 0");
    }
    void Start()
    {
        animator = puppetMaster.targetAnimator.GetComponent<Animator>();
        myInput = GameObject.Find("MyInput").GetComponent<MyInput>();
        puppet = puppetMaster.transform.root.GetComponentInChildren<BehaviourPuppet>();

        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }

   
    void Update()
    {
        DynamicCombo();
        //SimpleCombo();

        Dying();  
    }
    #region ComboCode
    void DynamicCombo()
    {
        //if (thirdCombo && myInput.SpacePressed) Debug.Log("Combo 3 exectude!");
        if (secondCombo && myInput.SpacePressed)
        {
            if (thirdCombo) return;
            int x = Random.Range(1, 4);
            switch (x)
            {
                case (1):
                    animator.CrossFadeInFixedTime(comboTwoA, 0.2f);
                    break;
                case (2):
                    animator.CrossFadeInFixedTime(comboTwoB, 0.2f);
                    break;
                case (3):
                    animator.CrossFadeInFixedTime(comboTwoC, 0.2f);
                    break;
            }
            thirdCombo = true;
        }
        else if (myInput.SpacePressed)
        {
            if (secondCombo) return;
            if (thirdCombo) return;
            int x = Random.Range(1, 4);
            switch (x)
            {
                case (1):
                    animator.CrossFadeInFixedTime(comboOneA, 0.2f);
                    break;
                case (2):
                    animator.CrossFadeInFixedTime(comboOneB, 0.2f);
                    break;
                case (3):
                    animator.CrossFadeInFixedTime(comboOneC, 0.2f);
                    break;
            }
            secondCombo = true;
        }
        attacking = true;
    }
    void SimpleCombo()
    {
        if (secondCombo && myInput.SpacePressed)
        {
            animator.CrossFadeInFixedTime(comboTwoB, 0.2f);
        }
        else if (myInput.SpacePressed)
        {
            if (secondCombo) return;
            animator.CrossFadeInFixedTime(comboOneB, 0.2f);
            secondCombo = true;
        }
        attacking = true;
    }
    public void CancelCombo() // this one called in every animation event
    {
        secondCombo = false;
        thirdCombo = false;
        attacking = false;
    }
#endregion
    #region DyingCode
    private void Dying()
    {
        if (myInput.DPressed)
        {
            animator.CrossFadeInFixedTime(deadAnimation, 0.2f);

            if (puppetMaster != null)
            {
                StopAllCoroutines();
                StartCoroutine(FadeOutPinWeight());
                StartCoroutine(FadeOutMuscleWeight());
            }

            isDead = true;
        }

        if (myInput.RPressed && isDead)
        {

            transform.position = defaultPosition;
            transform.rotation = defaultRotation;

            animator.Play("Main Stance Idle", 0, 0f);

            if (puppetMaster != null)
            {
                StopAllCoroutines();
                puppetMaster.pinWeight = 1f;
                puppetMaster.muscleWeight = 1f;
            }
            isDead = false;
        }
    }
    #endregion

    private IEnumerator FadeOutPinWeight()
    {
        
        while (puppetMaster.pinWeight > 0f)
        {
            puppetMaster.pinWeight = Mathf.MoveTowards(puppetMaster.pinWeight, 0f, Time.deltaTime *fadeOutPinWeightSpeed);
            yield return null;
        }
    }

    private IEnumerator FadeOutMuscleWeight ()
    {
        while (puppetMaster.muscleWeight > 0f)
        {
            puppetMaster.muscleWeight = Mathf.MoveTowards(puppetMaster.muscleWeight, deadMuscleWeight, Time.deltaTime * fadeOutMuscleWeightSpeed);
            yield return null;
        }
    }
}
