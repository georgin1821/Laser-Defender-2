using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopInfoPanel : MonoBehaviour
{
    [SerializeField] TMP_Text coinsText, batteryTxt, gemsTxt;

    void Start()
    {
        coinsText.text = GameDataManager.Instance.coins.ToString();
        gemsTxt.text = GameDataManager.Instance.gems.ToString();
        batteryTxt.text = GameDataManager.Instance.batteryLife + "%";

    }

}
