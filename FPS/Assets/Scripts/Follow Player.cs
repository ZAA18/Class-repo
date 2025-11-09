using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float OffsetX;
    [SerializeField] private float OffsetZ;
    [SerializeField] private float LerpSpeed;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void LastUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(target.position.x + OffsetX, transform.position.y, target.position.z + OffsetZ), LerpSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
