using UnityEngine;
[System.Serializable]
public class CharacterResources
{
    //Resources
    [SerializeField] int vitality = 30;
    [SerializeField] int maxVitality = 30;
    //int speed;
    [SerializeField] float energy = 1;
    [SerializeField] float hunger = 1;
    [SerializeField] float hygiene = 1;
    [SerializeField] float loyalty = 0.5f;

    public int MaxVitality
    {
        get { return maxVitality; }
        set
        {
            maxVitality = value;
        }
    }

    public int Vitality {
        get { return vitality; }
        set {
            vitality = Mathf.Clamp(value, 0, maxVitality);
            if (vitality <= 0)
            {
                Debug.Log("Is dead");
            }
        } 
    }
    public float Energy
    {
        get { return energy; }
        set
        {
            energy = Mathf.Min(value, 1);
        }
    }
    public float Hunger
    {
        get { return hunger; }
        set
        {
            hunger = Mathf.Min(value, 1);
        }
    }
    public float Hygiene
    {
        get { return hygiene; }
        set
        {
            hygiene = Mathf.Min(value, 1);
        }
    }

    public float Loyalty
    {
        get { return loyalty; }
        set
        {
            loyalty = Mathf.Min(value, 1);
        }
    }

    //public enum ResourceType { Vitality, Energy, Hunger, Hygiene, Loyalty}
    //public float GetResource(ResourceType resourceType)
    //{
    //    switch (resourceType)
    //    {
    //        case ResourceType.Vitality:
    //            return Vitality;
    //        case ResourceType.Energy:
    //            return Energy;
    //        case ResourceType.Hunger:
    //            return Hunger;
    //        case ResourceType.Hygiene:
    //            return Hygiene;
    //        case ResourceType.Loyalty:
    //            return Loyalty;
    //        default:
    //            Debug.LogError("Not set up resourcetype");
    //            return 0;
    //    }
    //}
}