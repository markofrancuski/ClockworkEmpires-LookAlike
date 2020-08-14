using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Camera _camera;
    [SerializeField] private bool _receiveInput = true;


    [Header("Camera Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _tempMoveSpeed;
    [SerializeField] private float _moveTimer;
    [Space]
    [SerializeField] private float _tempMoveTimer;


    [Header("Camera Zoom")]
    [SerializeField] private float _currentZoom;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;
    [SerializeField] private float _zoomStep;

    private Vector3 _clickMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        _zoomStep = (_maxZoom - _minZoom) / 10;

        // TODO: Remove
        _receiveInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_receiveInput) return;

        // Check Scroll
        CheckScrollInput();
        // Check Movement
        CheckCameraMoveInput();

    }

    private void CheckScrollInput()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            if (_currentZoom >= _maxZoom) return;
            _currentZoom += _zoomStep;
            UpdateCameraFOV();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (_currentZoom <= _minZoom) return;
            _currentZoom -= _zoomStep;
            UpdateCameraFOV();
        }
    }

    private void CheckCameraMoveInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float speedBonus = 0;
        Vector3 velocity = new Vector3(horizontal, 0, vertical).normalized;
        if (horizontal == 0 && vertical == 0)
        {
            velocity = Vector3.zero;
            _tempMoveTimer = 0;
        }
        else
        {
            if (_tempMoveTimer < _moveTimer) _tempMoveTimer += Time.deltaTime;
            else _tempMoveTimer = _moveTimer;

            // Percentage
            speedBonus = ((_tempMoveTimer * 100) / _moveTimer) / 100;
        }

        _tempMoveSpeed = _moveSpeed + (_moveSpeed * speedBonus);

        _rigidbody.velocity = velocity * _tempMoveSpeed;

    }

    private void UpdateCameraFOV()
    {
        _camera.fieldOfView = _currentZoom;
    }

    public void EnableInput() => _receiveInput = true;
    public void DisableInput() => _receiveInput = false;

}
