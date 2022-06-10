using System;
using UnityEngine;

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    None,
    Up,
    Down,
    Left,
    Right,
    UpRight,
    UpLeft,
    DownRight,
    DownLeft
}

public class SwipeSystem : MonoSingleton<SwipeSystem>
{
    private class GetCardinalDirections
    {
        public static readonly Vector2 Up = new Vector2(0, 1);
        public static readonly Vector2 Down = new Vector2(0, -1);
        public static readonly Vector2 Right = new Vector2(1, 0);
        public static readonly Vector2 Left = new Vector2(-1, 0);

        public static readonly Vector2 UpRight = new Vector2(1, 1);
        public static readonly Vector2 UpLeft = new Vector2(-1, 1);
        public static readonly Vector2 DownRight = new Vector2(1, -1);
        public static readonly Vector2 DownLeft = new Vector2(-1, -1);
    }

    private Vector3 startTouchPosition;
    private Vector3 endTouchPosition;

    public event Action<SwipeData> OnSwipe;

    private void OnEnable()
    {
        InputSystem.Instance.OnTouch += OnTouch;
        InputSystem.Instance.OnRelease += OnRelease;
    }

    private void OnDisable()
    {
        InputSystem.Instance.OnTouch -= OnTouch;
        InputSystem.Instance.OnRelease -= OnRelease;
    }

    private void OnTouch()
    {
        startTouchPosition = Input.mousePosition;
    }

    private void OnRelease()
    {
        endTouchPosition = Input.mousePosition;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        Vector3 currentSwipe = new Vector3(endTouchPosition.x - startTouchPosition.x, endTouchPosition.y - startTouchPosition.y);
        currentSwipe.Normalize();
        SendSwipe(FindSwipeDirection(currentSwipe));
    }

    private SwipeDirection FindSwipeDirection(Vector2 currentSwipe)
    {
        SwipeDirection direction = SwipeDirection.None;
        if (Vector2.Dot(currentSwipe, GetCardinalDirections.Up) > 0.906f)
        {
            direction = SwipeDirection.Up;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Down) > 0.906f)
        {
            direction = SwipeDirection.Down;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Left) > 0.906f)
        {
            direction = SwipeDirection.Left;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Right) > 0.906f)
        {
            direction = SwipeDirection.Right;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.UpRight) > 0.906f)
        {
            direction = SwipeDirection.UpRight;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.UpLeft) > 0.906f)
        {
            direction = SwipeDirection.UpLeft;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.DownLeft) > 0.906f)
        {
            direction = SwipeDirection.DownLeft;
        }
        else if (Vector2.Dot(currentSwipe, GetCardinalDirections.DownRight) > 0.906f)
        {
            direction = SwipeDirection.DownRight;
        }
        return direction;
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = startTouchPosition,
            EndPosition = endTouchPosition
        };
        OnSwipe?.Invoke(swipeData);
    }
}
