using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class csFightPlanPrefab : MonoBehaviour {

    public Text txtTurn;

    [SerializeField]
    private GameObject cmbAttackType;

    [SerializeField]
    private GameObject cmbAttacks;

    [SerializeField]
    private GameObject cmbPowers;

    [SerializeField]
    private GameObject cmbPhysicalDefense;

    [SerializeField]
    private GameObject cmbMagicalDefense;

    [SerializeField]
    private GameObject cmbItems;

    private Slider manaSlider;

    private Slider staminaSlider;

    private Text txtStamina;

    private Text txtMana;

    private Dropdown attackType;

    private Dropdown attacks;

    private Dropdown powers;

    private Dropdown physicalDefense;

    private Dropdown magicalDefense;

    private CanvasGroup attackCanvas;

    private CanvasGroup powerCanvas;

    private csMagicPower lastSelectedPower;

    private csPhysicalPower lastSelectedPhysical;
    public csPhysicalPower PhysicalPower { get; set; }
    public csPhysicalDefense PhysicalDefense { get; set; }
    public csMagicPower MagicalPower { get; set; }
    public csMagicDefense MagicalDefense { get; set; }
    public string AttackType { get; set; }

    public int Turn { get; set; }

	// Use this for initialization
	void Start () 
    {
        var objManaSlider = GameObject.Find("Mana Slider");
        manaSlider = objManaSlider.GetComponent<Slider>();
        var objStaminaSlider = GameObject.Find("Stamina Slider");
        staminaSlider = objStaminaSlider.GetComponent<Slider>();

        var objTxtMana = GameObject.Find("txtMana");
        txtMana = objTxtMana.GetComponent<Text>();
        var objTxtStamina = GameObject.Find("txtStamina");
        txtStamina = objTxtStamina.GetComponent<Text>();

        AttackType = "Power";

        attackType = cmbAttackType.GetComponent<Dropdown>();

        attackType.onValueChanged.AddListener(delegate
        {
            OnCmbAttackTypeChanged(attackType);
        });

        attacks = cmbAttacks.GetComponent<Dropdown>();
        attackCanvas = cmbAttacks.GetComponent<CanvasGroup>();
        var attackData = csGameController.control.Kagotchi.Attacks;
        foreach(var item in attackData)
        {
            Dropdown.OptionData cmbItem = new Dropdown.OptionData(item.Name);
            attacks.options.Add(cmbItem);
        }

        attacks.onValueChanged.AddListener(delegate
        {
            OnCmbAttacksChanged(attacks);
        });

        powers = cmbPowers.GetComponent<Dropdown>();
        powerCanvas = cmbPowers.GetComponent<CanvasGroup>();
        var powersData = csGameController.control.Kagotchi.Powers;
        foreach (var item in powersData)
        {
            Dropdown.OptionData cmbItem = new Dropdown.OptionData(item.Name);
            powers.options.Add(cmbItem);
        }

        powers.onValueChanged.AddListener(delegate
        {
            OnCmbPowersChanged(powers);
        });

        physicalDefense = cmbPhysicalDefense.GetComponent<Dropdown>();
        var physicalDefenseData = csGameController.control.Kagotchi.PhysicalDefense;
        foreach (var item in physicalDefenseData)
        {
            Dropdown.OptionData cmbItem = new Dropdown.OptionData(item.Name);
            physicalDefense.options.Add(cmbItem);
        }

        physicalDefense.onValueChanged.AddListener(delegate
        {
            OnCmbPhysicalDefenseChanged(physicalDefense);
        });

        magicalDefense = cmbMagicalDefense.GetComponent<Dropdown>();
        var magicalDefenseData = csGameController.control.Kagotchi.MagicalDefense;
        foreach (var item in magicalDefenseData)
        {
            Dropdown.OptionData cmbItem = new Dropdown.OptionData(item.Name);
            magicalDefense.options.Add(cmbItem);
        }

        magicalDefense.onValueChanged.AddListener(delegate
        {
            OnCmbMagicalDefenseChanged(magicalDefense);
        });

        /*var items = cmbItems.GetComponent<Dropdown>();
        var itemsData = csGameController.control.Items;
        foreach (var item in itemsData)
        {
            Dropdown.OptionData cmbItem = new Dropdown.OptionData(item.Name);
            items.options.Add(cmbItem);
        }*/

        attackCanvas.alpha = 0;
        attackCanvas.blocksRaycasts = false;
        attackCanvas.interactable = false;

        powerCanvas.alpha = 1;
        powerCanvas.blocksRaycasts = true;
        powerCanvas.interactable = true;
  
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCmbAttackTypeChanged(Dropdown dropdown)
    {
        var selectedOption = dropdown.options[dropdown.value];
        AttackType = selectedOption.text;

        if(selectedOption.text == "Martial Arts")
        {
            attackCanvas.alpha = 1;
            attackCanvas.blocksRaycasts = true;
            attackCanvas.interactable = true;

            powerCanvas.alpha = 0;
            powerCanvas.blocksRaycasts = false;
            powerCanvas.interactable = false;
        }
        else
        {
            attackCanvas.alpha = 0;
            attackCanvas.blocksRaycasts = false;
            attackCanvas.interactable = false;

            powerCanvas.alpha = 1;
            powerCanvas.blocksRaycasts = true;
            powerCanvas.interactable = true;
        }
    }

    public void OnCmbPowersChanged(Dropdown dropdown)
    {
        var selectedOption = dropdown.options[dropdown.value];
        MagicalPower = (csMagicPower)csGameController.control.Kagotchi.Powers.FirstOrDefault(i => i.Name == selectedOption.text);
        if (MagicalPower == null)
            manaSlider.value += lastSelectedPower.Mana;
        else
        {
            manaSlider.value -= MagicalPower.Mana;
            lastSelectedPower = MagicalPower;
        }
        txtMana.text = Mathf.Round(manaSlider.value).ToString() + "%";    
    }

    public void OnCmbMagicalDefenseChanged(Dropdown dropdown)
    {
        var selectedOption = dropdown.options[dropdown.value];
        MagicalDefense = csGameController.control.Kagotchi.MagicalDefense.FirstOrDefault(i => i.Name == selectedOption.text);
    }

    public void OnCmbAttacksChanged(Dropdown dropdown)
    {
        var selectedOption = dropdown.options[dropdown.value];
        PhysicalPower = (csPhysicalPower)csGameController.control.Kagotchi.Attacks.FirstOrDefault(i => i.Name == selectedOption.text);
        if (PhysicalPower == null)
            staminaSlider.value += lastSelectedPhysical.Stamina;
        else
        {
            staminaSlider.value -= PhysicalPower.Stamina;
            lastSelectedPhysical = PhysicalPower;
        }
        txtStamina.text = Mathf.Round(staminaSlider.value).ToString() + "%";
    }

    public void OnCmbPhysicalDefenseChanged(Dropdown dropdown)
    {
        var selectedOption = dropdown.options[dropdown.value];
        PhysicalDefense = csGameController.control.Kagotchi.PhysicalDefense.FirstOrDefault(i => i.Name == selectedOption.text);
    }
}
