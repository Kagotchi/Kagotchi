using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class csTurnPrefab : MonoBehaviour {

    [SerializeField]
    private Text txtTurn;
    [SerializeField]
    private Text txtDescription;
    [SerializeField]
    private Text txtPlayerHits;
    [SerializeField]
    private Text txtPlayerStamina;
    [SerializeField]
    private Text txtPlayerMana;
    [SerializeField]
    private Text txtBotHits;
    [SerializeField]
    private Text txtBotStamina;
    [SerializeField]
    private Text txtBotMana;

    public string Turn { get; set; }
    public string Description { get; set; }
    public string PlayerHits { get; set; }
    public string PlayerStamina { get; set; }
    public string PlayerMana { get; set; }
    public string BotHits { get; set; }
    public string BotStamina { get; set; }
    public string BotMana { get; set; }

    // Use this for initialization
	void Start () 
    {
        txtTurn.text = Turn;
        txtDescription.text = Description;
        txtPlayerHits.text = PlayerHits;
        txtPlayerStamina.text = PlayerStamina;
        txtPlayerMana.text = PlayerMana;
        txtBotHits.text = BotHits;
        txtBotStamina.text = BotStamina;
        txtBotMana.text = BotMana;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
