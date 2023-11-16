using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SquadPanelController : MonoBehaviour
{
    private float amountUpgrade = 100;

    private string shipName;
    private int selectedShip;
    [SerializeField] TMP_Text powerText, upgradeInfo, coinsText;
    [SerializeField] Sprite[] sprites;
    [SerializeField] Button shipBtn;

    private void Start()
    {
        powerText.text = GameDataManager.Instance.shipsPower[selectedShip].ToString();

    }
    public void SelectShip()
    {
        shipName = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        switch (shipName)
        {
            case "Ship1":
                selectedShip = 0;
                UpdateUpgradeInfo();
                break;
            case "Ship2":
                selectedShip = 1;
                UpdateUpgradeInfo();
                break;
            case "Ship3":
                selectedShip = 1;
                UpdateUpgradeInfo();
                break;
            case "Ship4":
                selectedShip = 1;
                UpdateUpgradeInfo();
                break;
        }
    }
    private void UpdateUpgradeInfo()
    {
        amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
        powerText.text = GameDataManager.Instance.shipsPower[selectedShip].ToString();
        shipBtn.image.sprite = sprites[selectedShip];

        upgradeInfo.text = amountUpgrade.ToString();
    }
    private void UpgradeShip()
    {
        {
            switch (selectedShip)
            {
                case 0:
                    amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
                    if (GameDataManager.Instance.coins >= amountUpgrade)
                    {
                        Upgrade();
                    }
                    break;
                case 1:
                    amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
                    if (GameDataManager.Instance.coins >= amountUpgrade)
                    {
                        Upgrade();
                    }
                    break;
            }
        }
    }
    private void Upgrade()
    {

        GameDataManager.Instance.selectedShip = selectedShip;
        int power = GameDataManager.Instance.shipsPower[selectedShip] + 100;
        GameDataManager.Instance.shipsPower[selectedShip] = power;
        GameDataManager.Instance.coins -= (int)amountUpgrade;
        GameDataManager.Instance.shipsRank[selectedShip]++;
        powerText.text = GameDataManager.Instance.shipsPower[selectedShip].ToString();
        amountUpgrade = Mathf.Round(GameDataManager.Instance.shipsRank[selectedShip] * 100 * 1.1f);
        upgradeInfo.text = amountUpgrade.ToString();

        coinsText.text = GameDataManager.Instance.coins.ToString();

        GameDataManager.Instance.Save();
    }

}
