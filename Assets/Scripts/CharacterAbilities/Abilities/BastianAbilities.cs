using System.Collections.Generic;

public class ChainWhip : Ability
{
    int damage = 5;
    int loyaltyDecrease = 5;
    public ChainWhip(int abilityIndex) : base(abilityIndex)
    {
        cost = 10;
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
    int range = 3;

    public GrabAndPull(int abilityIndex) : base(abilityIndex)
    {
        cost = 10;
        RequireSkillCheck = true;
        AttackerSkillcheck = CharacterStatType.Strength;
        TargetSkillcheck = CharacterStatType.Agility;
        effects.Add(new Displace(true, 1));
        targeting = new SingleTargetRangeLine(range, true);
        base.SetDescriptionFromEffects();
    }
}

public class Punch : Ability
{
    int damage = 10;
    int durationStun = 1;
    public Punch(int abilityIndex) : base(abilityIndex)
    {
        cost = 20;
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
    int loyaltyIncrease = 5;
    int durationTaunt = 1;
    int buffrange = 2;
    public WarCry(int abilityIndex) : base(abilityIndex)
    {
        cost = 100;
        RequireSkillCheck = false;
        effects.Add(new Taunt(durationTaunt));
        effects.Add(new LoyaltyIncrease(loyaltyIncrease));
        targeting = new SelfAOE(buffrange);
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
            foreach (var item in effects)
            {
                //Apply taunt on self
                if (item is Taunt && attacker == targets[i])
                {
                    item.ApplyEffect(attacker, targets[i], outcomes[i]);
                }
                if (item is LoyaltyIncrease && attacker != targets[i]) //Apply buff in loyalty to other than self
                {
                    item.ApplyEffect(attacker, targets[i], outcomes[i]);
                }
            }
        }
    }
}