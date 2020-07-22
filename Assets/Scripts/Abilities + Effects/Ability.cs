using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    public static Dictionary<int, Ability> abilityDictionary = new Dictionary<int, Ability>()
    {
        {0, new ChainWhip(0) },
        {1, new GrabAndPull(1) },
        {2, new Punch(2) },
        {3, new WarCry(3) },
        {10, new PoisonBlade(10) },
        {11, new SneakAttack(11) },
        {12, new CutInTheKnee(12) },
        {13, new LethalDose(13) },
        {20, new Quickdraw(20) },
        {21, new PiercingShot(21) },
        {22, new Shockwave(22) },
        {23, new TheBigBoom(23) },
        {30, new GuitarString(30) },
        {31, new FightSong(31) },
        {32, new ToneDeafSinging(32) },
        {33, new RuleBritannia(33) },
        {40, new Cleave(40) },
        {41, new CarefulIncision(41) },
        {42, new BadMedicine(42) },
        {43, new TheGoodStuff(43) },
        {100, new Slice(100) }
    };

    public string abilityName;
    public string abilityDescription;
    public Sprite AbilitySprite
    {
        private set;
        get;
    }

    public int cost;

    public SkillcheckSystem.SkillcheckRequirement AbilityuserHitSkillcheck
    {
        protected set;
        get;
    }
    public SkillcheckSystem.SkillcheckRequirement HostileDodgeSkillcheck
    {
        protected set;
        get;
    }
    public SkillcheckSystem.SkillcheckRequirement FriendlyDodgeSkillcheck
    {
        protected set;
        get;
    }

    protected List<Effect> effects = new List<Effect>();
    public TargetType targeting;

    const string path = "AbilitySprites/";
    protected Ability(int abilityIndex)
    {
        abilityName = ToString();
        AbilitySprite = Resources.Load<Sprite>(path + "AbilityIcon" + abilityIndex);
    }

    //No decided outcomes required (autohits)
    public virtual void Use(Character attacker, List<Character> hostileTargets, List<Character> friendlyTargets, List<HexCell> affectedCells)
    {
        List<SkillcheckSystem.CombatOutcome> hostileOutcomes = new List<SkillcheckSystem.CombatOutcome>();
        for (int i = 0; i < hostileTargets.Count; i++)
        {
            hostileOutcomes.Add(SkillcheckSystem.CombatOutcome.NormalHit);
        }
        List<SkillcheckSystem.CombatOutcome> friendlyOutcomes = new List<SkillcheckSystem.CombatOutcome>();
        for (int i = 0; i < friendlyTargets.Count; i++)
        {
            friendlyOutcomes.Add(SkillcheckSystem.CombatOutcome.NormalHit);
        }
        Use(attacker, hostileTargets, hostileOutcomes, friendlyTargets, friendlyOutcomes, affectedCells);
    }

    public virtual void Use(Character attacker, List<Character> hostileTargets, List<SkillcheckSystem.CombatOutcome> hostileOutcomes, List<Character> friendlyTargets, List<SkillcheckSystem.CombatOutcome> friendlyOutcomes, List<HexCell> affectedCells)
    {
        for (int i = 0; i < hostileTargets.Count; i++)
        {
            if (hostileOutcomes[i] == SkillcheckSystem.CombatOutcome.Miss)
            {
                continue;
            }
            foreach (var item in effects)
            {
                item.ApplyEffect(attacker, hostileTargets[i], hostileOutcomes[i], true);
            }
        }
        for (int i = 0; i < friendlyTargets.Count; i++)
        {
            if (friendlyOutcomes[i] == SkillcheckSystem.CombatOutcome.Miss)
            {
                continue;
            }
            foreach (var item in effects)
            {
                item.ApplyEffect(attacker, friendlyTargets[i], friendlyOutcomes[i], false);
            }
        }
    }
    protected void SetDescriptionFromEffects()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            abilityDescription += effects[i].GetDescription();
            if (i == effects.Count - 1)
            {
                abilityDescription += "\n";
            }
        }
    }
    public string CreateCombatLogMessage(Character attacker, List<Character> targets)
    {
        string targetsToString = "";

        // Billy, John, Robert and Andrew
        if (targets.Count > 0)
        {
            if (targets.Count > 1)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    if (i == targets.Count - 1)
                    {
                        targetsToString += $"and {targets[i].characterData.CharacterName}.";
                    }
                    else if (i == targets.Count - 2)
                    {
                        targetsToString += $"{targets[i].characterData.CharacterName} ";
                    }
                    else
                    {
                        targetsToString += $"{targets[i].characterData.CharacterName}, ";
                    }
                }
            }
            else
            {
                targetsToString = $"{targets[0].characterData.CharacterName}.";
            }
        }
        else
        {
            return $"Used {abilityName} but completely failed at aiming.";
        }
        return $"Used {abilityName} on {targetsToString}";
    }
}