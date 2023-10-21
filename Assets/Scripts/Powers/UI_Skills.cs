using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Skills : MonoBehaviour
{
    [SerializeField] GameObject skillPrefab, skillButton;
    [SerializeField] Image fillImage;

    float coolDownTime = 50f;
    float cooldownTimer = 0.0f;
    bool isCooldown = true;


    public IEnumerator ApplySkill1()
    {
        cooldownTimer = coolDownTime;
        fillImage.gameObject.SetActive(true);
        Instantiate(skillPrefab, skillPrefab.transform.position, Quaternion.identity);

        if (isCooldown)
        {
            isCooldown = false;
            while (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;

                fillImage.fillAmount = cooldownTimer / coolDownTime;
                yield return null;
            }

        }
        isCooldown = true;
        fillImage.fillAmount = 100;
        fillImage.gameObject.SetActive(false);

    }

    public void ApplySkill()
    {
        if (isCooldown)
        {
            StartCoroutine(ApplySkill1());
        }
    }
}
