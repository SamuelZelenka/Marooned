using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class HexUnit : MonoBehaviour
{
    public delegate void HexUnitUpdateHandler(HexUnit unit);
    public static event HexUnitUpdateHandler OnUnitBeganMove;
    public static event HexUnitUpdateHandler OnUnitMoved;

    public LogMessage logMessage;

    [Header("Movement")]
    const float travelSpeed = 4f;
    public int remainingMovementPoints = 0, defaultMovementPoints = 5;
    public int oceanMovementCost = 1;
    public int landMovementCost = 1;
    List<HexCell> pathToTravel;
    List<HexCell> reachableCells = new List<HexCell>(); public List<HexCell> ReachableCells { get => reachableCells; }

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
            if (myGrid)
            {
                ShowReachableCells(false);
                CalculateReachableCells();
                ShowReachableCells(true);
            }
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

    public virtual void MakeUnitActive()
    {
        logMessage = new LogMessage();
        ShowUnitActive(true);
    }

    public void MakeUnitInactive()
    {
        ShowUnitActive(false);
    }

    public virtual void ShowUnitActive(bool status)
    {
        Location.ShowHighlight(status, HexCell.HighlightType.ActiveCell);

        ShowReachableCells(false);

        if (status)
        {
            CalculateReachableCells();
            ShowReachableCells(true);
        }
    }

    public void ValidateLocation() => transform.localPosition = location.Position;

    public abstract bool CanEnter(HexCell cell);

    private void CalculateReachableCells() => reachableCells = Pathfinding.GetAllReachableCells(Location, this);

    private void ShowReachableCells(bool status)
    {
        if (!playerControlled)
        {
            return;
        }
        foreach (var item in reachableCells)
        {
            item.ShowHighlight(status, HexCell.HighlightType.ValidMoveInteraction);
        }
    }



    public abstract IEnumerator PerformAutomaticTurn();

    public IEnumerator Travel(List<HexCell> path)
    {
        if (!IsMoving && path.Count > 1)
        {
            Location.ShowHighlight(false, HexCell.HighlightType.ActiveCell);
            pathToTravel = path;
            remainingMovementPoints -= path.Count - 1; //TODO CHANGE TO PATH COST
            if (path.Count - 1 > 1)
            {
                logMessage.AddLine($"Moved {path.Count - 1} steps.");
            }
            else
            {
                logMessage.AddLine($"Moved 1 step.");
            }
            yield return StartCoroutine(TravelPath());
            Location.ShowHighlight(true, HexCell.HighlightType.ActiveCell);
        }
    }

    IEnumerator TravelPath()
    {
        OnUnitBeganMove?.Invoke(this); //TODO: Better position for this delegate call recommended
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
            Orientation = HexDirectionExtension.GetDirectionToNeighbor(latestCell, pathToTravel[i]);

            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                //Move
                Vector3 newPos = Bezier.GetPoint(a, b, c, t);
                newPos.z = zPos;
                transform.localPosition = newPos;

                yield return null;
            }
            //Location = pathToTravel[i - 1]; //Removed to ignore OnHexUnit moved while only being on a path (need solution for Fog of War updates)
            t -= 1f;
        }

        //Last point
        a = c;
        b = pathToTravel[pathToTravel.Count - 1].Position;
        c = b;

        //Rotation
        latestCell = pathToTravel[pathToTravel.Count - 2];
        Orientation = HexDirectionExtension.GetDirectionToNeighbor(latestCell, pathToTravel[pathToTravel.Count - 1]);

        for (; t < 1f; t += Time.deltaTime * travelSpeed)
        {
            //Move
            Vector3 newPos = Bezier.GetPoint(a, b, c, t);
            newPos.z = zPos;
            transform.localPosition = newPos;

            yield return null;
        }
        Location = pathToTravel[pathToTravel.Count - 1]; //Set new location
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
public class LogMessage
{
    string message = "";
    Sprite usedAbilitySprite;
    public string Message { get { return message; } }
    public Sprite UsedAbilitySprite { get { return usedAbilitySprite; } }

    public void AddLine(string newLine)
    {
        if (message == "")
        {
            message += $"{newLine}";
            return;
        }
        message += $"\n{newLine}";
    }
    public void SetAbilitySprite(Sprite sprite)
    {
        usedAbilitySprite = sprite;
    }
}