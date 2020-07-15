using System.Collections.Generic;

public class Quickdraw : Ability
{
    int abilityCost = 10;
    int range = 8;
    int rangeAfterFirstHit = 1;
    int damage = 5;

    public Quickdraw(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Accuracy;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        effects.Add(new Damage(damage, true, true));
        targeting = new CollateralRangeLine(range, rangeAfterFirstHit);
        base.SetDescriptionFromEffects();
    }
}

public class PiercingShot : Ability
{
    int abilityCost = 10;
    int range = 8;
    int damage = 3;

    public PiercingShot(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Accuracy;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        effects.Add(new PiercingDamage(damage, true, true));
        targeting = new SingleTargetRanged(range);
        base.SetDescriptionFromEffects();
    }
}

public class Shockwave : Ability
{
    int abilityCost = 30;
    int range = 8;
    int damage = 5;

    public Shockwave(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Accuracy;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        effects.Add(new Damage(damage, true, true));
        effects.Add(new Displace(false, 1, true, true));
        targeting = new SingleTargetRangeLine(range);
        base.SetDescriptionFromEffects();
    }
}

public class TheBigBoom : Ability
{
    int abilityCost = 100;
    int range = 8;
    int damageRange = 2;
    int damage = 5;

    public TheBigBoom(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Accuracy;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        effects.Add(new Damage(damage, true, true));
        targeting = new AOE(range, damageRange, false);
        base.SetDescriptionFromEffects();
    }
}