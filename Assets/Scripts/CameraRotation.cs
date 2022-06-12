using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    //[SerializeField] private float duration = .6f;
    //float angle = 90;
    //bool isFinished = true;
    //void Update()
    //{
    //    if (!isFinished) return;
    //    if (Input.GetKeyDown(KeyCode.Q)) RotateBase(Vector3.left);
    //    else
    //    if (Input.GetKeyDown(KeyCode.E)) RotateBase(Vector3.right);
    //}

    //private void RotateBase(Vector3 direction)
    //{
    //    isFinished = false;
    //    float angle = (direction.x > 0) ? this.angle : -this.angle;
    //    Vector3 endRotation = transform.rotation.eulerAngles + Vector3.up * angle;
    //    transform.DORotate(endRotation, duration, RotateMode.FastBeyond360)
    //        .OnComplete(() => isFinished = true);
    //}
    [SerializeField] float speed = 1f;
    [SerializeField] float speedEditor = 2f;
    [SerializeField] private float smoothTime = .5f;
    [SerializeField] private Camera camera;
    [SerializeField] private float FOVmin = 30f;
    [SerializeField] private float FOVmax = 90f;
    [SerializeField] private float FOVStep = .01f;
    [SerializeField] private float minY = 40f;
    [SerializeField] private float maxY = 90f;

    float startFOV;
    float rotateX = 0f;
    float rotateY = 0f;
    bool isRotationg = false;

    private void Start()
    {
        rotateX = transform.eulerAngles.x;
        rotateY = transform.eulerAngles.y;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {

            return;
        }
        if (Input.touchCount == 2)
        {
            startFOV = camera.fieldOfView;
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            Vector2 firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            Vector2 secondtTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            float prevMagnitude = (firstTouchPrevPos - secondtTouchPrevPos).magnitude;
            float currentMagnitude = (firstTouch.position - secondTouch.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            if (difference == 0.0f) return;
            Zoom((int)difference * FOVStep);
        }
        else if (Input.GetMouseButton(0))
        {
#if UNITY_EDITOR_WIN
            rotateY += speedEditor * Input.GetAxis("Mouse X");
            rotateX -= speedEditor * Input.GetAxis("Mouse Y");
#else
            rotateY += speed * Input.touches[0].deltaPosition.x / Screen.width;
            rotateX -= speed * Input.touches[0].deltaPosition.y / Screen.height;
            //Debug.Log(rotateY + " " + rotateX + "\n" +
            //    Input.touches[0].position.x + " " + Input.touches[0].position.y);

            //Debug.Log(Screen.width + " " + Screen.height);
#endif
            rotateX = Mathf.Clamp(rotateX, minY, maxY);

            transform.eulerAngles = new Vector3(rotateX, rotateY, 0);
        }
    }

    private void Zoom(float value)
    {
        camera.fieldOfView = Mathf.Clamp(startFOV - value, FOVmin, FOVmax);
    }

}
