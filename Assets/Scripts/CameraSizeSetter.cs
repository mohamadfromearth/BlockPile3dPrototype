using UnityEngine;
using Zenject;

public class CameraSizeSetter
{
    private readonly Camera _camera;


    [Inject] private Board _board;

    public CameraSizeSetter(Camera camera)
    {
        _camera = camera;
    }


    public void RefreshSize()
    {
        if (Screen.width > Screen.height) return;

        var leftPosition = _board.CellToWorld(new Vector3Int(0, 0, _board.Height));
        var rightPosition = _board.CellToWorld(new Vector3Int(_board.Width, 0, 0));

        var topPosition = _board.CellToWorld(new Vector3Int(_board.Width, 0, 0));

        float screenAspect = (float)Screen.width / (float)Screen.height;

        float cameraWidth = Vector3.Distance(leftPosition, rightPosition);

        _camera.orthographicSize = cameraWidth / (2 * screenAspect);
    }
}