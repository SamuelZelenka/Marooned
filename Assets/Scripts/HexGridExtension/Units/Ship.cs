using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : HexUnit
{
    public delegate void ShipInteractionHandler(Player thisPlayer, Player otherPlayer);
    public event ShipInteractionHandler OnShipBoarded;
    public event ShipInteractionHandler OnShipInspected;

    [SerializeField] ShipViewer shipViewer = null;
    public ShipViewer ShipViewer { get => shipViewer; }

    #region Stats
    int hull = 25, maxHull = 25;
    public int Hull
    {
        set
        {
            hull = Mathf.Min(maxHull, value);
        }
        get => hull;
    }
    float cleanliness = 1;
    public float Cleanliness
    {
        get => cleanliness;
        set
        {
            cleanliness = Mathf.Min(value, 1);
        }
    }
    int cannonRange = 2;
    public int CannonRange
    {
        get => cannonRange;
        set
        {
            cannonRange = value;
        }
    }
    
    #endregion

    #region Resources
    public bool IsOverStorageLimit()
    {
        int tonnage = 0;
        tonnage += myPlayer.PlayerData.Resources.Wool.Value;
        tonnage += myPlayer.PlayerData.Resources.Tobacco.Value;
        tonnage += myPlayer.PlayerData.Resources.Coffee.Value;
        tonnage += myPlayer.PlayerData.Resources.Silk.Value;
        tonnage += myPlayer.PlayerData.Resources.Ores.Value;
        return tonnage > myPlayer.PlayerData.Resources.maxTonnage;
    }
    #endregion

    #region OtherShips
    List<Ship> shipsWithinVisionRange;
    public List<Ship> ShipsWithinVisionRange
    {
        get => shipsWithinVisionRange;
        set
        {
            if (shipsWithinVisionRange != null)
            {
                foreach (var item in shipsWithinVisionRange)
                {
                    if (item == this)
                    {
                        continue;
                    }
                    if (this.playerControlled)
                    {
                        item.ShipViewer.ShowViewPossible(false);
                    }
                }
            }
            shipsWithinVisionRange = value;
            if (shipsWithinVisionRange != null)
            {
                foreach (var item in shipsWithinVisionRange)
                {
                    if (item == this)
                    {
                        continue;
                    }
                    if (this.playerControlled)
                    {
                        item.ShipViewer.ShowViewPossible(true);
                    }
                }
            }
        }
    }
    List<Ship> shipsWithinCannonRange;
    public List<Ship> ShipsWithinCannonRange
    {
        get => shipsWithinCannonRange;
        set
        {
            if (shipsWithinCannonRange != null)
            {
                foreach (var item in shipsWithinCannonRange)
                {
                    if (item == this)
                    {
                        continue;
                    }
                    if (this.playerControlled)
                    {
                        item.ShipViewer.ShowShootingPossible(false);
                    }
                }
            }
            shipsWithinCannonRange = value;
            if (shipsWithinCannonRange != null)
            {
                foreach (var item in shipsWithinCannonRange)
                {
                    if (item == this)
                    {
                        continue;
                    }
                    if (this.playerControlled)
                    {
                        item.ShipViewer.ShowShootingPossible(true);
                    }
                }
            }
        }
    }
    List<Ship> shipsWithinBoardingRange;
    public List<Ship> ShipsWithinBoardingRange
    {
        get => shipsWithinBoardingRange;
        set
        {
            if (shipsWithinBoardingRange != null)
            {
                foreach (var item in shipsWithinBoardingRange)
                {
                    if (item == this)
                    {
                        continue;
                    }
                    if (this.playerControlled)
                    {
                        item.ShipViewer.ShowBoardingPossible(false);
                    }
                }
            }
            shipsWithinBoardingRange = value;
            if (shipsWithinBoardingRange != null)
            {
                foreach (var item in shipsWithinBoardingRange)
                {
                    if (item == this)
                    {
                        continue;
                    }
                    if (this.playerControlled)
                    {
                        item.ShipViewer.ShowBoardingPossible(true);
                    }
                }
            }
        }
    }
    
    #endregion

    public override bool CanEnter(HexCell cell)
    {
        if (cell.Unit)
        {
            return false;
        }
        if (cell.IsLand)
        {
            if (!cell.HasHarbor && !cell.HasStronghold)
            {
                return false;
            }
        }
        return true;
    }

    public override void MakeUnitActive()
    {
        base.MakeUnitActive();
        CheckInteractableCells();
        if (playerControlled)
        {
            if (IsOverStorageLimit())
            {
                remainingMovementPoints = Mathf.RoundToInt((float)remainingMovementPoints / 2);
            }
        }
        else
        {
            remainingMovementPoints = defaultMovementPoints;
        }
    }

    public override IEnumerator PerformAutomaticTurn(int visionRange)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Enables actions dependent on where the ship stops
    /// </summary>
    /// <param name="shipMoved"></param>
    protected override void CheckInteractableCells()
    {
        base.CheckInteractableCells();
        //Viewed units
        List<Ship> ships = new List<Ship>();
        foreach (var cell in VisionCells)
        {
            if (cell.Unit is Ship)
            {
                ships.Add(cell.Unit as Ship);
            }
        }
        ShipsWithinVisionRange = ships;

        //Cannon range
        ships = new List<Ship>();
        List<HexCell> cannonCells = CellFinder.GetCellsWithinRange(Location, cannonRange, (c) => c.Unit != null);
        foreach (var cell in cannonCells)
        {
            if (cell.Unit is Ship)
            {
                ships.Add(cell.Unit as Ship);
            }
        }
        ShipsWithinCannonRange = ships;

        //Boarding range
        ships = new List<Ship>();
        List<HexCell> boardingCells = CellFinder.GetAllAdjacentCells(Location, (c) => c.Unit != null);
        foreach (var item in boardingCells)
        {
            if (item.Unit is Ship)
            {
                ships.Add(item.Unit as Ship);
            }
        }
        ShipsWithinBoardingRange = ships;
    }

    public void InspectShip() => OnShipInspected?.Invoke(myPlayer, HexGridController.player);
    public void PlayerBoard() => Board(HexGridController.player);
    public void Board(Player boardedBy) => OnShipBoarded?.Invoke(myPlayer, boardedBy);
}