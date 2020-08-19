using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillcheckCoin : MonoBehaviour
{
    const float PERCENTAGETOWIN = 50;
    [SerializeField] Image coinFace = null;
    [SerializeField] Sprite winSprite = null, lossSprite = null, undecidedSprite = null;
    public const float FLIPTIME = 0.5f;
    const float MAXROTATION = 50f;

    public static bool GetCoinflipResult() => Random.Range(0, 100) >= PERCENTAGETOWIN;

    public void TurnCoinToUndecided()
    {
        coinFace.sprite = undecidedSprite;
    }

    public IEnumerator VisualizeCoinFlip(bool win)
    {
        float flipTimer = 0;

        //Rotate
        while (flipTimer < FLIPTIME)
        {
            float rotation = MAXROTATION * (FLIPTIME - flipTimer) / FLIPTIME;
            coinFace.sprite = Utility.ReturnRandom(winSprite, lossSprite);
            this.transform.Rotate(new Vector3(0, rotation, 0));
            yield return null;
            flipTimer += Time.deltaTime;
        }

        this.transform.rotation = Quaternion.identity;
        coinFace.sprite = win ? winSprite : lossSprite;
    }
}
