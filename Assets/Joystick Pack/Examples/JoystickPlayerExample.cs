using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JoystickPlayerExample : MonoBehaviour
{
    private Joystick _joystick;
    private Vector2 _direction;
    [SerializeField] private Vector2GameEvent OnStickChangeDirection;

    private void Awake()
    {
        _joystick = GetComponent<Joystick>();
    }

    public void FixedUpdate()
    {
        if (_joystick.Horizontal != _direction.x && _joystick.Vertical != _direction.y)
        {
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        _direction.Set(_joystick.Horizontal, _joystick.Vertical);
        OnStickChangeDirection.RaiseEvent(_direction);
    }
}