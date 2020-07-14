public class PoisonBlade : Ability
{
    int abilityCost = 10;
    int poison = 2;
    int poisonDuration = 2;

    public PoisonBlade(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Accuracy;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Constitution;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Constitution;
        effects.Add(new Poison(poison, poisonDuration, true, true));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}

public class SneakAttack : Ability
{
    int abilityCost = 10;
    int normalDamage = 5;
    int stunnedDamage = 15;

    public SneakAttack(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Accuracy;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        effects.Add(new ConditionalDamage(normalDamage, stunnedDamage, true, true, Condition.Stunned));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}

public class CutInTheKnee : Ability
{
    int abilityCost = 30;
    int agilityDebuff = 5;
    int duration = 2;

    public CutInTheKnee(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Accuracy;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        effects.Add(new StatDebuff(agilityDebuff, CharacterStatType.Agility, duration, true, true));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}

public class LethalDose : Ability
{
    int abilityCost = 100;
    int range = 0;
    int normalDamage = 15;
    int poisonedDamage = 30;

    public LethalDose(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        AbilityuserHitSkillcheck = SkillcheckSystem.SkillcheckRequirement.Accuracy;
        HostileDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        FriendlyDodgeSkillcheck = SkillcheckSystem.SkillcheckRequirement.Agility;
        effects.Add(new ConditionalDamage(normalDamage, poisonedDamage, true, true, Condition.Poisoned));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}