using System;
using UnityEngine;
using Zenject;

[Serializable]
public class CameraSizeSetterData
{
    public Camera camera;
    public RectTransform top;
    public RectTransform down;
    public LayerMask mask;
}


public class CameraSizeSetter
{
    private CameraSizeSetterData _data;

    [Inject] private Board _board;


    public CameraSizeSetter(CameraSizeSetterData data)
    {
        _data = data;
    }


    public void RefreshSize()
    {
        Vector3 cameraPos = _data.camera.transform.position;
        if (Screen.height > Screen.width)
        {
            float screenAspect = (float)Screen.width / (float)Screen.height;

            var leftPosition = _board.CellToWorld(new Vector3Int(0, 0, _board.Height + 1));
            var rightPosition = _board.CellToWorld(new Vector3Int(_board.Width - 1, 0, 0));

            float cameraWidth = Vector3.Distance(leftPosition, rightPosition);
            var size = cameraWidth / (2 * screenAspect);
            _data.camera.orthographicSize = cameraWidth / (2 * screenAspect);
            cameraPos.y = size;
        }
        else
        {
            var topPosition = _board.CellToWorld(new Vector3Int(_board.Width, 0, _board.Width));
            var downPosition = _board.CellToWorld(new Vector3Int(-2, 0, -2));
            float cameraHeight = Vector3.Distance(topPosition, downPosition);
            var size = cameraHeight / 2;
            _data.camera.orthographicSize = size;
            cameraPos.y = size;
        }

        _data.camera.transform.position = cameraPos;
    }
}