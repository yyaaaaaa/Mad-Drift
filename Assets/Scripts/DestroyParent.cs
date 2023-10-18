using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
public void DestroyParentt()
    {
        GameObject parent = transform.parent.gameObject;
        Destroy(parent);
    }
}
