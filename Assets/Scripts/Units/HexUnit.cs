using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class HexUnit : MonoBehaviour
{
    public delegate void HexUnitUpdateHandler(HexUnit unit);
    public static event HexUnitUpdateHandler OnUnitMoved;

    [Header("Movement")]
    const float travelSpeed = 4f;
    public int remainingMovementPoints = 5, defaultMovementPoints = 5;
    public int oceanMovementCost = 1;
    public int landMovementCost = 1;
    List<HexCell> pathToTravel;

    //public int playerIndex;
    public bool playerControlled;
    public HexGrid myGrid;

    public int currentVisionRange, defaultVisionRange;

    public bool IsMoving
    {
        get => pathToTravel != null && pathToTravel.Count > 0;
    }

    HexCell location;
    public HexCell Location
    {
        get => location;
        set
        {
            if (location)
            {
                location.Unit = null;
            }
            location = value;
            value.Unit = this;
            transform.localPosition = value.Position;
            OnUnitMoved?.Invoke(this);
        }
    }

    HexDirection orientation;
    public HexDirection Orientation
    {
        get => orientation;
        set
        {
            orientation = value;
            //Change sprite
        }
    }

    void OnEnable()
    {
        if (location)
        {
            transform.localPosition = location.Position;
            pathToTravel = null;
        }
    }

    public virtual void ShowUnitActive(bool status)
    {
        Location.ShowHighlight(status, HexCell.HighlightType.ActiveCell);
    }


    public void ValidateLocation() => transform.localPosition = location.Position;

    public abstract bool CanMoveTo(HexCell cell);

    public abstract void StartNewTurn();

    public abstract IEnumerator PerformAutomaticTurn();

    public IEnumerator Travel(List<HexCell> path)
    {
        if (playerControlled)
        {
            Location.ShowHighlight(false, HexCell.HighlightType.ActiveCell);
        }
        pathToTravel = path;
        yield return StartCoroutine(TravelPath());
        if (playerControlled)
        {
            Location.ShowHighlight(true, HexCell.HighlightType.ActiveCell);
        }
    }

    IEnumerator TravelPath()
    {
        float zPos = transform.localPosition.z;
        HexCell latestCell = pathToTravel[0];

        Vector3 a, b, c = pathToTravel[0].Position;
        transform.localPosition = c;


        float t = Time.deltaTime * travelSpeed;
        for (int i = 1; i < pathToTravel.Count; i++)
        {
            a = c;
            b = pathToTravel[i - 1].Position;
            c = (b + pathToTravel[i].Position) * 0.5f;

            //Rotation
            latestCell = pathToTravel[i - 1];
            Orientation = HexDirectionExtension.GetDirectionTo(latestCell, pathToTravel[i]);

            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                //Move
                Vector3 newPos = Bezier.GetPoint(a, b, c, t);
                newPos.z = zPos;
                transform.localPosition = newPos;

                yield return null;
            }
            Location = pathToTravel[i - 1];
            t -= 1f;
        }

        //Last point
        a = c;
        b = pathToTravel[pathToTravel.Count - 1].Position;
        c = b;

        //Rotation
        latestCell = pathToTravel[pathToTravel.Count - 2];
        Orientation = HexDirectionExtension.GetDirectionTo(latestCell, pathToTravel[pathToTravel.Count - 1]);

        for (; t < 1f; t += Time.deltaTime * travelSpeed)
        {
            //Move
            Vector3 newPos = Bezier.GetPoint(a, b, c, t);
            newPos.z = zPos;
            transform.localPosition = newPos;

            yield return null;
        }
        Location = pathToTravel[pathToTravel.Count - 1];
        transform.localPosition = location.Position;
        pathToTravel = null; //Clear the list
    }

    public void Despawn()
    {
        location.Unit = null;
        Destroy(gameObject);
    }

    public void Die()
    {
        location.Unit = null;
        Destroy(gameObject);
    }
}
