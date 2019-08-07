using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text heighestTxt, evadeTxt, healthTxt, manaTxt, shootTxt;                         // for debuging purpose
    [SerializeField] private AI_EvadeEnemy MyAi_Evade;
    [SerializeField] private AI_SeekHealth MyAi_SeekHeal;
    [SerializeField] private AI_SeekMana MyAi_SeekMana;
    [SerializeField] private AI_ShootEnemy MyAi_Shoot;

    private float maxWeight;
    
    [SerializeField] private Text killCountTxt;
    private int killCount = 0;
    [SerializeField] private Slider actualAiState;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(HeroAI_Controller.MyInstance != null)
        maxWeight = HeroAI_Controller.MyInstance.MyMaxWeight;
        //UI Debug Only
        heighestTxt.text = "Decision: " + maxWeight;
        evadeTxt.text = "Evade: " + MyAi_Evade.MyWeight;
        healthTxt.text = "Heal: " + MyAi_SeekHeal.MyWeight;
        manaTxt.text = "Mana: " + MyAi_SeekMana.MyWeight;
        shootTxt.text = "Fight: " + MyAi_Shoot.MyWeight;
    }
}
