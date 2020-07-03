public class ChainWhip : Ability
{
    int damage = 5;
    int loyaltyDecrease = 5;
    public ChainWhip(int abilityIndex) : base(abilityIndex)
    {
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

public class GrabAndPull : Ability
{
    public GrabAndPull(int abilityIndex) : base(abilityIndex)
    {
        abilityDescription = $"Pull the target one hex closer";
        cost = 10;
        RequireSkillCheck = true;
        AttackerSkillcheck = CharacterStatType.Strength;
        TargetSkillcheck = CharacterStatType.Agility;
        effects.Add(new Displace(true, 1));
        targeting = new SingleTargetRanged();
    }
}
