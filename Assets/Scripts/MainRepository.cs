using System;
using UnityEngine;


[CreateAssetMenu(fileName = "MainRepository", menuName = "So/MainRepository")]
public class MainRepository : ScriptableObject
{
    private const string RefreshCountKey = "REFRESH_COUNT_KEY";
    private const string SwapCountKey = "SWAP_COUNT_KEY";
    private const string PunchCountKey = "PUNCH_COUNT_KEY";


    private int _refreshCount;
    private int _swapCount;
    private int _punchCount;

    public int RefreshCount => _refreshCount;
    public int SwapCount => _swapCount;
    public int PunchCount => _punchCount;

    private void OnEnable()
    {
        _refreshCount = PlayerPrefs.GetInt(RefreshCountKey, 0);
        _swapCount = PlayerPrefs.GetInt(SwapCountKey, 0);
        _punchCount = PlayerPrefs.GetInt(PunchCountKey, 0);
    }

    public void IncreaseRefreshCount()
    {
        _refreshCount++;
        PlayerPrefs.SetInt(RefreshCountKey, _refreshCount);
    }

    public void DecreaseRefreshCount()
    {
        _refreshCount--;
        PlayerPrefs.SetInt(RefreshCountKey, _refreshCount);
    }

    public void IncreaseSwapCount()
    {
        _swapCount++;
        PlayerPrefs.SetInt(SwapCountKey, _swapCount);
    }

    public void DecreaseSwapCount()
    {
        _swapCount--;
        PlayerPrefs.SetInt(SwapCountKey, _swapCount);
    }

    public void IncreasePunchCount()
    {
        _punchCount++;
        PlayerPrefs.SetInt(PunchCountKey, _punchCount);
    }

    public void DecreasePunchCount()
    {
        _punchCount--;
        PlayerPrefs.SetInt(PunchCountKey, _punchCount);
    }
}