using Event;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class Input : MonoBehaviour
    {
        private PlayerControls _playerControls;

        [SerializeField] private UnityEvent<Vector2> PointerMoveEvent;
        [SerializeField] private UnityEvent PointerUpEvent;

        [Inject] private EventChannel _channel;

        private void Awake()
        {
            _playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            _playerControls.Enable();
            _playerControls.Game.PointerUp.performed += OnPointerUp;
        }

        private void OnDisable()
        {
            _playerControls.Disable();
            _playerControls.Game.PointerUp.performed -= OnPointerUp;
        }

        private void Update()
        {
            if (_playerControls.Game.PointerDown.IsInProgress())
            {
                var position = _playerControls.Game.PointerMove.ReadValue<Vector2>();
                PointerMoveEvent?.Invoke(position);
            }
        }

        private void OnPointerUp(InputAction.CallbackContext context)
        {
            PointerUpEvent?.Invoke();
            _channel.Rise<PointerUp>(new PointerUp());
        } 
    }
}