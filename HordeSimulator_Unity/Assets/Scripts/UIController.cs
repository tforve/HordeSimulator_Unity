using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    [SerializeField] private Text heighestTxt, evadeTxt, healthTxt, manaTxt, shootTxt;                         // for debuging purpose
    [SerializeField] private AI_EvadeEnemy MyAi_Evade;
    [SerializeField] private AI_SeekHealth MyAi_SeekHeal;
    [SerializeField] private AI_SeekMana MyAi_SeekMana;
    [SerializeField] private AI_ShootEnemy MyAi_Shoot;

    private float maxWeight;

    [SerializeField] private Text killCountTxt;
    private int killCount = 0;
    public Slider actualAiState;

    // Update is called once per frame
    void Update()
    {
        if (HeroAI_Controller.MyInstance != null)
            maxWeight = HeroAI_Controller.MyInstance.MyMaxWeight;
        //UI Debug Only
        heighestTxt.text = "Highest Weight: " + maxWeight;
        evadeTxt.text = "Evade: " + MyAi_Evade.MyWeight;
        healthTxt.text = "Heal: " + MyAi_SeekHeal.MyWeight;
        manaTxt.text = "Mana: " + MyAi_SeekMana.MyWeight;
        shootTxt.text = "Fight: " + MyAi_Shoot.MyWeight;
    }

    public void SetSliderValue(float value)
    {
        actualAiState.value = value;
    }
}
