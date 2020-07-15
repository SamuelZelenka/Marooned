using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : HexUnit
{
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
        if (playerControlled)
        {

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
}
