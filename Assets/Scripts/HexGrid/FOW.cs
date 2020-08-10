using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FOW : MonoBehaviour
{
    public enum FOWMode { Hidden, Viewed, InView }

    [SerializeField] Tilemap fowTilemap = null;
    [SerializeField] TileBase hidden = null;
    [SerializeField] TileBase viewed = null;

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
                fowTilemap.SetTile(tilemapPosition, hidden);
                break;
            case FOWMode.Viewed:
                fowTilemap.SetTile(tilemapPosition, viewed);
                break;
            case FOWMode.InView:
                fowTilemap.SetTile(tilemapPosition, null);
                break;
        }
    }
}