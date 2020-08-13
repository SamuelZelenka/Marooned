public class GuitarString : Ability
{
    int abilityCost = 10;
    int bleed = 2;
    int bleedDuration = 2;
    int heal = 5;

    public GuitarString(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;

        FriendlyHitChanceSkillcheck = CharacterStatType.NONE;
        HostileHitChanceSkillcheck = CharacterStatType.Accuracy;

        effects.Add(new Bleed(bleed, bleedDuration, true, false));
        effects.Add(new Heal(heal, false, true));
        targeting = new SingleTargetAdjacent();
        base.SetDescriptionFromEffects();
    }
}

public class FightSong : Ability
{
    int abilityCost = 10;
    int range = 8;
    int accuracyBuff = 5;
    int accuracyDebuff = 5;
    int buffDuration = 2;
    int debuffDuration = 2;
    int loyaltyIncrease = 5;

    public FightSong(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;

        FriendlyHitChanceSkillcheck = CharacterStatType.NONE;
        HostileHitChanceSkillcheck = CharacterStatType.Accuracy;

        effects.Add(new StatBuff(accuracyBuff, CharacterStatType.Accuracy, buffDuration, false, true));
        effects.Add(new LoyaltyIncrease(loyaltyIncrease, false, true));
        effects.Add(new StatDebuff(accuracyDebuff, CharacterStatType.Accuracy, debuffDuration, true, false));
        targeting = new SingleTargetRanged(range);
        base.SetDescriptionFromEffects();
    }
}

public class ToneDeafSinging : Ability
{
    int abilityCost = 30;
    int range = 8;
    int loyaltyDecrease = 25;

    public ToneDeafSinging(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;

        FriendlyHitChanceSkillcheck = CharacterStatType.NONE;
        HostileHitChanceSkillcheck = CharacterStatType.Accuracy;

        effects.Add(new LoyaltyDecrease(loyaltyDecrease, true, false));
        targeting = new SingleTargetRanged(range);
        base.SetDescriptionFromEffects();
    }
}

public class RuleBritannia : Ability
{
    int abilityCost = 100;
    int range = 0;
    int aoeRange = 2;
    int duration = 1;

    public RuleBritannia(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;

        FriendlyHitChanceSkillcheck = CharacterStatType.NONE;
        HostileHitChanceSkillcheck = CharacterStatType.Accuracy;

        effects.Add(new Stun(duration, true, false));
        targeting = new AOE(range, aoeRange, true);
        base.SetDescriptionFromEffects();
    }
}