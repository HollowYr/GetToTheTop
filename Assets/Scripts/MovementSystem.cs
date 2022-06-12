using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private Transform rotationRef;
    [SerializeField] private float rotateSpeed = 1f;
    [SerializeField] private LayerMask walkableMask;
    [SerializeField] private CameraRotation cameraRotation;

    List<BaseState> states = new List<BaseState>();
    private bool isMoving = false;
    Renderer renderer;
    private float rayDistance = .6f;
    Camera camera;

    private void Start()
    {
        camera = Camera.main;
        renderer = GetComponent<Renderer>();
        PaintAllSides();
    }
    Ray debugRay;
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            debugRay = ray;
            if (!Physics.Raycast(ray, out RaycastHit hit, 100, walkableMask)) return;
            Vector3 diff = hit.transform.position - transform.position;
            Debug.Log(diff);
            if (MathF.Abs(diff.x) == 1) RollInDirection(Vector3.right * diff.x);
            if (MathF.Abs(diff.z) == 1) RollInDirection(Vector3.forward * diff.z);
        }

        if (Input.GetKeyDown(KeyCode.D)) RollInDirection(Vector3.right);
        else
        if (Input.GetKeyDown(KeyCode.A)) RollInDirection(Vector3.right * -1);
        else
        if (Input.GetKeyDown(KeyCode.W)) RollInDirection(Vector3.forward);
        else
        if (Input.GetKeyDown(KeyCode.S)) RollInDirection(Vector3.forward * -1);
    }

    private void PaintAllSides()
    {
        foreach (BaseState state in states)
        {
            state.ResetState();
        }
        PaintSide(Vector3.forward);
        PaintSide(Vector3.back);
        PaintSide(Vector3.left);
        PaintSide(Vector3.right);
    }

    private void PaintSide(Vector3 direction)
    {
        Vector3 position;
        GetRaycastPosition(direction, out position);
        Ray ray = new Ray(position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, walkableMask))
        {
            if (hit.transform.TryGetComponent(out BaseState state))
            {
                states.Add(state);
                state.ChangeState(true);
            }

            return;
        }

        position = position + direction * rayDistance;
        ray = new Ray(position, Vector3.down);
        if (Physics.Raycast(ray, out hit, rayDistance * 2, walkableMask))
        {
            if (hit.transform.TryGetComponent(out BaseState state))
            {
                states.Add(state);
                state.ChangeState(true);
            }

            return;
        }

    }

    private bool CheckSides(Vector3 direction, out Vector3 anchorPos, out int angle)
    {
        Vector3 position;
        GetRaycastPosition(direction, out position);

        anchorPos = Vector3.down;
        angle = 90;
        Ray ray = new Ray(position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, walkableMask))
        {
            anchorPos = Vector3.up;
            angle *= 2;
            return true;
        }

        position = position + direction * rayDistance;
        ray = new Ray(position, Vector3.down);
        if (Physics.Raycast(ray, out hit, rayDistance * 2, walkableMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RollInDirection(Vector3 direction)
    {
        if (!CheckSides(direction, out Vector3 anchorPos, out int angle) || isMoving) return;
        float sideSize = Vector3.Scale(renderer.bounds.size, direction).Abs().GetValueByDirection(direction) / 2;
        Vector3 axis = Vector3.Cross(Vector3.up, direction);
        Vector3 anchor = transform.position + (anchorPos + direction) * sideSize;
        debugPos = anchor;
        anchor.y = (anchorPos == Vector3.down) ? renderer.bounds.min.y : renderer.bounds.max.y;
        StartCoroutine(RollRoutine(anchor, axis, angle));
    }

    IEnumerator RollRoutine(Vector3 anchor, Vector3 axis, int angle = 90)
    {
        isMoving = true;
        for (int i = 0; i < (angle / rotateSpeed); i++)
        {
            transform.RotateAround(anchor, axis, rotateSpeed);
            yield return new WaitForFixedUpdate();
        }
        PaintAllSides();
        RoundPlayerPosition();
        isMoving = false;
    }

    private void RoundPlayerPosition()
    {
        Vector3 roundedPosition = transform.position;
        roundedPosition.x = MathF.Round(roundedPosition.x);
        //roundedPosition.y = MathF.Round(roundedPosition.y);
        roundedPosition.z = MathF.Round(roundedPosition.z);
        transform.position = roundedPosition;
    }

    Vector3 debugPos;
    private void OnDrawGizmos()
    {
        if (renderer == null) renderer = GetComponent<Renderer>();

        Gizmos.color = Color.green;
        Gizmos.DrawRay(debugRay);
        Gizmos.DrawSphere(debugPos, .1f);
        //Vector3 position;
        //GetRaycastPosition(Vector3.right, out position);

        //Ray ray = new Ray(position, Vector3.right);
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(position, .1f);
        //Gizmos.DrawRay(ray);
        //position = position + Vector3.right * rayDistance;
        //ray = new Ray(position, Vector3.down);
        //Gizmos.DrawRay(ray);

        //GetRaycastPosition(Vector3.left, out position);


        //ray = new Ray(position, Vector3.left);
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(position, .1f);
        //Gizmos.DrawRay(ray);
        //position = position + Vector3.left * rayDistance;
        //ray = new Ray(position, Vector3.down);
        //Gizmos.DrawRay(ray);
        //GetRaycastPosition(Vector3.forward, out position);

        //ray = new Ray(position, Vector3.forward);
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(position, .1f);
        //Gizmos.DrawRay(ray);
        //position = position + Vector3.forward * rayDistance;
        //ray = new Ray(position, Vector3.down);
        //Gizmos.DrawRay(ray);

        //GetRaycastPosition(Vector3.back, out position);

        //ray = new Ray(position, Vector3.back);
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(position, .1f);
        //Gizmos.DrawRay(ray);
        //position = position + Vector3.back * rayDistance;
        //ray = new Ray(position, Vector3.down);
        //Gizmos.DrawRay(ray);

    }

    private void GetRaycastPosition(Vector3 direction, out Vector3 position)
    {
        position = renderer.bounds.center;
        Vector3 pointPos = renderer.bounds.max;
        if (HasNegative(direction)) pointPos -= renderer.bounds.size;

        Vector3 scale = Vector3.Scale(direction.Abs(), pointPos);
        position.x = (scale.x == 0) ? position.x : scale.x;
        position.y = (scale.y == 0) ? position.y : scale.y;
        position.z = (scale.z == 0) ? position.z : scale.z;
        position -= direction * .1f;

        bool HasNegative(Vector3 vector) => vector != vector.Abs();
    }
}
