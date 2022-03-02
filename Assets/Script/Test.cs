using UnityEngine;
using RootMotion.FinalIK;

public class Test : MonoBehaviour
{
    public FullBodyBipedEffector[] effectors;
    public InteractionObject interactionObject;
    [SerializeField] InteractionSystem interactionSystem;
    void Start()
    {
        interactionSystem = GetComponent<InteractionSystem>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (interactionSystem == null) return;
            Debug.Log("Space is pressed!");
            foreach (FullBodyBipedEffector e in effectors)
            {
                interactionSystem.StartInteraction(e, interactionObject, true);
            }

        }
    }
}
