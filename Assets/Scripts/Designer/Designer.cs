using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Designer
{
    public class Designer : MonoBehaviour
    {
        [Inject] private Board _board;


        private void Start()
        {
            //_board.SpawnCells(new List<Vector3Int>());
        }
    }
}