using System;
using Data;
using Event;
using UI;
using UnityEngine;
using Zenject;

public class FortuneWheelController : MonoBehaviour
{
    [SerializeField] private FortuneWheelRepository fortuneWheelRepository;
    [SerializeField] private FortuneWheelRewardShowerUI fortuneWheelRewardShowerUI;
    [SerializeField] private FortuneWheelCollectionStrategyHandler rewardCollectionStrategyHandler;
    [SerializeField] private FortuneWheelUI fortuneWheelUI;
    // It is not a good idea to access gameUI here but fuck it dont wanna make game manager hell!!
    [SerializeField] private GameUI gameUI;

    [Inject] private EventChannel _channel;


    private bool _isAdvertise = false;
    private FortuneWheelItemData _fortuneWheelItemData;

    private Action _fortuneWheelFlowCompleted;

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnSubscribeToEvents();
    }


    public void AddFortuneWheelFlowCompleteListener(Action action) => _fortuneWheelFlowCompleted += action;
    public void RemoveFortuneWheelFlowCompleteListener(Action action) => _fortuneWheelFlowCompleted -= action;


    public void OnWinUIHide()
    {
        if (fortuneWheelRepository.CanClaimWheel())
        {
            fortuneWheelUI.SetData(fortuneWheelRepository.GetData(), _isAdvertise);
            fortuneWheelUI.Show();
        }
    }


    private void SubscribeToEvents()
    {
        fortuneWheelUI.AddSpinClickListener(OnSpinFortuneWheelClick);
        fortuneWheelUI.AddRotationCompletionListener(FortuneWheelSpinningCompleted);
        fortuneWheelRewardShowerUI.AddClaimButtonClickListener(OnFortuneRewardClaim);
        fortuneWheelRewardShowerUI.AddClaimButtonClickListener(OnClaim);
        fortuneWheelUI.AddCloseClickListener(OnClose);

        _channel.Subscribe<FortuneWheelRewardCollect>(OnClaimComplete);
    }

    private void UnSubscribeToEvents()
    {
        fortuneWheelUI.RemoveSpinClickListener(OnSpinFortuneWheelClick);
        fortuneWheelUI.RemoveRotationCompletionListener(FortuneWheelSpinningCompleted);
        fortuneWheelRewardShowerUI.RemoveClaimButtonClickListener(OnClaim);
        fortuneWheelUI.RemoveCloseClickListener(OnClose);


        _channel.UnSubscribe<FortuneWheelRewardCollect>(OnClaimComplete);
    }


    #region Subscribers

    private void OnSpinFortuneWheelClick() => fortuneWheelUI.Rotate();

    private void FortuneWheelSpinningCompleted()
    {
        _fortuneWheelItemData = fortuneWheelRepository.GetFortuneWheelItemData(fortuneWheelUI.GetRotation());
        fortuneWheelUI.Hide();
        fortuneWheelRewardShowerUI.Show(_fortuneWheelItemData.sprite, _fortuneWheelItemData.count.ToString());
        gameUI.Hide();
    }


    private void OnClaim()
    {
        gameUI.Show();
        fortuneWheelRewardShowerUI.Hide();
        rewardCollectionStrategyHandler.Claim(_fortuneWheelItemData);

        if (_isAdvertise == false)
        {
            _isAdvertise = true;
        }
        else
        {
            _isAdvertise = false;
        }
    }


    private void OnClaimComplete()
    {
        if (_isAdvertise)
        {
            fortuneWheelUI.SetData(fortuneWheelRepository.GetData(), _isAdvertise);
            fortuneWheelUI.Show();
        }
        else
        {
            _fortuneWheelFlowCompleted();
        }
    }


    private void OnClose()
    {
        _fortuneWheelFlowCompleted();
        fortuneWheelUI.Hide();
    }

    #endregion


    private void OnFortuneRewardClaim() => rewardCollectionStrategyHandler.Claim(_fortuneWheelItemData);
}