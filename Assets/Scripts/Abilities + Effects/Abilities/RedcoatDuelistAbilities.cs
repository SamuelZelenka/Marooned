public class Slice : Ability
{
    int damage = 5;

    public Slice(int abilityIndex) : base(abilityIndex)
    {
        cost = 10;

        FriendlyHitChanceSkillcheck = CharacterStatType.Strength;
        HostileHitChanceSkillcheck = CharacterStatType.Strength;

        effects.Add(new Damage(damage, true, true));
        effects.Add(new Bleed(damage, 2, true, true));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}

public class Parry : Ability //Not implemented
{
    int damage = 5;

    public Parry(int abilityIndex) : base(abilityIndex)
    {
        cost = 10;

        FriendlyHitChanceSkillcheck = CharacterStatType.NONE;
        HostileHitChanceSkillcheck = CharacterStatType.NONE;

        effects.Add(new Damage(damage, true, true));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}
