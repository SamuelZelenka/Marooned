public class Slice : Ability
{
    int damage = 5;

    public Slice(int abilityIndex) : base(abilityIndex)
    {
        cost = 10;
        RequireSkillCheck = true;
        AttackerSkillcheck = CharacterStatType.Strength;
        TargetSkillcheck = CharacterStatType.Agility;
        effects.Add(new Damage(damage));
        effects.Add(new Bleed(damage, 2));
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
        RequireSkillCheck = true;
        AttackerSkillcheck = CharacterStatType.Strength;
        TargetSkillcheck = CharacterStatType.Agility;
        effects.Add(new Damage(damage));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}
