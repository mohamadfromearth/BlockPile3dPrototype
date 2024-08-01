using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Designer
{
    public class DesignerUI : MonoBehaviour
    {
        [SerializeField] private Button setUpBoardButton;


        public void AddSetUpBoardClickListener(UnityAction action) => setUpBoardButton.onClick.AddListener(action);

        public void RemoveSetUpBoardClickListener(UnityAction action) =>
            setUpBoardButton.onClick.RemoveListener(action);
    }
}