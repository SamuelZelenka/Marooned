public class PoisonBlade : Ability
{
    int abilityCost = 10;
    int poison = 2;
    int poisonDuration = 2;

    public PoisonBlade(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;

        FriendlyHitChanceSkillcheck = CharacterStatType.Accuracy;
        HostileHitChanceSkillcheck = CharacterStatType.Accuracy;

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

        FriendlyHitChanceSkillcheck = CharacterStatType.Accuracy;
        HostileHitChanceSkillcheck = CharacterStatType.Accuracy;

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

        FriendlyHitChanceSkillcheck = CharacterStatType.Accuracy;
        HostileHitChanceSkillcheck = CharacterStatType.Accuracy;

        effects.Add(new StatDebuff(agilityDebuff, CharacterStatType.Agility, duration, true, true));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}

public class LethalDose : Ability
{
    int abilityCost = 100;
    int damagePerPoison = 5;

    public LethalDose(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;

        FriendlyHitChanceSkillcheck = CharacterStatType.Accuracy;
        HostileHitChanceSkillcheck = CharacterStatType.Accuracy;

        effects.Add(new StackDamage(damagePerPoison, true, true, Condition.Poisoned));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}