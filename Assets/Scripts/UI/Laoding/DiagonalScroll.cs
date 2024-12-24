using UnityEngine;
using UnityEngine.UI;

public class DiagonalScroll : MonoBehaviour
{
    public float scrollSpeedX = 0.1f; // Speed in X direction
    public float scrollSpeedY = 0.1f; // Speed in Y direction
    private RawImage rawImage;
    private Vector2 offset;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        
        if (rawImage == null)
        {
            Debug.LogError("DiagonalScroll script must be attached to a GameObject with a RawImage component.");
        }
    }

    void Update()
    {
        if (rawImage != null)
        {
            // Calculate the new offset
            offset += new Vector2(scrollSpeedX, scrollSpeedY) * Time.deltaTime;
            // Apply the offset to the texture's UVs
            rawImage.uvRect = new Rect(offset.x, offset.y, 1, 1);
        }
    }
}