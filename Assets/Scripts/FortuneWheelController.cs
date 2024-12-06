using System;
using Data;
using Event;
using Event.FortuneWheel;
using UI;
using UnityEngine;
using Zenject;

public class FortuneWheelController : MonoBehaviour
{
    [SerializeField] private FortuneWheelRepository fortuneWheelRepository;
    [SerializeField] private FortuneWheelRewardShowerUI fortuneWheelRewardShowerUI;
    [SerializeField] private FortuneWheelCollectionStrategyHandler rewardCollectionStrategyHandler;

    [SerializeField] private FortuneWheelUI fortuneWheelUI;

    [SerializeField] private bool endClaimingOnClose = true;

    // It is not a good idea to access the gameUI here but fuck it dont wanna make the game manager hell!!
    //[SerializeField] private GameUI gameUI;

    [Inject] private EventChannel _channel;


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


    public void CheckFortuneWheel()
    {
        if (fortuneWheelRepository.CanClaimWheel)
        {
            fortuneWheelUI.SetData(fortuneWheelRepository.GetData(), fortuneWheelRepository.IsAdvertise);
            fortuneWheelUI.Show();
        }
    }

    public void Show()
    {
        fortuneWheelUI.SetData(fortuneWheelRepository.GetData(), fortuneWheelRepository.IsAdvertise);
        fortuneWheelUI.Show();
    }


    private void SubscribeToEvents()
    {
        fortuneWheelUI.AddSpinClickListener(OnSpinFortuneWheelClick);
        fortuneWheelUI.AddRotationCompletionListener(FortuneWheelSpinningCompleted);
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

    private void OnSpinFortuneWheelClick()
    {
        if (fortuneWheelRepository.CanClaimWheel)
        {
            fortuneWheelUI.Rotate();
        }
    }

    private void FortuneWheelSpinningCompleted()
    {
        _fortuneWheelItemData = fortuneWheelRepository.GetFortuneWheelItemData(fortuneWheelUI.GetRotation());
        fortuneWheelUI.Hide();
        fortuneWheelRewardShowerUI.Show(_fortuneWheelItemData.sprite, _fortuneWheelItemData.count.ToString());
        _channel.Rise<FortuneWheelSpinningComplete>(new FortuneWheelSpinningComplete());
        //gameUI.Hide();
    }


    private void OnClaim()
    {
        _channel.Rise<FortuneWheelClaim>(new FortuneWheelClaim());
        //gameUI.Show();
        fortuneWheelRewardShowerUI.Hide();

        if (fortuneWheelRepository.IsAdvertise == false)
        {
            Debug.Log("OnClaim::IsAdvertise:false");
            fortuneWheelRepository.IsAdvertise = true;
        }
        else
        {
            Debug.Log("OnClaim::IsAdvertise:true");
            fortuneWheelRepository.IsAdvertise = false;
        }

        rewardCollectionStrategyHandler.Claim(_fortuneWheelItemData);
    }


    private void OnClaimComplete()
    {
        if (fortuneWheelRepository.IsAdvertise)
        {
            Debug.Log("OnClaimCompleted::IsAdvertise:true");

            fortuneWheelUI.SetData(fortuneWheelRepository.GetData(), fortuneWheelRepository.IsAdvertise);
            fortuneWheelUI.Show();
        }
        else
        {
            Debug.Log("OnClaimCompleted::IsAdvertise:false");

            fortuneWheelRepository.CanClaimWheel = false;
            fortuneWheelRepository.IncreaseIndex();
            _fortuneWheelFlowCompleted?.Invoke();
        }
    }


    private void OnClose()
    {
        _fortuneWheelFlowCompleted?.Invoke();
        fortuneWheelUI.Hide();
        if (endClaimingOnClose)
        {
            fortuneWheelRepository.IncreaseIndex();
            fortuneWheelRepository.IsAdvertise = false;
            fortuneWheelRepository.CanClaimWheel = false;
        }
    }

    #endregion
}