using UnityEngine;
using UnityEngine.UI;

public class StringRandomizer : MonoBehaviour
{
    [SerializeField] InputField text = null;
    [SerializeField] string[] randomStrings = null;
    public void PrintRandomString() => text.text = GetRandomString();

    public string GetRandomString() => Utility.ReturnRandom(randomStrings);
}
