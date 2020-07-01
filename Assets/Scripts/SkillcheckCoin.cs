using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillcheckCoin : MonoBehaviour
{
    const float percentageToWin = 50;
    [SerializeField] Image coinFace = null;
    [SerializeField] Sprite winSprite = null, lossSprite = null, undecidedSprite = null;
    const float flipTime = 0.5f;
    const float maxRotation = 50f;

    public static bool FlipCoin() => Random.Range(0, 100) >= percentageToWin;

    public void TurnCoinToUndecided()
    {
        coinFace.sprite = undecidedSprite;
    }

    public IEnumerator VisualizeCoinFlip(bool win)
    {
        float flipTimer = 0;

        //Rotate
        while (flipTimer < flipTime)
        {
            float rotation = maxRotation * (flipTime - flipTimer) / flipTime;
            coinFace.sprite = Utility.ReturnRandom(winSprite, lossSprite);
            this.transform.Rotate(new Vector3(0, rotation, 0));
            yield return null;
            flipTimer += Time.deltaTime;
        }

        this.transform.rotation = Quaternion.identity;
        coinFace.sprite = win ? winSprite : lossSprite;
    }
}
