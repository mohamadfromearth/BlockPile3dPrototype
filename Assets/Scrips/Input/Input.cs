using UnityEngine;
using UnityEngine.Events;

public class Input : MonoBehaviour
{
    private PlayerControls _playerControls;

    [SerializeField] private UnityEvent<Vector2> PointerMoveEvent;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void Update()
    {
        if (_playerControls.Game.PointerDown.IsInProgress())
        {
            var position = _playerControls.Game.PointerMove.ReadValue<Vector2>();
            PointerMoveEvent?.Invoke(position);
        }
    }
}