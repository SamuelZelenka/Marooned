public class ChainWhip : Ability
{
    int damage = 5;
    int loyaltyDecrease = 5;
    public ChainWhip(int abilityIndex) : base(abilityIndex)
    {
        abilityName = this.ToString();
        abilityDescription = $"Deal {damage} vitality damage and reduce {loyaltyDecrease} loyalty from up to three adjacent targets";
        cost = 10;
        RequireSkillCheck = true;
        AttackerSkillcheck = CharacterStatType.Strength;
        TargetSkillcheck = CharacterStatType.Agility;
        effects.Add(new Damage(damage));
        effects.Add(new LoyaltyDecrease(loyaltyDecrease));
        targeting = new SwipeAdjacent();
    }
}

public class GrabAndPull
{

}
