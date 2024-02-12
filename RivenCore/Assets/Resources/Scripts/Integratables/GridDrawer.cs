// This is waaaayyy too laggy to be useful :(
// ~Liz M.




using UnityEngine;

public class GridDrawer : MonoBehaviour
{
    //=-----------------=
    // Public Variables
    //=-----------------=
    public int gridWidth = 100; // Number of grid cells horizontally
    public int gridHeight = 100; // Number of grid cells vertically
    public float cellSize = 25f; // Size of each grid cell
    public Color gridColor = Color.white; // Color of the grid lines
    public float teleportRange = 10f; // Range from the grid center to trigger teleportation

    //=-----------------=
    // Private Variables
    //=-----------------=
    private SpriteRenderer spriteRenderer;
    private Camera viewCamera;
    private Vector3 lastCameraPosition;

    //=-----------------=
    // Mono Functions
    //=-----------------=
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        viewCamera = FindObjectOfType<Camera>();
        //lastCameraPosition = viewCamera.transform.position;
    }

    private void Update()
    {
        if (!viewCamera)
        {
            viewCamera = FindObjectOfType<Camera>();
            return;
        }
        if (IsCameraOutOfRange())
        {
            DrawGrid();
        }
    }

    //=-----------------=
    // Internal Functions
    //=-----------------=
    private void DrawGrid()
    {
        // Calculate the size of the grid
        float width = gridWidth * cellSize;
        float height = gridHeight * cellSize;

        // Calculate the position of the grid's center based on the camera's position,
        // rounding to the nearest half-cell
        Vector3 cameraCenter = viewCamera.transform.position;
        Vector3 gridCenter = new Vector3(Mathf.Round(cameraCenter.x / (cellSize / 8f)) * (cellSize / 8f),
                                         Mathf.Round(cameraCenter.y / (cellSize / 8f)) * (cellSize / 8f), 0f);

        // Calculate the position of the bottom-left corner of the grid
        Vector3 gridBottomLeft = gridCenter - new Vector3(width / 2f, height / 2f, 0f);

        // Create a new texture for the grid
        Texture2D texture = new Texture2D((int)width, (int)height);

        // Draw horizontal and vertical grid lines
        for (int y = 0; y < gridHeight; y++)
        {
            texture.SetPixel(0, (int)(y * cellSize), gridColor);
            texture.SetPixel((int)(width - 1), (int)(y * cellSize), gridColor);
        }

        for (int x = 0; x < gridWidth; x++)
        {
            texture.SetPixel((int)(x * cellSize), 0, gridColor);
            texture.SetPixel((int)(x * cellSize), (int)(height - 1), gridColor);
        }

        // Apply the changes to the texture
        texture.Apply();

        // Create a new sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.one * 0.5f);

        // Set the sprite and color of the sprite renderer
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = gridColor;

        // Set the position of the grid
        transform.position = gridBottomLeft + new Vector3(width / 2f, height / 2f, 0f);
    }

    private bool IsCameraOutOfRange()
    {
        // Check if the camera has moved beyond the teleport range
        if (Vector3.Distance(lastCameraPosition, viewCamera.transform.position) > teleportRange)
        {
            lastCameraPosition = viewCamera.transform.position;
            return true;
        }
        return false;
    }
}
