using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class HexUnit : MonoBehaviour, ITrackable
{
    public delegate void HexUnitUpdateHandler();
    public delegate void HexUnitReferenceUpdateHandler(HexUnit unit);
    public static event HexUnitReferenceUpdateHandler OnAnyUnitBeganMove;
    public event HexUnitUpdateHandler OnUnitMoved;

    public delegate void VisionHandler(List<HexCell> viewedCells);
    public event VisionHandler OnNewCellsViewed;


    public LogMessage logMessage;

    [Header("Movement")]
    const float travelSpeed = 4f;
    public int remainingMovementPoints = 0, defaultMovementPoints = 10;
    public int oceanMovementCost = 5;
    public int landMovementCost = 5;
    List<HexCell> pathToTravel;

    public List<HexCell> ReachableCells { get; private set; } = new List<HexCell>();

    [Header("Visuals")]
    [SerializeField] SpriteRenderer unitRenderer = null;
    [SerializeField] Sprite[] unitSprites = new Sprite[3];

    public bool playerControlled;
    public HexGrid myGrid;
    public Player myPlayer;

    public void Setup(Player myPlayer)
    {
        this.myPlayer = myPlayer;
    }

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
            ShowUnit(InView);
            OnUnitMoved?.Invoke();
            CheckInteractableCells();
        }
    }

    List<HexCell> visionCells;
    protected List<HexCell> VisionCells
    {
        get => visionCells;
        set
        {
            visionCells = value;
            OnNewCellsViewed?.Invoke(visionCells);
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
            if (unitRenderer && unitSprites != null && unitSprites.Length == 3)
            {
                unitRenderer.flipX = value >= HexDirection.SW;
                int index = (int)value;
                if (value == HexDirection.SW)
                {
                    index = (int)HexDirection.SE;
                }
                else if (value == HexDirection.W)
                {
                    index = (int)HexDirection.E;
                }
                else if (value == HexDirection.NW)
                {
                    index = (int)HexDirection.NE;
                }
                unitRenderer.sprite = unitSprites[index];
            }
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

    public void ShowUnit(bool status)
    {
        if (unitRenderer)
            unitRenderer.enabled = status;
    }

    public void ValidateLocation() => transform.localPosition = location.Position;

    public abstract bool CanEnter(HexCell cell);

    private void CalculateReachableCells() => ReachableCells = Pathfinding.GetAllReachableCells(Location, this);

    private void ShowReachableCells(bool status)
    {
        if (!playerControlled)
        {
            return;
        }
        foreach (var item in ReachableCells)
        {
            item.ShowHighlight(status, HexCell.HighlightType.ValidMoveInteraction);
        }
    }

    public bool TrackMe() => InView;
    bool InView => Location.FOWMode == FOW.FOWMode.InView;
    public Transform MyTransform() => transform;

    public abstract IEnumerator PerformAutomaticTurn(int visionRange);

    public IEnumerator Travel(List<HexCell> path)
    {
        if (!IsMoving && path.Count > 1)
        {
            Location.ShowHighlight(false, HexCell.HighlightType.ActiveCell);
            pathToTravel = path;
            remainingMovementPoints -= path[path.Count - 1].MovementCost;
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
        OnAnyUnitBeganMove?.Invoke(this); //TODO: Better position for this delegate call recommended
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

            ShowUnit(Location.FOWMode == FOW.FOWMode.InView);

            //Rotation
            latestCell = pathToTravel[i - 1];
            Orientation = HexDirectionExtension.GetDirectionToNeighbor(latestCell, pathToTravel[i]);

            if (InView || pathToTravel[i].FOWMode == FOW.FOWMode.InView)
            {
                for (; t < 1f; t += Time.deltaTime * travelSpeed)
                {
                    //Move
                    Vector3 newPos = Bezier.GetPoint(a, b, c, t);
                    newPos.z = zPos;
                    transform.localPosition = newPos;

                    yield return null;
                }
                t -= 1f;
            }
            Location = pathToTravel[i];
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

    protected virtual void CheckInteractableCells()
    {
        //Vision range
        List<HexCell> hexCells = CellFinder.GetCellsWithinRange(Location, currentVisionRange);
        hexCells.Add(location);
        VisionCells = hexCells;
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