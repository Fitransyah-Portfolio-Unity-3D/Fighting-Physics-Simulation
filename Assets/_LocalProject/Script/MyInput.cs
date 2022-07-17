
using UnityEngine;

public class MyInput : MonoBehaviour
{
    bool spacePressed;
    public bool SpacePressed { get { return spacePressed; } }
    bool dPressed;
    public bool DPressed { get { return dPressed; } }
    bool rPressed;
    public bool RPressed { get { return rPressed; } }
    void Update()
    {
        spacePressed = Input.GetKeyDown(KeyCode.Space);
        dPressed = Input.GetKeyDown(KeyCode.D);   
        rPressed = Input.GetKeyDown (KeyCode.R);
    }
}
