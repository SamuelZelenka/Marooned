using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributionSystem : MonoBehaviour
{
    public GameObject mainPanel;
    public Text distributionTitle;
    public Button confirmButton;

    public DistributionShare distributionPrefab;
    public Transform distributionParent;

    public DistributionShare captainShare;
    public List<DistributionShare> shares = new List<DistributionShare>();

    public delegate void DistributionHandler();
    public static DistributionHandler OnSharesChanged;

    public void Setup(List<Character> charactersToShare, int totalShare, int numberOfShares)
    {
        OnSharesChanged += SharesChanged;
        mainPanel.SetActive(true);

        for (int i = 0; i < charactersToShare.Count; i++)
        {
            DistributionShare newShareObject = Instantiate(distributionPrefab);
            newShareObject.transform.SetParent(distributionParent);
            if (i > 0)
            {
                shares.Add(newShareObject);
            }
            else
            {
                captainShare = newShareObject;
            }
            newShareObject.Setup(charactersToShare[i], totalShare, numberOfShares, i == 0);
        }
    }

    public void SharesChanged()
    {
        float remainingShare = 1;

        foreach (var item in shares)
        {
            remainingShare -= item.shareFactor;
        }
        confirmButton.interactable = remainingShare > 0;
        captainShare.SetSliderValue(Utility.FactorToPercentage(remainingShare));
    }

    public void ConfirmShare()
    {
        captainShare.ConfirmShare();

        foreach (var item in shares)
        {
            item.ConfirmShare();
        }

        for (int i = 0; i < distributionParent.childCount; i++)
        {
            Destroy(distributionParent.GetChild(i));
        }

        shares.Clear();
        OnSharesChanged -= SharesChanged;
    }
}
