using Data;
using Event;
using UI;
using Utils;

public class FortuneWheelCoinCollectionStrategy : IFortuneWheelCollectionStrategy
{
    //private readonly GameUI _gameUI;
    private readonly CurveMover _coinCollectionCurveMover;
    private readonly CurrencyRepository _currencyRepository;
    private readonly FortuneWheelRewardShowerUI _fortuneWheelRewardShowerUI;
    private readonly EventChannel _channel;

    private bool _isClaiming = false;

    public FortuneWheelCoinCollectionStrategy(CurrencyRepository currencyRepository,
        FortuneWheelRewardShowerUI fortuneWheelRewardShowerUI, EventChannel channel,
        CurveMover coinCollectionCurveMover
    )
    {
        _currencyRepository = currencyRepository;
        _fortuneWheelRewardShowerUI = fortuneWheelRewardShowerUI;
        //_gameUI.AddCoinCollectionAnimationCompleteListener(OnCoinCollectionAnimationCompleted);
        _coinCollectionCurveMover = coinCollectionCurveMover;
        _coinCollectionCurveMover?.AddLastMovingCompleteAnimationListener(OnCoinCollectionAnimationCompleted);
        _channel = channel;
    }


    private void OnCoinCollectionAnimationCompleted()
    {
        if (_isClaiming)
        {
            _channel.Rise<FortuneWheelRewardCollect>(new FortuneWheelRewardCollect());
            _isClaiming = false;
        }
    }


    public void Claim(int count)
    {
        _currencyRepository.AddCoin(count);

        if (_coinCollectionCurveMover.IsEmpty)
        {
            _channel.Rise<FortuneWheelRewardCollect>(new FortuneWheelRewardCollect());
            return;
        }

        _isClaiming = true;
        _coinCollectionCurveMover.CalculateWayPoints();
        _coinCollectionCurveMover.Move();
        //_gameUI.ShowCoinCollection();
    }
}