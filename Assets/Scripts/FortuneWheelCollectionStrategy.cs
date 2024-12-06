using Data;
using DG.Tweening;
using Event;
using UI;
using UnityEngine;

public class FortuneWheelCollectionStrategy : IFortuneWheelCollectionStrategy
{
    private readonly Transform _claimedObject;
    private readonly Transform _target;
    private readonly AbilityRepository _abilityRepository;
    private readonly AbilityType _abilityType;
    private readonly float _duration;
    private readonly FortuneWheelRewardShowerUI _fortuneWheelRewardShowerUI;
    private readonly EventChannel _channel;

    private readonly Vector3 _startPosition;

    public FortuneWheelCollectionStrategy(Transform claimedObject, Transform target,
        AbilityRepository abilityRepository, AbilityType abilityType, float duration,
        FortuneWheelRewardShowerUI fortuneWheelRewardShowerUI,
        EventChannel channel
    )
    {
        _claimedObject = claimedObject;
        _target = target;
        _abilityRepository = abilityRepository;
        _abilityType = abilityType;
        _duration = duration;
        if (_claimedObject != null) _startPosition = claimedObject.position;
        _fortuneWheelRewardShowerUI = fortuneWheelRewardShowerUI;
        _channel = channel;
    }

    public void Claim(int count)
    {
        if (_claimedObject == null)
        {
            _channel.Rise<FortuneWheelRewardCollect>(new FortuneWheelRewardCollect());
            _abilityRepository.AddAbility(_abilityType, count);
            return;
        }

        _claimedObject.position = _startPosition;
        _claimedObject.gameObject.SetActive(true);
        _abilityRepository.AddAbility(_abilityType, count);
        _claimedObject.DOMove(_target.position, _duration).onComplete = OnMovingCompleted;
    }


    private void OnMovingCompleted()
    {
        _claimedObject.gameObject.SetActive(false);
        _channel.Rise<FortuneWheelRewardCollect>(new FortuneWheelRewardCollect());
    }
}