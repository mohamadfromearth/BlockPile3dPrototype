using UnityEngine;

namespace Objects.Cell
{
    public class CellPlacer : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;

        [SerializeField] private Grid grid;

        [SerializeField] private int width = 10;
        [SerializeField] private int height = 10;


        private void Start()
        {
            GenerateGrid();
        }


        public void GenerateGrid()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var gridPos = new Vector3Int(x, 0, y);
                    Instantiate(cellPrefab, grid.CellToWorld(gridPos), Quaternion.identity,grid.gameObject.transform);
                }
            }
        }
    }
}