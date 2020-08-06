using UnityEditor.PackageManager.Requests;
using UnityEngine;

public abstract class Quest : ScriptableObject
{
    public string title;
    [SerializeField] string blueprintDescription = "";
    [HideInInspector] public string description;

    public string messageOnActivated;
    public string messageOnCompleted;
    public string messageOnFailed;

    public delegate void QuestHandler();
    public QuestHandler OnQuestCompleted;

    public enum POIRequest { StartingHarbor, Stronghold}
    [SerializeField] POIRequest request = POIRequest.StartingHarbor;
    public POIRequest Request { get; private set; }
    protected Player player;
    protected PointOfInterest poi;
    public virtual void Setup(Player player, PointOfInterest poi)
    {
        Request = request;
        this.player = player;
        this.poi = poi;
        SetTravelPointName(poi.Name);
    }

    void SetTravelPointName(string locationName) => description = Utility.ReplaceWordInString(blueprintDescription, "NAME", locationName);


    protected virtual void CompleteQuest()
    {
        player.PlayerData.RemoveQuest(this, true);
    }
}