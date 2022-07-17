using RootMotion.FinalIK;
using UnityEngine;
using System.Collections;

public class MyGrabber : MonoBehaviour
{
    [SerializeField] FullBodyBipedIK grabberChar;
    [SerializeField] FullBodyBipedIK grabbedChar;


    [SerializeField] Transform offset;
    [SerializeField] Transform target;

    [SerializeField] [Range(0,1)]float crossFade;
    [SerializeField] float crossFadeSpeed;
    [SerializeField] [Range(0, 1)] float positionWeight;
    [SerializeField] [Range(0, 1)] float rotationWeight;

    [SerializeField] bool grabbingOn;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FadeInEffector());
        StartCoroutine(FadeOutEffector());
        

    }
    void LateUpdate()
    {
        //Vector3 position = Vector3.Lerp(offset.position, grabbedChar.solver.rightHandEffector.bone.position, crossFade);
        Vector3 position = Vector3.Lerp(offset.position, target.position, crossFade);
        grabberChar.solver.leftHandEffector.position = Vector3.Lerp(grabberChar.solver.leftHandEffector.position, position, Time.deltaTime * crossFadeSpeed);
        grabberChar.solver.leftHandEffector.rotation = Quaternion.Lerp(grabberChar.solver.leftHandEffector.rotation, target.rotation, Time.deltaTime * crossFadeSpeed);

        grabberChar.solver.leftHandEffector.positionWeight = positionWeight;
        grabberChar.solver.leftHandEffector.rotationWeight = rotationWeight;
    }
    IEnumerator FadeInEffector()
    {
        if (crossFade < 1 && grabbingOn)
        {
            crossFade = Mathf.MoveTowards(crossFade, 1f, Time.deltaTime * crossFadeSpeed);
            positionWeight = Mathf.MoveTowards(positionWeight, 1f, Time.deltaTime * crossFadeSpeed);
            rotationWeight = Mathf.MoveTowards(rotationWeight, 1f, Time.deltaTime * crossFadeSpeed);
        }
        yield return null;
    }
    IEnumerator FadeOutEffector()
    {
        if (crossFade > 0 && !grabbingOn)
        {
            crossFade = Mathf.MoveTowards(crossFade, 0f, Time.deltaTime * crossFadeSpeed);
            positionWeight = Mathf.MoveTowards(positionWeight, 0f, Time.deltaTime * crossFadeSpeed);
            rotationWeight = Mathf.MoveTowards(rotationWeight, 0f, Time.deltaTime * crossFadeSpeed);
        }
        yield return null;
    }
    // next work on the other hand

    // can we change the grablocator destination to puppet master collider
}
