using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
   
    public Transform target; //따라갈 목표
    public Vector3 offset; //위치 오프셋 
    
   

    
    void Update()
    {
        transform.position = target.position + offset; //타겟에서 오프셋 만큼 떨어져서 비춤
    }
}
