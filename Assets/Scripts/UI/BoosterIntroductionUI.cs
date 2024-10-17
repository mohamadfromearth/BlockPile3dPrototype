using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [System.Serializable]
    public class BoosterIntroductionUI
    {
        [SerializeField] private Image boosterImage;
        [SerializeField] private float moveDuration;
        [SerializeField] private Ease moveEase;
        [SerializeField] private Transform startTransform;

        private Action _movingCompleted;


        public void AddMovingCompletedListener(Action action)
        {
            _movingCompleted += action;
        }

        public void RemoveMovingCompletedListener(Action action)
        {
            _movingCompleted -= action;
        }

        public void StartMoving(Sprite boosterSprite, Vector3 targetPosition)
        {
            boosterImage.transform.position = startTransform.position;
            boosterImage.gameObject.SetActive(true);
            boosterImage.sprite = boosterSprite;
            boosterImage.transform.DOMove(targetPosition, moveDuration).onComplete += () =>
            {
                _movingCompleted?.Invoke();
                boosterImage.gameObject.SetActive(false);
            };
        }
    }
}