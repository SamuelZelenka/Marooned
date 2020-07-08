using System.Collections.Generic;

public class ChainWhip : Ability
{
    int abilityCost = 10;
    int damage = 5;
    int loyaltyDecrease = 5;

    public ChainWhip(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        RequireSkillCheck = true;
        AttackerSkillcheck = CharacterStatType.Strength;
        TargetSkillcheck = CharacterStatType.Agility;
        effects.Add(new Damage(damage));
        effects.Add(new LoyaltyDecrease(loyaltyDecrease));
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
        RequireSkillCheck = true;
        AttackerSkillcheck = CharacterStatType.Strength;
        TargetSkillcheck = CharacterStatType.Agility;
        effects.Add(new Displace(true, 1));
        targeting = new SingleTargetRangeLine(range);
        base.SetDescriptionFromEffects();
    }
}

public class Punch : Ability
{
    int abilityCost = 20;
    int damage = 10;
    int durationStun = 1;

    public Punch(int abilityIndex) : base(abilityIndex)
    {
        cost = abilityCost;
        RequireSkillCheck = true;
        AttackerSkillcheck = CharacterStatType.Strength;
        TargetSkillcheck = CharacterStatType.Agility;
        effects.Add(new Damage(damage));
        effects.Add(new Stun(durationStun));
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
        RequireSkillCheck = false;
        effects.Add(new Taunt(durationTaunt));
        effects.Add(new LoyaltyIncrease(loyaltyIncrease));
        targeting = new AOE(0, buffrange, true);
        base.SetDescriptionFromEffects();
    }

    public override void Use(Character attacker, List<Character> targets, List<SkillcheckSystem.CombatOutcome> outcomes)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (outcomes[i] == SkillcheckSystem.CombatOutcome.Miss)
            {
                continue;
            }
            foreach (var effect in effects)
            {
                //Apply taunt on self
                if (effect is Taunt && attacker == targets[i])
                {
                    effect.ApplyEffect(attacker, targets[i], outcomes[i]);
                }
                if (effect is LoyaltyIncrease && attacker != targets[i] && attacker.playerControlled == targets[i].playerControlled) //Apply buff in loyalty to other than self but only on same team
                {
                    effect.ApplyEffect(attacker, targets[i], outcomes[i]);
                }
            }
        }
    }
}