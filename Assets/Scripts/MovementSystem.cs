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

    private bool isMoving = false;
    Renderer renderer;
    private float rayDistance = .6f;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.D)) RollInDirection(Vector3.right);
        else
        if (Input.GetKeyDown(KeyCode.A)) RollInDirection(Vector3.right * -1);
        else
        if (Input.GetKeyDown(KeyCode.W)) RollInDirection(Vector3.forward);
        else
        if (Input.GetKeyDown(KeyCode.S)) RollInDirection(Vector3.forward * -1);
    }

    private bool CheckSides(Vector3 direction, out Vector3 anchorPos, out int angle)
    {
        Vector3 position;
        GetRaycastPosition(direction, out position);

        anchorPos = Vector3.down;
        angle = 90;
        Ray ray = new Ray(position, direction);
        if (Physics.Raycast(ray, rayDistance, walkableMask))
        {
            anchorPos = Vector3.up;
            angle *= 2;
            return true;
        }

        position = position + direction * rayDistance;
        ray = new Ray(position, Vector3.down);
        if (Physics.Raycast(ray, rayDistance * 2, walkableMask))
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
        if (!CheckSides(direction, out Vector3 anchorPos, out int angle)) return;
        float sideSize = Vector3.Scale(renderer.bounds.size, direction).Abs().GetValueByDirection(direction) / 2;
        Vector3 axis = Vector3.Cross(Vector3.up, direction);
        Vector3 anchor = transform.position + (anchorPos + direction) * sideSize;
        debug = anchor;
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
        isMoving = false;
    }
    Vector3 debug;
    private void OnDrawGizmos()
    {
        if (renderer == null) renderer = GetComponent<Renderer>();

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(debug, .1f);
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
