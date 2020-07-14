public class CreateObject : Effect
{
    ObjectType type;
    public CreateObject(ObjectType type) : base((int)EffectIndex.Damage, false, false)
    {
        this.type = type;
    }
    public override string GetDescription()
    {
        return $"Create an object on the battlefield";
    }
    public void UseSpecialEffect(HexCell cell)
    {
        //Spawn object
        ObjectSpawnSystem.instance.SpawnObject(type, cell);
    }
    public override void ApplyEffect(Character attacker, Character target, SkillcheckSystem.CombatOutcome outcome, bool hostile) { }
}
