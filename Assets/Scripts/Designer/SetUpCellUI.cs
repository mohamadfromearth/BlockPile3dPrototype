using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Designer
{
    public class SetUpCellUI : MonoBehaviour
    {
        [SerializeField] private Button removeCellButton;
        [SerializeField] private Button addColorButton;
        [SerializeField] private Button removeColorButton;

        [SerializeField] private GameObject panel;


        public void AddRemoveCellClickListener(UnityAction action) => removeCellButton.onClick.AddListener(action);

        public void RemoveRemoveCellClickListener(UnityAction action) =>
            removeCellButton.onClick.RemoveListener(action);


        public void AddAddColorClickListener(UnityAction action) => addColorButton.onClick.AddListener(action);


        public void RemoveAddColorClickListener(UnityAction action) => addColorButton.onClick.RemoveListener(action);


        public void AddRemoveColorClickListener(UnityAction action) => removeColorButton.onClick.AddListener(action);

        public void RemoveRemoveColorClickListener(UnityAction action) =>
            removeColorButton.onClick.RemoveListener(action);


        public void Show() => panel.SetActive(true);


        public void Hide() => panel.SetActive(false);
    }
}