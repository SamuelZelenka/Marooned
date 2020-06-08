using UnityEngine;
[System.Serializable]
public class CharacterResources
{
    //Resources
    [SerializeField] int vitality = 0;
    //int speed;
    int energy = 0;
    int hunger = 0;
    int hygiene = 0;
    int bounty = 0;

    public int Vitality {
        get { return vitality; }
        set {
            vitality = Mathf.Clamp(value, 0, int.MaxValue);
            if (vitality <= 0)
            {
                Debug.Log("Is dead");
            }
        } 
    }
    public int Energy
    {
        get { return energy; }
        set
        {
            energy = value;
        }
    }
    public int Hunger
    {
        get { return hunger; }
        set
        {
            hunger = value;
        }
    }
    public int Hygiene
    {
        get { return hygiene; }
        set
        {
            hygiene = value;
        }
    }
    public int Bounty
    {
        get { return bounty; }
        set
        {
            bounty = value;
        }
    }
}