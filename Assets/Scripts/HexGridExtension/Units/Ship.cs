﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : HexUnit
{
    public delegate void ShipHandler();
    public event ShipHandler OnShipBoarded;

    [SerializeField] ShipViewer shipViewer = null;
    public ShipViewer ShipViewer { get => shipViewer; }

    ShipData shipData = new ShipData();
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
    int currentVisionRange = 3;
    public int CurrentVisionRange
    {
        get => currentVisionRange;
        set
        {
            currentVisionRange = value;
        }
    }
    public readonly int defaultVisionRange = 3;
    #endregion

    #region Resources
    public bool IsOverStorageLimit()
    {
        int tonnage = 0;
        tonnage += shipData.Wool.value;
        tonnage += shipData.Tobacco.value;
        tonnage += shipData.Coffee.value;
        tonnage += shipData.Silk.value;
        tonnage += shipData.Ores.value;
        return tonnage > shipData.maxTonnage;
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
            if (!cell.HasHarbor)
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
                remainingMovementPoints /= 2;
            }
        }
        else
        {
            remainingMovementPoints = defaultMovementPoints;
        }
    }

    public override IEnumerator PerformAutomaticTurn()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Enables actions dependent on where the ship stops
    /// </summary>
    /// <param name="shipMoved"></param>
    protected override void CheckInteractableCells()
    {
        //Vision range
        List<HexCell> visionCells = CellFinder.GetCellsWithinRange(Location, currentVisionRange);
        List<Ship> ships = new List<Ship>();
        foreach (var item in visionCells)
        {
            if (item.Unit is Ship)
            {
                ships.Add(item.Unit as Ship);
            }
        }
        ShipsWithinVisionRange = ships;
        ships = new List<Ship>();

        //Cannon range
        List<HexCell> cannonCells = CellFinder.GetCellsWithinRange(Location, cannonRange, (c) => c.Unit != null);
        foreach (var item in cannonCells)
        {
            if (item.Unit is Ship)
            {
                ships.Add(item.Unit as Ship);
            }
        }
        ShipsWithinCannonRange = ships;
        ships = new List<Ship>();

        //Boarding range
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

    public void PlayerBoard()
    {
        Board(HexGridController.player.Ship);
    }
    public void Board(Ship ship)
    {
        OnShipBoarded?.Invoke();
    }
}