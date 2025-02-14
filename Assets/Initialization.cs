using UnityEngine;

public class Initialization : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PointSystem.Instance.Score = 0;
        PointSystem.Instance.Bonus1.isActive = false;
        PointSystem.Instance.Bonus2.isActive = false; 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
