using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform; 
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        transform.position = new Vector3 (playerTransform.position.x,playerTransform.position.y+20, playerTransform.position.z);
    }
}
