
using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField] float timescale;
    void Start()
    {
        timescale = Time.timeScale; 
    }


    void Update()
    {
        Time.timeScale = timescale;
    }
}
