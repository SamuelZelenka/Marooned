using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void ContinueGame()
    {
        Debug.LogException(new NotImplementedException());
    }
    public void NewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
    }
}
