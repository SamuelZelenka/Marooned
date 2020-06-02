using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HexUnit : MonoBehaviour
{
    public static HexUnit unitPrefab;
    List<HexCell> pathToTravel;
    const float travelSpeed = 4f;
    const float rotationSpeed = 180f;

    public int movement = 5, maxMovement = 5;
    public bool canMoveOnOcean;
    public bool canMoveOnLand;
    public int oceanMovementCost = 1;
    public int landMovementCost = 1;

    public bool useCurvedMovement = true;

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

    public void ValidateLocation()
    {
        transform.localPosition = location.Position;
    }



    public bool CanMoveTo(HexCell cell)
    {
        if (cell.Unit)
        {
            return false;
        }
        if (cell.IsOcean)
        {
            if (!canMoveOnOcean)
            {
                return false;
            }
        }
        else
        {
            if (!canMoveOnLand)
            {
                return false;
            }
        }
        return true;
    }

    public void Travel(List<HexCell> path)
    {
        Location = path[path.Count - 1];
        pathToTravel = path;
        StartCoroutine(TravelPath());
    }

    IEnumerator TravelPath()
    {
        float zPos = transform.localPosition.z;
        HexCell latestCell = pathToTravel[0];

        if (useCurvedMovement)
        {
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
        }
        else
        {
            for (int i = 1; i < pathToTravel.Count; i++)
            {
                Vector3 a = pathToTravel[i - 1].Position;
                Vector3 b = pathToTravel[i].Position;

                //Rotation
                latestCell = pathToTravel[i - 1];
                Orientation = HexDirectionExtension.GetDirectionTo(latestCell, pathToTravel[i]);

                for (float t = 0f; t < 1f; t += Time.deltaTime * travelSpeed)
                {
                    //Move
                    Vector3 newPos = Vector3.Lerp(a, b, t);
                    newPos.z = zPos;
                    transform.localPosition = newPos;
                    yield return null;
                }
            }
        }

        transform.localPosition = location.Position;
        pathToTravel = null; //Clear the list
    }


    public void Die()
    {
        location.Unit = null;
        Destroy(gameObject);
    }


    #region Save and Load
    public void Save(BinaryWriter writer)
    {
        location.coordinates.Save(writer);
        writer.Write((int)orientation);
    }

    public static void Load(BinaryReader reader, HexGrid grid)
    {
        HexCoordinates coordinates = HexCoordinates.Load(reader);
        int orientation = reader.ReadInt32();
        grid.AddUnit(Instantiate(unitPrefab), grid.GetCell(coordinates), orientation);
    }
    #endregion
}
