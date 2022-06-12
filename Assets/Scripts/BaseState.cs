using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour
{
    [SerializeField] private Material activeMat;
    [SerializeField] private Material inactiveMat;
    private Material startMat;
    private Renderer renderer;
    private bool isActive = false;
    private void Start()
    {
        renderer = GetComponent<Renderer>();
        startMat = renderer.material;
    }
    public void ChangeState(bool state)
    {
        if (state)
        {
            renderer.material = activeMat;
            isActive = true;
        }
        else
        {
            isActive = false;
            renderer.material = inactiveMat;
        }
    }

    public void ResetState()
    {
        isActive = false;
        renderer.material = startMat;
    }
}
