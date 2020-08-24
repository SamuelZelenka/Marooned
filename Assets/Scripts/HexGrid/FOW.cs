using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FOW : MonoBehaviour
{
    public enum FOWMode { Hidden, Viewed, InView }

    [SerializeField] Tilemap hiddenTilemap = null;
    [SerializeField] Tilemap outOfViewTilemap = null;

    [SerializeField] TileBase hidden = null;
    [SerializeField] TileBase outOfView = null;

    List<HexCell> previousCellsInView = new List<HexCell>();

    private void Awake()
    {
        SessionSetup.OnPlayerCreated += Setup;
    }

    public void SetupStart(HexGrid grid)
    {
        foreach (var cell in grid.Cells)
        {
            SetFOWCell(cell);
        }
    }

    private void Setup(Player player)
    {
        player.Ship.OnNewCellsViewed += SetCellsInView;
    }

    private void SetCellsInView(List<HexCell> cellsInView)
    {
        foreach (HexCell cell in previousCellsInView)
        {
            cell.FOWMode = FOWMode.Viewed;
            SetFOWCell(cell);
        }
        previousCellsInView = cellsInView;
        foreach (HexCell cell in cellsInView)
        {
            cell.FOWMode = FOWMode.InView;
            SetFOWCell(cell);
        }
    }

    private void SetFOWCell(HexCell cell)
    {
        Vector3Int tilemapPosition = HexCoordinates.CoordinatesToTilemapCoordinates(cell.coordinates);
        switch (cell.FOWMode)
        {
            case FOWMode.Hidden:
                hiddenTilemap.SetTile(tilemapPosition, hidden);
                outOfViewTilemap.SetTile(tilemapPosition, outOfView);
                break;
            case FOWMode.Viewed:
                hiddenTilemap.SetTile(tilemapPosition, null);
                outOfViewTilemap.SetTile(tilemapPosition, outOfView);
                break;
            case FOWMode.InView:
                hiddenTilemap.SetTile(tilemapPosition, null);
                outOfViewTilemap.SetTile(tilemapPosition, null);
                break;
        }
    }
}