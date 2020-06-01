public class Bleed : Effect
{
    int damage;
    public Bleed(int damage)
    {
        description = $"Reduces {damage} vitality every turn.";
        this.damage = damage;
    }
    public override void ApplyEffect(Character character){}
    public override void EffectTick(Character character)
    {
        character.resources.Vitality -= damage; 
    }
}