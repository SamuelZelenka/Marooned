public class Damage : Effect
{
    int damage;
    public Damage(int damage)
    {
        Description = $"Reduces {damage} vitality.";
        this.damage = damage;
    }
    public override void ApplyEffect(Character character)
    {
        character.characterData.Vitality.CurrentValue -= damage;
    }
}
