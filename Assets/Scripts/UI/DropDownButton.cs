using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DropDownButton : MonoBehaviour
    {
        [SerializeField] private Button mainButton;
        [SerializeField] private Image mainButtonImage;
        [SerializeField] private Button[] buttons;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float padding = 2f;
        [SerializeField] private bool isCameraCanvas;

        private Quaternion _initialRotation;
        private bool _isDropped = false;

        private float _size;


        private void Start()
        {
            if (isCameraCanvas)
            {
                _initialRotation = mainButton.transform.rotation;
                _size = Screen.width > Screen.height ? 5 : 3;
            }
            else
            {
                _size = mainButtonImage.rectTransform.rect.height;
            }
        }


        private void OnEnable()
        {
            mainButton.onClick.AddListener(OnMainButtonClick);
        }

        private void OnDisable()
        {
            mainButton.onClick.RemoveListener(OnMainButtonClick);
        }


        private void OnMainButtonClick()
        {
            if (_isDropped)
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].transform.DOMove(mainButton.transform.position, duration);
                }

                _isDropped = false;
            }
            else
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    var position = mainButton.transform.position;
                    position.y -= (_size + padding) * (i + 1);
                    buttons[i].transform.DOMove(position, duration);
                }

                _isDropped = true;
            }

            mainButtonImage.transform.DORotate(GetRotation().eulerAngles, duration);
        }


        private Quaternion GetRotation()
        {
            if (isCameraCanvas)
            {
                var rot = _isDropped ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
                return _initialRotation * rot;
            }

            return _isDropped ? Quaternion.Euler(0, 0, 180) : Quaternion.identity;
        }
    }
}