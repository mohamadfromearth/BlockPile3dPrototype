using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class FortuneWheelUI
{
    [SerializeField] private Button spinButton;
    [SerializeField] private Image[] itemsImages;
    [SerializeField] private TextMeshProUGUI[] itemsTexts;
    [SerializeField] private Transform transform;

    [SerializeField] private AnimationCurve rotationAnimationCurve;

    [SerializeField] private float rotationDuration = 2f;


    private float degree;

    public float Degree => degree;

    private Action _rotationCompleted;


    public void SetData(Sprite[] sprites, int[] counts)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            var sprite = sprites[i];
            var countText = counts[i].ToString();

            itemsImages[i].sprite = sprite;
            itemsTexts[i].text = countText;
        }
    }


    public void SetItemsSprites(Sprite[] sprites)
    {
        for (int i = 0; i < itemsImages.Length; i++)
        {
            itemsImages[i].sprite = sprites[i];
        }
    }

    public void Rotate()
    {
        var rotationCount = Random.Range(6, 12);
        var rotationValue = rotationCount * 360f;
        transform.DORotate(new Vector3(0, 0, rotationValue), rotationDuration)
            .SetEase(rotationAnimationCurve)
            .onComplete = () => { _rotationCompleted?.Invoke(); };
    }


    public void AddSpinClickListener(UnityAction action) => spinButton.onClick.AddListener(action);
    public void RemoveSpinClickListener(UnityAction action) => spinButton.onClick.RemoveListener(action);

    public void AddRotationCompletionListener(Action action) => _rotationCompleted += action;

    public void RemoveRotationCompletionListener(Action action) => _rotationCompleted -= action;
}