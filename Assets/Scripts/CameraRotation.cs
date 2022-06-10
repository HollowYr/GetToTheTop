using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private float duration = 1;
    float angle = 90;
    bool isFinished = true;
    void Update()
    {
        if (!isFinished) return;
        if (Input.GetKeyDown(KeyCode.Q)) RotateBase(Vector3.left);
        else
        if (Input.GetKeyDown(KeyCode.E)) RotateBase(Vector3.right);
    }

    private void RotateBase(Vector3 direction)
    {
        isFinished = false;
        float angle = (direction.x > 0) ? this.angle : -this.angle;
        Vector3 endRotation = transform.rotation.eulerAngles + Vector3.up * angle;
        transform.DORotate(endRotation, duration, RotateMode.FastBeyond360)
            .OnComplete(() => isFinished = true);
    }
}
