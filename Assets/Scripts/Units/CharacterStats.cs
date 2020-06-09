[System.Serializable]
public class CharacterStats
{

    //Stats
    int strength = 0;
    int stamina = 0;
    int constitution = 0;
    int agility = 0;
    int toughness = 0;
    int accuracy = 0;
    int bounty = 0;
   
    public int Strength
    {
        get { return strength; }
        set
        {
            strength = value;
        }
    }
    public int Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;
        }
    }

    public int Constitutuion
    {
        get { return constitution; }
        set
        {
            constitution = value;
        }
    }

    public int Agility
    {
        get { return agility; }
        set
        {
            agility = value;
        }
    }

    public int Toughness
    {
        get { return toughness; }
        set
        {
            toughness = value;
        }
    }

    public int Accuracy
    {
        get { return accuracy; }
        set
        {
            accuracy = value;
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