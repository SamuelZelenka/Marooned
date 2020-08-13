public static class SessionSetupTransfer
{
    //#region Singleton
    //public static SessionSetupTransfer instance;
    //private void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Debug.Log("Another instance of : " + instance.ToString() + " was tried to be instanced, but was destroyed from gameobject: " + this.transform.name);
    //        GameObject.Destroy(this);
    //        return;
    //    }
    //    instance = this;
    //}
    //#endregion

    //private void Start()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //}

    static SetupData sessionData;

    public static void SetNewSession(SetupData newSessionData) => sessionData = newSessionData;

    public static SetupData GetSetupDataAndDestroyInstance()
    {
        //Destroy(instance.gameObject, 0.1f);
        return sessionData;
    }
}
