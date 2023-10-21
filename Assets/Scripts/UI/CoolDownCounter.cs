using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoolDownCounter : MonoBehaviour
{
    [SerializeField] TMP_Text cooldownText;
    [SerializeField] GameObject cdPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] AudioClip cdClip;

    public void StartCountDown()
    {
        StartCoroutine(countDownTimer());
    }

    IEnumerator countDownTimer()
    {
        pausePanel.SetActive(false);
        cdPanel.SetActive(true);
        AudioController.Instance.PlayAudio(AudioType.countDown);
        float time = 3;
        while (time > 0)
        {
            time -= Time.unscaledDeltaTime;
            cooldownText.text = time.ToString("N0");
            yield return null;
        }

        cdPanel.SetActive(false);
        GamePlayController.Instance.UpdateState(GameState.PLAY);
    }
}
