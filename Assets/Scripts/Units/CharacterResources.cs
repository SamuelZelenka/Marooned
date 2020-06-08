using UnityEngine;
[System.Serializable]
public class CharacterResources
{
    //Resources
    [SerializeField] int vitality;
    int speed;
    int energy;
    int hunger;
    int hygiene;
    int bounty;

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
}