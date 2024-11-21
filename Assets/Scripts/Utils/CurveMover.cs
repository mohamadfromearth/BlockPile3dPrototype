using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    [Serializable]
    public class CurveMover
    {
        [SerializeField] private float startingDuration;
        [SerializeField] private float spawningInterval;
        [SerializeField] private Transform[] wayPointsTransforms;
        [SerializeField] private Transform[] objects;

        private TweenCallback _firstMovingComplete;
        private TweenCallback _lastMovingComplete;


        private Vector3[] _wayPoints;


        public void AddFirstMovingCompleteAnimationListener(TweenCallback callback) => _firstMovingComplete += callback;

        public void RemoveFirstMovingCompleteAnimationListener(TweenCallback callback) =>
            _firstMovingComplete -= callback;


        public void AddLastMovingCompleteAnimationListener(TweenCallback callback) => _lastMovingComplete += callback;

        public void RemoveLastMovingCompleteAnimationListener(TweenCallback callback) =>
            _lastMovingComplete -= callback;


        public void CalculateWayPoints()
        {
            _wayPoints = wayPointsTransforms.Select(transform => transform.position).ToArray();
        }

        public void Move()
        {
            var currentDuration = startingDuration;
            for (int i = 0; i < objects.Length; i++)
            {
                var movingObject = objects[i];
                movingObject.position = wayPointsTransforms[0].position;
                movingObject.gameObject.SetActive(true);

                var tween = movingObject.DOPath(_wayPoints, currentDuration, PathType.CatmullRom);

                if (i == 0) tween.onComplete += _firstMovingComplete;

                if (i == objects.Length - 1) tween.onComplete += _lastMovingComplete;

                var currentObject = movingObject;

                tween.onComplete += () => { currentObject.gameObject.SetActive(false); };

                currentDuration += spawningInterval;
            }
        }
    }
}