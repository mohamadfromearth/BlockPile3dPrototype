using Data;
using UI;

public class FortuneWheelCoinCollectionStrategy : IFortuneWheelCollectionStrategy
{
    private GameUI _gameUI;
    private CurrencyRepository _currencyRepository;
    private FortuneWheelRewardShowerUI _fortuneWheelRewardShowerUI;

    public FortuneWheelCoinCollectionStrategy(GameUI gameUI, CurrencyRepository currencyRepository,
        FortuneWheelRewardShowerUI fortuneWheelRewardShowerUI
    )
    {
        _gameUI = gameUI;
        _currencyRepository = currencyRepository;
        _fortuneWheelRewardShowerUI = fortuneWheelRewardShowerUI;
    }


    public void Claim(int count)
    {
        _gameUI.ShowCoinCollection();
    }
}