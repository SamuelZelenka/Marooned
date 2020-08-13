public class CreateObject : Effect
{
    int objectIndex;
    public CreateObject(int objectIndex) : base((int)EffectIndex.Damage, false, false)
    {
        this.objectIndex = objectIndex;
    }
    public override string GetDescription()
    {
        return $"Create an object on the battlefield";
    }
    public HexObject SpawnNormalObject(HexCell cell)
    {
        //Spawn object
        return ObjectSpawnSystem.instance.SpawnNormalObject(objectIndex, cell);
    }
    public ActiveHexObject SpawnActiveObject(HexCell cell, int changePerTurn, Character connectedCharacter)
    {
        //Spawn object
        return ObjectSpawnSystem.instance.SpawnActiveObject(objectIndex, cell, changePerTurn, connectedCharacter);
    }

    public override void ApplyEffect(Character attacker, Character target, bool crit, bool hostile) { }
}
