using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Designer
{
    public class SetUpCellUI : MonoBehaviour
    {
        [SerializeField] private Button removeCellButton;
        [SerializeField] private Button addCellButton;
        [SerializeField] private Button addColorButton;
        [SerializeField] private Button removeColorButton;
        [SerializeField] private Button addAdvertiseButton;
        [SerializeField] private Button removeAdvertiseButton;
        [SerializeField] private Button addLockButton;
        [SerializeField] private Button removeLockButton;

        [SerializeField] private GameObject panel;


        public void AddRemoveCellClickListener(UnityAction action) => removeCellButton.onClick.AddListener(action);

        public void RemoveRemoveCellClickListener(UnityAction action) =>
            removeCellButton.onClick.RemoveListener(action);

        public void AddAddCellClickListener(UnityAction action) => addCellButton.onClick.AddListener(action);

        public void RemoveAddCellClickListener(UnityAction action) => addCellButton.onClick.RemoveListener(action);


        public void AddAddColorClickListener(UnityAction action) => addColorButton.onClick.AddListener(action);


        public void RemoveAddColorClickListener(UnityAction action) => addColorButton.onClick.RemoveListener(action);


        public void AddRemoveColorClickListener(UnityAction action) => removeColorButton.onClick.AddListener(action);

        public void RemoveRemoveColorClickListener(UnityAction action) =>
            removeColorButton.onClick.RemoveListener(action);

        public void AddAddAdvertiseClickListener(UnityAction action) => addAdvertiseButton.onClick.AddListener(action);

        public void RemoveAddAdvertiseClickListener(UnityAction action) =>
            addAdvertiseButton.onClick.RemoveListener(action);


        public void AddRemoveAdvertiseClickListener(UnityAction action) =>
            removeAdvertiseButton.onClick.AddListener(action);


        public void RemoveRemoveAdvertiseClickListener(UnityAction action) =>
            removeAdvertiseButton.onClick.RemoveListener(action);


        public void AddAddLockClickListener(UnityAction action) => addLockButton.onClick.AddListener(action);

        public void RemoveAddLockClickListener(UnityAction action) => addLockButton.onClick.RemoveListener(action);


        public void AddRemoveLockClickListener(UnityAction action) => removeLockButton.onClick.AddListener(action);


        public void RemoveRemoveLockClickListener(UnityAction action) =>
            removeLockButton.onClick.RemoveListener(action);


        public void Show() => panel.SetActive(true);


        public void Hide() => panel.SetActive(false);
    }
}