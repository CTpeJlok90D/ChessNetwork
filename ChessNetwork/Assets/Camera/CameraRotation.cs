using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform _cameraCenterTransform;
    [SerializeField] private Vector2 _maxCameraRotaion;
    [SerializeField] private Vector2 _minCameraRotation;
    private StandartInputActions _inputActions => StaticInput.Singletone;

    private void Awake()
    {
        _inputActions.MapSwither.Enable();
    }

    private void OnEnable()
    {
        _inputActions.MapSwither.StartCameraRotate.started += OnStartCameraRotate;
        _inputActions.MapSwither.StartCameraRotate.canceled += OnStopCameraRotate;
        _inputActions.CameraRotate.Rotate.started += OnCameraRotate;
        _inputActions.CameraRotate.Rotate.performed += OnCameraRotate;
    }
    private void OnDisable()
    {
        _inputActions.MapSwither.StartCameraRotate.started -= OnStartCameraRotate;
        _inputActions.MapSwither.StartCameraRotate.canceled -= OnStopCameraRotate;
        _inputActions.CameraRotate.Rotate.started -= OnCameraRotate;
        _inputActions.CameraRotate.Rotate.performed -= OnCameraRotate;
    }

    private void OnStartCameraRotate(InputAction.CallbackContext context)
    {
        _inputActions.Standart.Disable();
        _inputActions.CameraRotate.Enable();
    }
    private void OnStopCameraRotate(InputAction.CallbackContext context)
    {
        _inputActions.Standart.Enable();
        _inputActions.CameraRotate.Disable();
    }

    private void OnCameraRotate(InputAction.CallbackContext context)
    {
        Vector2 rotateOffcet = context.ReadValue<Vector2>();

        Vector3 newRotate = new Vector3(rotateOffcet.y + _cameraCenterTransform.transform.eulerAngles.x, rotateOffcet.x + _cameraCenterTransform.transform.eulerAngles.y, 0);
        for (int i = 0; i < 2; i++)
        {
            if (newRotate[i] < 180 && newRotate[i] > _maxCameraRotaion[i])
            {
                newRotate[i] = _maxCameraRotaion[i];
            }
        }
        for (int i = 0; i < 2; i++)
        {
            if (newRotate[i] > 180 && newRotate[i] < _minCameraRotation[i])
            {
                newRotate[i] = _minCameraRotation[i];
            }
        }
        _cameraCenterTransform.transform.rotation = Quaternion.Euler(newRotate);
    }
}
