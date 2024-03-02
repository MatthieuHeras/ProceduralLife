using System.Collections.Generic;
using MHLib.Hexagon;
using UnityEngine;

public class HexTilePlacer : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    
    [SerializeField]
    private GameObject hexTile;

    private readonly Plane groundPlane = new(Vector3.up, Vector3.zero);

    private readonly float tileSize = 2f / Mathf.Sqrt(3f);

    private readonly Dictionary<Vector2Int, GameObject> placedTiles = new();
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2Int? cellPosition = HexagonHelper.ScreenToCell(Input.mousePosition, this.mainCamera, this.groundPlane, this.tileSize);
            if (cellPosition == null || this.placedTiles.ContainsKey(cellPosition.Value))
                return;
            
            Vector3 worldPosition = HexagonHelper.CellToWorld(cellPosition.Value, this.tileSize);
            
            GameObject newTile = Instantiate(this.hexTile, worldPosition, Quaternion.identity);
            this.placedTiles.Add(cellPosition.Value, newTile);
        }
        else if (Input.GetMouseButton(1))
        {
            Vector2Int? cellPosition = HexagonHelper.ScreenToCell(Input.mousePosition, this.mainCamera, this.groundPlane, this.tileSize);
            if (cellPosition == null || !this.placedTiles.ContainsKey(cellPosition.Value))
                return;
            
            Destroy(this.placedTiles[cellPosition.Value]);
            this.placedTiles.Remove(cellPosition.Value);
        }
    }
}
