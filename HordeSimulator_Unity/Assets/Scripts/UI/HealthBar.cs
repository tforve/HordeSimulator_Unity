using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healBarImg;
    [SerializeField] private Image manaBarImg;

    private float updateSpeedSeconds = 0.25f;

    private void Awake()
    {
        GetComponentInParent<Character>().OnHealthChanged += HandleHealthChanged;
        GetComponentInParent<Character>().OnManaChanged += HandleManaChanged;
    }

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeHealthToPct(pct));
    }

    private void HandleManaChanged(float pct)
    {
        StartCoroutine(ChangeManaToPct(pct));
    }

    private IEnumerator ChangeHealthToPct(float pct)
    {
        float preChangePct = healBarImg.fillAmount;
        float elapse = 0.0f;
        while(elapse < updateSpeedSeconds)
        {
            elapse += Time.deltaTime;
            healBarImg.fillAmount = Mathf.Lerp(preChangePct, pct, elapse / updateSpeedSeconds);
            yield return null;
        }
        healBarImg.fillAmount = pct;
    }

    private IEnumerator ChangeManaToPct(float pct)
    {
        float preChangePct = manaBarImg.fillAmount;
        float elapse = 0.0f;
        while(elapse < updateSpeedSeconds)
        {
            elapse += Time.deltaTime;
            manaBarImg.fillAmount = Mathf.Lerp(preChangePct, pct, elapse / updateSpeedSeconds);
            yield return null;
        }
        manaBarImg.fillAmount = pct;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0,180,0);
    }


}
