using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private static UIController _instance;
    public static UIController MyInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIController>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    [SerializeField] private Text evadeTxt, healthTxt, manaTxt, shootTxt;     // for debuging purpose
    [SerializeField] private AI_EvadeEnemy MyAi_Evade;
    [SerializeField] private AI_SeekHealth MyAi_SeekHeal;
    [SerializeField] private AI_SeekMana MyAi_SeekMana;
    [SerializeField] private AI_ShootEnemy MyAi_Shoot;
    [SerializeField] private Button restartBtn;

    private float maxWeight;

    [SerializeField] private Text killCountTxt;
    public int killCount = 0;
    public Slider actualAiState;

    void Update()
    {
        if (HeroAI_Controller.MyInstance != null)
        {
            maxWeight = HeroAI_Controller.MyInstance.MyMaxWeight;
        }

        //UI Debug Only
        evadeTxt.text = "Evade: " + MyAi_Evade.MyWeight;
        healthTxt.text = "Heal: " + MyAi_SeekHeal.MyWeight;
        manaTxt.text = "Mana: " + MyAi_SeekMana.MyWeight;
        shootTxt.text = "Fight: " + MyAi_Shoot.MyWeight;

        killCountTxt.text = "Kills: " + killCount;
    }

    public void SetSliderValue(float value)
    {
        actualAiState.value = value;
    }

    public void SetBtnActive()
    {
        restartBtn.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
