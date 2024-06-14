using Scrips.Objects.CellContainerHolder;
using UnityEngine;
using Zenject;

namespace Scrips
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Grid _grid;

        [SerializeField] private int width, height;

        [Inject] private ICellContainerHolderFactory _cellContainerHolderFactory;
    }
}