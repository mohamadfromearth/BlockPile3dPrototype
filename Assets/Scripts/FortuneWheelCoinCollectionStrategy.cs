using Data;
using Event;
using UI;

public class FortuneWheelCoinCollectionStrategy : IFortuneWheelCollectionStrategy
{
    private readonly GameUI _gameUI;
    private readonly CurrencyRepository _currencyRepository;
    private readonly FortuneWheelRewardShowerUI _fortuneWheelRewardShowerUI;
    private readonly EventChannel _channel;

    private bool _isClaiming = false;

    public FortuneWheelCoinCollectionStrategy(GameUI gameUI, CurrencyRepository currencyRepository,
        FortuneWheelRewardShowerUI fortuneWheelRewardShowerUI, EventChannel channel
    )
    {
        _gameUI = gameUI;
        _currencyRepository = currencyRepository;
        _fortuneWheelRewardShowerUI = fortuneWheelRewardShowerUI;
        _gameUI.AddCoinCollectionAnimationCompleteListener(OnCoinCollectionAnimationCompleted);
        _channel = channel;
    }


    private void OnCoinCollectionAnimationCompleted()
    {
        if (_isClaiming)
        {
            _channel.Rise<FortuneWheelRewardCollect>( new FortuneWheelRewardCollect());
            _isClaiming = false;
        }
    }


    public void Claim(int count)
    {
        _isClaiming = true;
        _gameUI.ShowCoinCollection();
    }
}