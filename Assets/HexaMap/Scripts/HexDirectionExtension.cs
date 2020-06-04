using UnityEngine;

public static class HexDirectionExtension
{
    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }

    public static HexDirection Previous(this HexDirection direction)
    {
        return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
    }

    public static HexDirection Next(this HexDirection direction)
    {
        return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
    }

    public static HexDirection Previous2(this HexDirection direction)
    {
        direction -= 2;
        return direction >= HexDirection.NE ? direction : (direction + 6);
    }

    public static HexDirection Next2(this HexDirection direction)
    {
        direction += 2;
        return direction <= HexDirection.NW ? direction : (direction - 6);
    }

    public static HexDirection GetDirectionTo(HexCell fromCell, HexCell toCell)
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            if (fromCell.GetNeighbor(d) == toCell)
            {
                return d;
            }
        }
        Debug.Log("Could not find direction to cell");
        return HexDirection.NE;
    }

    public static HexDirection ReturnRandomDirection()
    {
        return (HexDirection) Random.Range((int)HexDirection.NE, (int)HexDirection.NW);
    }
}
