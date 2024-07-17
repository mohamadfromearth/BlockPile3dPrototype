using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI progressText;
        [SerializeField] private Image progressImage;


        public void SetProgress(float value) => progressImage.fillAmount = value;

        public void SetProgressText(string text) => progressText.text = text;
    }
}