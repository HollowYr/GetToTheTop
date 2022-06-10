using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out MovementSystem movement)) return;
        Debug.Log("Finished");
    }

}
