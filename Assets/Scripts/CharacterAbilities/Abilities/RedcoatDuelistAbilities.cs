public class Slice : Ability
{
    int damage = 5;

    public Slice(int abilityIndex) : base(abilityIndex)
    {
        cost = 10;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Strength;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Toughness;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Toughness;
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
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Strength;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        effects.Add(new Damage(damage, true, true));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}
