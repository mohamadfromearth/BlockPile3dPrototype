using System.Collections.Generic;
using Data;
using Event;
using UI;
using UnityEngine;
using Utils;
using Zenject;

public class FortuneWheelCollectionStrategyHandler : MonoBehaviour
{
    private IFortuneWheelCollectionStrategy _coinCollectionStrategy;
    private IFortuneWheelCollectionStrategy _hammerCollectionStrategy;
    private IFortuneWheelCollectionStrategy _swapCollectionStrategy;
    private IFortuneWheelCollectionStrategy _refreshCollectionStrategy;

    [SerializeField] private CurrencyRepository currencyRepository;

    [SerializeField] private AbilityRepository abilityRepository;

    //[SerializeField] private GameUI gameUI;
    [SerializeField] private CurveMover coinsCollectionCurveMover;


    [SerializeField] private Transform hammerTarget;
    [SerializeField] private Transform hammer;
    [SerializeField] private Transform swapTarget;
    [SerializeField] private Transform swapImage;
    [SerializeField] private Transform refresh;
    [SerializeField] private Transform refreshTarget;
    [SerializeField] private FortuneWheelRewardShowerUI fortuneWheelRewardShowerUI;

    [Inject] private EventChannel _channel;

    [SerializeField] private float claimMovingDuration = 0.7f;


    private Dictionary<FortuneWheelItemType, IFortuneWheelCollectionStrategy> _fortuneWheelCollectionStrategies;


    private void Start()
    {
        _coinCollectionStrategy =
            new FortuneWheelCoinCollectionStrategy(currencyRepository, fortuneWheelRewardShowerUI, _channel,
                coinsCollectionCurveMover);
        _hammerCollectionStrategy =
            new FortuneWheelCollectionStrategy(hammer, hammerTarget, abilityRepository, AbilityType.Punch,
                claimMovingDuration, fortuneWheelRewardShowerUI, _channel);
        _swapCollectionStrategy =
            new FortuneWheelCollectionStrategy(swapImage, swapTarget, abilityRepository, AbilityType.Swap,
                claimMovingDuration, fortuneWheelRewardShowerUI, _channel);
        _refreshCollectionStrategy = new FortuneWheelCollectionStrategy(
            refresh, refreshTarget, abilityRepository, AbilityType.Refresh,
            claimMovingDuration,
            fortuneWheelRewardShowerUI,
            _channel
        );


        _fortuneWheelCollectionStrategies = new()
        {
            { FortuneWheelItemType.Coin, _coinCollectionStrategy },
            { FortuneWheelItemType.Refresh, _refreshCollectionStrategy },
            { FortuneWheelItemType.Swap, _swapCollectionStrategy },
            { FortuneWheelItemType.Punch, _hammerCollectionStrategy },
        };
    }


    public void Claim(FortuneWheelItemData data) => _fortuneWheelCollectionStrategies[data.type].Claim(data.count);
}