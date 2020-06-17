using UnityEngine;

public class AutoInactive : MonoBehaviour
{
    public enum CallTime { Awake, Start}
    public CallTime whenToSet;

    private void Awake()
    {
        if (whenToSet == CallTime.Awake)
        {
            SetInactive();
        }
    }

    private void Start()
    {
        if (whenToSet == CallTime.Start)
        {
            SetInactive();
        }
    }

    private void SetInactive() => this.gameObject.SetActive(false);
}
