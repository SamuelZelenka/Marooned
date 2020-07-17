public class ChainWhip : Ability
{
    int abilityCost = 10;
    int damage = 5;
    int loyaltyDecrease = 5;

    public ChainWhip(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Strength;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        effects.Add(new Damage(damage, true, true));
        effects.Add(new LoyaltyDecrease(loyaltyDecrease, true, true));
        targeting = new SwipeAdjacent();
        base.SetDescriptionFromEffects();
    }
}

public class GrabAndPull : Ability
{
    int abilityCost = 10;
    int range = 3;

    public GrabAndPull(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Strength;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        effects.Add(new Displace(true, 1, true, true));
        targeting = new SingleTargetRangeLine(range, true);
        base.SetDescriptionFromEffects();
    }
}

public class Punch : Ability
{
    int abilityCost = 30;
    int damage = 10;
    int durationStun = 1;

    public Punch(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Strength;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        effects.Add(new Damage(damage, true, true));
        effects.Add(new Stun(durationStun, true, true));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}

public class WarCry : Ability
{
    int abilityCost = 100;
    int loyaltyIncrease = 5;
    int durationTaunt = 1;
    int buffrange = 2;

    public WarCry(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.None;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.None;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.None;
        effects.Add(new Taunt(durationTaunt));
        effects.Add(new LoyaltyIncrease(loyaltyIncrease, false, true));
        targeting = new AOE(0, buffrange, true);
        base.SetDescriptionFromEffects();
    }
}