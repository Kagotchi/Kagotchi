using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class csFightPlanSceneManager : MonoBehaviour,csISceneManager 
{

    private Slider manaSlider;
    private Slider staminaSlider;

    private GameObject turnGrid;

    public int Turn { get; set; }
    public List<csFightPlanPrefab> Turns { get; set; }

    void Awake()
    {
        var objManaSlider = GameObject.Find("Mana Slider");
        manaSlider = objManaSlider.GetComponent<Slider>();
        var objStaminaSlider = GameObject.Find("Stamina Slider");
        staminaSlider = objStaminaSlider.GetComponent<Slider>();
        Turns = new List<csFightPlanPrefab>();
        manaSlider.value = csGameController.control.Kagotchi.Mana;
        staminaSlider.value = csGameController.control.Kagotchi.Stamina;
        turnGrid = GameObject.Find("TurnGrid");
    }
	// Use this for initialization
	void Start () 
    {
        CreateNewTurn();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnApplicationPause(bool paused)
    {
    }

    public void UpdateScenery(bool scroll, float direction)
    {
    }

    public void SetNoItemVisibility(bool visible)
    {
    }

    public void CreateNewTurn()
    {
        var itemObject = Resources.Load("Prefabs/Fight Plan");
        if (itemObject != null)
        {
            var turn = (GameObject)Instantiate(itemObject);
            var turnElement = turn.GetComponent<csFightPlanPrefab>();
            Turn += 1;
            turnElement.Turn = Turn;
            turnElement.txtTurn.text = Turn.ToString();
            turn.transform.SetParent(turnGrid.transform, false);
            Turns.Add(turnElement);

            var button = turn.GetComponentInChildren<Button>();
            if (button != null && Turn == 1)
            {
                var canvasGrp = button.GetComponent<CanvasGroup>();
                canvasGrp.alpha = 0;
                canvasGrp.blocksRaycasts = false;
                canvasGrp.interactable = false;
            }
        }
    }

    public void DeleteTurn(GameObject child)
    {
        var turn = child.transform.parent;
        var element = turn.GetComponent<csFightPlanPrefab>();
        Turns.Remove(element);
        for(var i = 0; i < Turns.Count; i++)
        {
            Turns[i].txtTurn.text = (i + 1).ToString();
            Turns[i].Turn = i + 1;
        }
    }

    public void Fight()
    {
        bool valid = true;

        foreach(var turn in Turns)
        {
            if (turn.AttackType == "Martial Arts" && ( turn.MagicalDefense == null || turn.PhysicalPower == null || turn.PhysicalDefense == null))
                valid = false;
            if (turn.AttackType == "Power" && (turn.MagicalDefense == null || turn.MagicalPower == null || turn.PhysicalDefense == null))
                valid = false;
        }

        if(valid)
        {
            List<csFightPlanElement> fightPlan = new List<csFightPlanElement>();
            foreach(var turn in Turns)
            {
                var fightPlanElem = new csFightPlanElement();
                fightPlanElem.AttackType = turn.AttackType;
                fightPlanElem.MagicalDefense = turn.MagicalDefense;
                fightPlanElem.MagicalPower = turn.MagicalPower;
                fightPlanElem.PhysicalDefense = turn.PhysicalDefense;
                fightPlanElem.PhysicalPower = turn.PhysicalPower;
                fightPlanElem.Turn = turn.Turn;
                fightPlan.Add(fightPlanElem);
            }
            csGameController.control.FightPlan = fightPlan;
        }
            
    }
}
