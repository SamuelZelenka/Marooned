using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantRoute
{
    public HexCell[] RouteStops
    {
        get;
        private set;
    }
    public MerchantRoute(HexCell[] harborCells)
    {
        RouteStops = harborCells;
    }
}
