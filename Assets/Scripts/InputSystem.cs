using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputSystem : MonoSingleton<InputSystem>, IPointerDownHandler, IPointerUpHandler
{
    private Camera mainCamera;

    private Vector3 previousMousePosition;
    private Vector3 currentMousePosition;
    private Vector3 deltaMousePosition;

    private Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            return mainCamera;
        }
    }

    private bool IsCameraIncluded => MainCamera != null;

    public event Action OnTouch;
    public event Action OnRelease;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }

    public Vector2 GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            currentMousePosition = Input.mousePosition;
            deltaMousePosition = currentMousePosition - previousMousePosition;
            previousMousePosition = currentMousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            previousMousePosition = Vector3.zero;
            currentMousePosition = Vector3.zero;
            deltaMousePosition = Vector3.zero;
        }

        return deltaMousePosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsCameraIncluded)
        {
            OnTouch?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsCameraIncluded)
        {
            OnRelease?.Invoke();
        }
    }
}