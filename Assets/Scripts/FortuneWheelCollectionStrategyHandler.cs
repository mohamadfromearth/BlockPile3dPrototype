using System.Collections.Generic;
using Data;
using UI;
using UnityEngine;

public class FortuneWheelCollectionStrategyHandler : MonoBehaviour
{
    private IFortuneWheelCollectionStrategy _coinCollectionStrategy;
    private IFortuneWheelCollectionStrategy _hammerCollectionStrategy;
    private IFortuneWheelCollectionStrategy _swapCollectionStrategy;
    private IFortuneWheelCollectionStrategy _refreshCollectionStrategy;

    [SerializeField] private CurrencyRepository currencyRepository;
    [SerializeField] private AbilityRepository abilityRepository;
    [SerializeField] private GameUI gameUI;

    [SerializeField] private Transform hammerTarget;
    [SerializeField] private Transform hammer;
    [SerializeField] private Transform swapTarget;
    [SerializeField] private Transform swapImage;
    [SerializeField] private Transform refresh;
    [SerializeField] private Transform refreshTarget;
    [SerializeField] private FortuneWheelRewardShowerUI fortuneWheelRewardShowerUI;

    [SerializeField] private float claimMovingDuration = 0.7f;


    private Dictionary<FortuneWheelItemType, IFortuneWheelCollectionStrategy> _fortuneWheelCollectionStrategies;


    private void Start()
    {
        _coinCollectionStrategy =
            new FortuneWheelCoinCollectionStrategy(gameUI, currencyRepository, fortuneWheelRewardShowerUI);
        _hammerCollectionStrategy =
            new FortuneWheelCollectionStrategy(hammer, hammerTarget, abilityRepository, AbilityType.Punch,
                claimMovingDuration, fortuneWheelRewardShowerUI);
        _swapCollectionStrategy =
            new FortuneWheelCollectionStrategy(swapImage, swapTarget, abilityRepository, AbilityType.Swap,
                claimMovingDuration, fortuneWheelRewardShowerUI);
        _refreshCollectionStrategy = new FortuneWheelCollectionStrategy(
            refresh, refreshTarget, abilityRepository, AbilityType.Refresh,
            claimMovingDuration,
            fortuneWheelRewardShowerUI
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