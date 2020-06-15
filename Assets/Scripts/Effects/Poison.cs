public class Poison : Effect
{
    int damage;
    public Poison(int damage)
    {
        description = $"Reduces vitality by {damage}";
        this.damage = damage;
    }
    public override void ApplyEffect(Character character){}
    public override void EffectTick(Character character)
    {
        character.characterData.Vitality -= damage;
    }
    public override string GetData()
    {
        return "";
    }
}