using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickPlayerExample : MonoBehaviour
{
    private Joystick _joystick;
    private Vector2 _direction;
    [SerializeField] private Vector2GameEvent OnStickChangeDirection;

    [SerializeField] private Image _topLeftImage;
    [SerializeField] private Image _topRightImage;
    [SerializeField] private Image _bottomLeftImage;
    [SerializeField] private Image _bottomRightImage;

    private void Awake()
    {
        _joystick = GetComponent<Joystick>();
    }

    private void Update()
    {
        if (_joystick.Horizontal == _direction.x && _joystick.Vertical == _direction.y)
        {
            return;
        }

        ChangeDirection();
    }

    private void ChangeDirection()
    {
        _direction.Set(_joystick.Horizontal, _joystick.Vertical);
        HighlightDirection(_direction);
        OnStickChangeDirection.RaiseEvent(_direction);
    }

    private void HighlightDirection(Vector2 direction)
    {
        _topLeftImage.enabled = false;
        _topRightImage.enabled = false;
        _bottomLeftImage.enabled = false;
        _bottomRightImage.enabled = false;

        HighlightTopLeft(direction);
        HighlightTopRight(direction);
        HighlightBottomLeft(direction);
        HighlightBottomRight(direction);
    }

    private void HighlightBottomRight(Vector2 direction)
    {
        if (direction.x > -0.2 && direction.y < 0.2)
        {
            _bottomRightImage.enabled = true;
        }
    }

    private void HighlightBottomLeft(Vector2 direction)
    {
        if (direction.x < 0.2 && direction.y < 0.2)
        {
            _bottomLeftImage.enabled = true;
        }
    }

    private void HighlightTopRight(Vector2 direction)
    {
        if (direction.x > -0.2 && direction.y > -0.2)
        {
            _topRightImage.enabled = true;
        }
    }

    private void HighlightTopLeft(Vector2 direction)
    {
        if (direction.x < 0.2 && direction.y > -0.2)
        {
            _topLeftImage.enabled = true;
        }
    }
}