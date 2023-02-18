using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    public bool isEmpty = true;
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<Character>(out _))
        {
            isEmpty = true;
        }
    }
}
