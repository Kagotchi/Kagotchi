using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts;
using System.Linq;
using UnityEngine.UI;

public class csFightResumeSceneManager : MonoBehaviour {

    [SerializeField]
    private Image playerAvatar;
    [SerializeField]
    private Image botAvatar;
    [SerializeField]
    private Text txtPlayerName;
    [SerializeField]
    private Text txtBotName;
    [SerializeField]
    private Text txtPlayerResult;
    [SerializeField]
    private Text txtBotResult;
    [SerializeField]
    private Text txtPlayerHit;
    [SerializeField]
    private Text txtPlayerSta;
    [SerializeField]
    private Text txtPlayerMan;
    [SerializeField]
    private Text txtBotHit;
    [SerializeField]
    private Text txtBotSta;
    [SerializeField]
    private Text txtBotMan;
    [SerializeField]
    private Text txtFame;
    [SerializeField]
    private Text txtMoney;
    [SerializeField]
    private Text txtExperience;
    [SerializeField]
    private Text txtNewKnowledge;
    [SerializeField]
    private GameObject pnlCombatDetails;

    private csKagotchi kagotchi = null;
    private List<csFightPlanElement> kagotchiFightPlan = null;
    private List<csFightPlanElement> botFightPlan = new List<csFightPlanElement>();
    private List<csMagicDefense> knowledgeList = new List<csMagicDefense>();
    private csBot bot = null;
    private const float DEFENSE_FACTOR = 0.1f;
    private const int ENEMY_EXP_FACTOR = 100;
    private const int ENEMY_MONEY_FACTOR = 100;
    private const int FAME_FACTOR = 100;
    private const int MAX_ATTACKS_THRESHOLD = 2;
    private const float REPEAT_ATTACK_PERCENTAGE = 10.0f;
    private int FightTurn { get; set; }
    private List<csTurn> TurnList { get; set; }
    public string Winner { get; set; }
    public string Loser { get; set; }
    private float cumulativeRepatedAttackPerc = 0;
    private float minNeededMana = float.PositiveInfinity;
    private float minNeededStamina = float.PositiveInfinity;

	// Use this for initialization
	void Start () 
    {
        FightTurn = 1;
        TurnList = new List<csTurn>();
        kagotchi = csGameController.control.Kagotchi;
        bot = csGameController.control.CurrentBot;
        Fight();
        txtPlayerName.text = kagotchi.Name;
        txtBotName.text = bot.Name;
        playerAvatar.sprite = csGameController.control.Kagotchi.Avatar;
        botAvatar.sprite = csGameController.control.CurrentBot.Avatar;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateBotPlan()
    {
        int turn = 1;
        var tempStamina = bot.Stamina;
        var tempMana = bot.Mana;
        csAttackType attackType = csAttackType.Power;

        while (tempStamina > 0 || tempMana > 0)
        {
            Array values = Enum.GetValues(typeof(csAttackType));
            if(tempStamina > 0 && tempMana > 0)
                attackType = (csAttackType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
            else if(tempStamina > 0 && tempMana <= 0)
                attackType = csAttackType.MartialArts;
            else if(tempStamina <= 0 && tempMana > 0)
                attackType = csAttackType.Power;

            var planElement = new csFightPlanElement();
            planElement.Turn = turn;

            if (attackType == csAttackType.Power)
            {
                var powerIdx = UnityEngine.Random.Range(0, bot.Powers.Count - 1);
                planElement.MagicalPower = (csMagicPower)bot.Powers[powerIdx];
                tempMana -= Mathf.RoundToInt(planElement.MagicalPower.Mana);
                if (tempMana < 0)
                    tempMana = 0;
            }

            if (attackType == csAttackType.MartialArts)
            {
                var physicalPowerIdx = UnityEngine.Random.Range(0, bot.Attacks.Count - 1);
                planElement.PhysicalPower = (csPhysicalPower)bot.Attacks[physicalPowerIdx];
                tempStamina -= Mathf.RoundToInt(planElement.PhysicalPower.Stamina);
                if (tempStamina < 0)
                    tempStamina = 0;
            }

            var magicalDefenceIdx = UnityEngine.Random.Range(0, bot.MagicalDefense.Count - 1);
            planElement.MagicalDefense = bot.MagicalDefense[magicalDefenceIdx];
            var physicalDefenceIdx = UnityEngine.Random.Range(0, bot.PhysicalDefense.Count - 1);
            planElement.PhysicalDefense = bot.PhysicalDefense[physicalDefenceIdx];

            botFightPlan.Add(planElement);

            turn++;
        }
    }

    private csDefenseData ChooseDefense(csIFighter attacker, csIFighter defender, csIAttack attack)
    {
        csIDefense defense = null;
        float defenseValue = 0;
        float attackValue = 0;
        bool canDefende = false;
        csDefenseData data = new csDefenseData();

        if (attack is csPhysicalPower)
        {
            defense = defender.PhysicalDefense.First(x => x.PhysicalDefenseType == attack.AttackType);
            canDefende = defender.Stamina > 0;
        }
        else
        {
            defense = defender.AllMagicalDefenses.First(x => x.MagicalDefenseType == attack.Core);
            canDefende = defender.Mana > 0;
        }

        data.Defense = defense;

        if (defense != null && canDefende)
        {
            defenseValue = defense.Value + defender.Dexterity + UnityEngine.Random.Range(0, 20);
            attackValue = attack.Value + attacker.Dexterity + UnityEngine.Random.Range(0, 20);
            if (attackValue > defenseValue)
            {
                data.Blocked = false;
                data.Value = 0;
            }
            else
            {
                //blocked attack
                data.Value = attack.AttackValue * defense.Value; //TODO: add defender armor 20%(max)
                data.Blocked = true;
                data.Defense.Increase();
            }
        }

        return data;
    }

    private csIAttack ChooseAttack(csIFighter attacker)
    {
        float totalPowersValue = 0;
        float probabilityRange = 0;
        string lastAttackName = String.Empty;

        List<csIAttack> attacks = new List<csIAttack>();

        foreach(csIAttack attack in attacker.Attacks)
        {
            if (attack.Stamina <= attacker.Stamina)
            {
                attacks.Add(attack);
                if (minNeededStamina > attack.Stamina)
                    minNeededStamina = attack.Stamina;
            }
                
        }
        foreach (csIAttack attack in attacker.Powers)
        {
            if (attack.Mana <= attacker.Mana)
            {
                attacks.Add(attack);
                if (minNeededMana > attack.Mana)
                    minNeededMana = attack.Mana;
            }
                
        }

        List<csAttackProbabilityData> probabilityList = new List<csAttackProbabilityData>();

        try
        {
            if (attacker.CurrentCombat.Count >= 2 && attacker.CurrentCombat.Count % MAX_ATTACKS_THRESHOLD == 0)
            {
                if (attacker.CurrentCombat[attacker.CurrentCombat.Count - 1].Name == attacker.CurrentCombat[attacker.CurrentCombat.Count - 2].Name)
                {
                    cumulativeRepatedAttackPerc += REPEAT_ATTACK_PERCENTAGE;
                    lastAttackName = attacker.CurrentCombat[attacker.CurrentCombat.Count - 1].Name;
                }
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        

        foreach (var attack in attacks)
        {
            totalPowersValue += attack.RawPower;
        }

        if(lastAttackName != String.Empty)
        {

            csIAttack repeatedAttack = attacks.FirstOrDefault(n => n.Name == lastAttackName);

            attacks = attacks.Where(n => n.Name != lastAttackName).ToList();

            float extraPercentage = (cumulativeRepatedAttackPerc * repeatedAttack.RawPower) / totalPowersValue;

            var probabilityDataRA = new csAttackProbabilityData();
            probabilityDataRA.Probability = ((repeatedAttack.RawPower * 100) / totalPowersValue) - extraPercentage;
            probabilityDataRA.Attack = repeatedAttack;
            probabilityList.Add(probabilityDataRA);

            foreach (var attack in attacks)
            {
                var probabilityData = new csAttackProbabilityData();
                probabilityData.Probability = ((attack.RawPower * 100) / totalPowersValue) + (extraPercentage / attacks.Count);
                probabilityData.Attack = attack;
                probabilityList.Add(probabilityData);
            }
        }
        else
        {
            foreach (var attack in attacks)
            {
                var probabilityData = new csAttackProbabilityData();
                probabilityData.Probability = (attack.RawPower * 100) / totalPowersValue;
                probabilityData.Attack = attack;
                probabilityList.Add(probabilityData);
            }
        }

        probabilityList = probabilityList.OrderByDescending(x => x.Probability).ToList();

        foreach(var probability in probabilityList)
        {
            probability.Min = probabilityRange;
            probability.Max = probabilityRange + probability.Probability;
            probabilityRange += probability.Probability;
        }

        var randValue = UnityEngine.Random.Range(0, 100);

        foreach (var probability in probabilityList)
        {
            if (randValue >= probability.Min && randValue < probability.Max)
            {
                attacker.CurrentCombat.Add(probability.Attack);
                return probability.Attack;
            }
                
        }

        return null;
    }

    private csTurn ProcessAttack(csIFighter attacker, csIFighter defender, int idx, csTurn turn)
    {
        float damage = 0.0f;

        var attack = ChooseAttack(attacker);

        if (attack is csMagicPower)
        {
            attack = (csMagicPower)attack;
            var meditation = attacker.PassivePower.First(s => s.Name == "Meditation");
            if ((attacker.Mana < minNeededMana || attacker.Mana < attack.Mana) && (attacker.Stamina < minNeededStamina || attacker.Stamina < attack.Stamina))
                meditation.IsInUse = true;

            if (meditation.IsInUse == true)
            {
                var meditationValue = meditation.Use();
                if (meditationValue < 1.0f)
                    meditationValue = 1.0f;

                turn.Description += attacker.Name + " used " + meditation.Name + System.Environment.NewLine;

                if(meditation.Increase())
                {
                    turn.Description += attacker.Name + "'s " + meditation.Name + " increased by " + meditation.Factor * 100.0f + " it's now " + meditation.Value * 100.0f + System.Environment.NewLine;
                }

                attacker.Mana += Mathf.RoundToInt(meditationValue);

                if (attacker.Mana > attacker.Intelligence)
                    attacker.Mana = attacker.Intelligence;

                if (attacker.Mana >= attacker.Intelligence)
                    meditation.IsInUse = false;

                if (attacker is csKagotchi)
                {
                    idx++;

                    try
                    {
                        var kagotchiStats = new csFighterStatsText();
                        kagotchiStats.Hitpoints = attacker.Hitpoints.ToString();
                        kagotchiStats.Stamina = attacker.Stamina.ToString();
                        kagotchiStats.Mana = attacker.Mana.ToString();

                        var botStats = new csFighterStatsText();
                        botStats.Hitpoints = defender.Hitpoints.ToString();
                        botStats.Stamina = defender.Stamina.ToString();
                        botStats.Mana = defender.Mana.ToString();

                        turn.Kagotchi = kagotchiStats;
                        turn.Bot = botStats;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else if (attacker is csBot)
                {
                    idx++;

                    try
                    { 

                        var kagotchiStats = new csFighterStatsText();
                        kagotchiStats.Hitpoints = defender.Hitpoints.ToString();
                        kagotchiStats.Stamina = defender.Stamina.ToString();
                        kagotchiStats.Mana = defender.Mana.ToString();

                        var botStats = new csFighterStatsText();
                        botStats.Hitpoints = attacker.Hitpoints.ToString();
                        botStats.Stamina = attacker.Stamina.ToString();
                        botStats.Mana = attacker.Mana.ToString();

                        turn.Kagotchi = kagotchiStats;
                        turn.Bot = botStats;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                turn.Idx = idx;
                return turn;
            }

            foreach (var weakness in defender.Weaknesses)
            {
                if (attack.Core == weakness.Weakness)
                {
                    attack.IsStrongAgainst = true;
                    attack.WeaknessMultiplier = weakness.Multiplier;
                    turn.Description += defender.Name + " has a weakness against " + attack.Name + " x" + weakness.Multiplier + System.Environment.NewLine;
                }
            }

            if(attacker.Mana >= attack.Mana)
            {
                attack.CastSpell();
                csDefenseData defenseData = ChooseDefense(attacker, defender, attack);
                damage = attack.AttackValue - (defenseData.Value < 1 && defenseData.Blocked ? 1 : defenseData.Value);
                if (defenseData.Blocked)
                {
                    turn.Description += attacker.Name + " used " + attack.Name + " and only gave " + damage.ToString() + " damage points." + System.Environment.NewLine;
                    turn.Description += defender.Name + " blocked your " + attack.Name + " attack." + System.Environment.NewLine;
                }
                else
                {
                    turn.Description += attacker.Name + " used " + attack.Name + " and gave " + damage.ToString() + " damage points." + System.Environment.NewLine;

                    if(defender is csKagotchi)
                    {
                        if (!defender.MagicalDefense.Any(s => s.MagicalDefenseType == attack.Core) && !knowledgeList.Any(s => s.MagicalDefenseType == attack.Core))
                        {
                            var defense = defender.AllMagicalDefenses.FirstOrDefault(s => s.MagicalDefenseType == attack.Core);
                            knowledgeList.Add(defense);
                            txtNewKnowledge.text += "-" + defense.Name + " Level:" + defense.Level + System.Environment.NewLine;
                        }
                        else
                        {
                            var defense = defender.MagicalDefense.FirstOrDefault(s => s.MagicalDefenseType == attack.Core);
                            if(defense != null && defense.Level < attack.Level)
                            {
                                defense.Level++;
                                knowledgeList.Add(defense);
                                txtNewKnowledge.text += "-" + defense.Name + " Level:" + defense.Level + System.Environment.NewLine;
                            }
                            var defenseKnowledge = knowledgeList.FirstOrDefault(s => s.MagicalDefenseType == attack.Core);
                            if (defenseKnowledge != null && defenseKnowledge.Level < attack.Level)
                            {
                                defense.Level++;
                                knowledgeList.Add(defense);
                                txtNewKnowledge.text += "-" + defense.Name + " Level:" + defense.Level + System.Environment.NewLine;
                            }
                        }
                    }
                }


                if (attack.Increase())
                {
                    turn.Description += attacker.Name + "'s " + attack.Name + " increased by " + attack.Factor * 100.0f + " it's now " + attack.Value * 100.0f + System.Environment.NewLine;
                }

                defender.Hitpoints -= Mathf.RoundToInt(damage);
                if (defender.Hitpoints < 0)
                    defender.Hitpoints = 0;
                attacker.Mana -= Mathf.RoundToInt(attack.Mana);
            }

            if (attacker is csKagotchi)
            {

                idx++;

                var kagotchiStats = new csFighterStatsText();
                kagotchiStats.Hitpoints = attacker.Hitpoints.ToString();
                kagotchiStats.Stamina = attacker.Stamina.ToString();
                kagotchiStats.Mana = attacker.Mana.ToString();

                var botStats = new csFighterStatsText();
                botStats.Hitpoints = defender.Hitpoints.ToString();
                botStats.Stamina = defender.Stamina.ToString();
                botStats.Mana = defender.Mana.ToString();

                turn.Kagotchi = kagotchiStats;
                turn.Bot = botStats;
            }
            else if (attacker is csBot)
            {

                idx++;

                var kagotchiStats = new csFighterStatsText();
                kagotchiStats.Hitpoints = defender.Hitpoints.ToString();
                kagotchiStats.Stamina = defender.Stamina.ToString();
                kagotchiStats.Mana = defender.Mana.ToString();

                var botStats = new csFighterStatsText();
                botStats.Hitpoints = attacker.Hitpoints.ToString();
                botStats.Stamina = attacker.Stamina.ToString();
                botStats.Mana = attacker.Mana.ToString();

                turn.Kagotchi = kagotchiStats;
                turn.Bot = botStats;
            }
        }

        if (attack is csPhysicalPower)
        {
            var meditation = attacker.PassivePower.First(s => s.Name == "Meditation");
            if ((attacker.Mana < minNeededMana || attacker.Mana < attack.Mana) && (attacker.Stamina < minNeededStamina || attacker.Stamina < attack.Stamina))
                meditation.IsInUse = true;

            if (meditation.IsInUse == true)
            {
                var meditationValue = meditation.Use();
                if (meditationValue < 1.0f)
                    meditationValue = 1.0f;

                turn.Description += attacker.Name + " used " + meditation.Name + System.Environment.NewLine;

                if (meditation.Increase())
                {
                    turn.Description += attacker.Name + "'s " + meditation.Name + " increased by " + meditation.Factor * 100.0f + " it's now " + meditation.Value * 100.0f + System.Environment.NewLine;
                }

                attacker.Stamina += Mathf.RoundToInt(meditationValue);

                if (attacker.Stamina > attacker.Dexterity)
                    attacker.Stamina = attacker.Dexterity;

                if (attacker.Stamina >= attacker.Dexterity)
                    meditation.IsInUse = false;

                if(attacker is csKagotchi)
                {
                    idx++;

                    var kagotchiStats = new csFighterStatsText();
                    kagotchiStats.Hitpoints = attacker.Hitpoints.ToString();
                    kagotchiStats.Stamina = attacker.Stamina.ToString();
                    kagotchiStats.Mana = attacker.Mana.ToString();

                    var botStats = new csFighterStatsText();
                    botStats.Hitpoints = defender.Hitpoints.ToString();
                    botStats.Stamina = defender.Stamina.ToString();
                    botStats.Mana = defender.Mana.ToString();

                    turn.Kagotchi = kagotchiStats;
                    turn.Bot = botStats;
                }
                else if(attacker is csBot)
                {

                    idx++;

                    var kagotchiStats = new csFighterStatsText();
                    kagotchiStats.Hitpoints = defender.Hitpoints.ToString();
                    kagotchiStats.Stamina = defender.Stamina.ToString();
                    kagotchiStats.Mana = defender.Mana.ToString();

                    var botStats = new csFighterStatsText();
                    botStats.Hitpoints = attacker.Hitpoints.ToString();
                    botStats.Stamina = attacker.Stamina.ToString();
                    botStats.Mana = attacker.Mana.ToString();

                    turn.Kagotchi = kagotchiStats;
                    turn.Bot = botStats;
                }

                turn.Idx = idx;
                return turn;
            }

            attack.Attack();

            if (attacker.Stamina >= attack.Stamina)
            {
                csDefenseData defenseData = ChooseDefense(attacker, defender, attack);
                damage = attack.AttackValue - (defenseData.Value < 1 && defenseData.Blocked ? 1 : defenseData.Value);
                if (defenseData.Blocked)
                {
                    turn.Description += attacker.Name + " used " + attack.Name + " and only gave " + damage.ToString() + " damage points." + System.Environment.NewLine;
                    turn.Description += defender.Name + " blocked the " + attack.Name + " attack." + System.Environment.NewLine;
                }
                else
                {
                    turn.Description += attacker.Name + " used " + attack.Name + " and gave " + damage.ToString() + " damage points." + System.Environment.NewLine;
                }

                if (attack.Increase())
                {
                    turn.Description += attacker.Name + "'s " + attack.Name + " increased by " + attack.Factor * 100.0f + " it's now " + attack.Value * 100.0f + System.Environment.NewLine;
                }

                defender.Hitpoints -= Mathf.RoundToInt(damage);
                if (defender.Hitpoints < 0)
                    defender.Hitpoints = 0;
                attacker.Stamina -= Mathf.RoundToInt(attack.Stamina);
            }

            if(attacker is csKagotchi)
            {
                idx++;

                var kagotchiStats = new csFighterStatsText();
                kagotchiStats.Hitpoints = attacker.Hitpoints.ToString();
                kagotchiStats.Stamina = attacker.Stamina.ToString();
                kagotchiStats.Mana = attacker.Mana.ToString();

                var botStats = new csFighterStatsText();
                botStats.Hitpoints = defender.Hitpoints.ToString();
                botStats.Stamina = defender.Stamina.ToString();
                botStats.Mana = defender.Mana.ToString();

                turn.Kagotchi = kagotchiStats;
                turn.Bot = botStats;
            }
            else if(attacker is csBot)
            {

                idx++;

                var kagotchiStats = new csFighterStatsText();
                kagotchiStats.Hitpoints = defender.Hitpoints.ToString();
                kagotchiStats.Stamina = defender.Stamina.ToString();
                kagotchiStats.Mana = defender.Mana.ToString();

                var botStats = new csFighterStatsText();
                botStats.Hitpoints = attacker.Hitpoints.ToString();
                botStats.Stamina = attacker.Stamina.ToString();
                botStats.Mana = attacker.Mana.ToString();

                turn.Kagotchi = kagotchiStats;
                turn.Bot = botStats;
            }
            
        }

        if (attack == null)
        {
            var meditation = attacker.PassivePower.First(s => s.Name == "Meditation");
            if ((attacker.Mana < minNeededMana) && (attacker.Stamina < minNeededStamina))
                meditation.IsInUse = true;

            if (meditation.IsInUse == true)
            {
                var meditationValue = meditation.Use();
                if (meditationValue < 1.0f)
                    meditationValue = 1.0f;

                turn.Description += attacker.Name + " used " + meditation.Name + System.Environment.NewLine;

                if (meditation.Increase())
                {
                    turn.Description += attacker.Name + "'s " + meditation.Name + " increased by " + meditation.Factor * 100.0f + " it's now " + meditation.Value * 100.0f + System.Environment.NewLine;
                }
            }

            if (attacker is csKagotchi)
            {
                idx++;

                var kagotchiStats = new csFighterStatsText();
                kagotchiStats.Hitpoints = attacker.Hitpoints.ToString();
                kagotchiStats.Stamina = attacker.Stamina.ToString();
                kagotchiStats.Mana = attacker.Mana.ToString();

                var botStats = new csFighterStatsText();
                botStats.Hitpoints = defender.Hitpoints.ToString();
                botStats.Stamina = defender.Stamina.ToString();
                botStats.Mana = defender.Mana.ToString();

                turn.Kagotchi = kagotchiStats;
                turn.Bot = botStats;
            }
            else if (attacker is csBot)
            {

                idx++;

                var kagotchiStats = new csFighterStatsText();
                kagotchiStats.Hitpoints = defender.Hitpoints.ToString();
                kagotchiStats.Stamina = defender.Stamina.ToString();
                kagotchiStats.Mana = defender.Mana.ToString();

                var botStats = new csFighterStatsText();
                botStats.Hitpoints = attacker.Hitpoints.ToString();
                botStats.Stamina = attacker.Stamina.ToString();
                botStats.Mana = attacker.Mana.ToString();

                turn.Kagotchi = kagotchiStats;
                turn.Bot = botStats;
            }
        }

        turn.Idx = idx;
        return turn;
    }

    public void Fight()
    {
        int botIdx = 0;
        int kagotchiIdx = 0;

        while(kagotchi.Hitpoints > 0 && bot.Hitpoints > 0)
        {
            var attackTurn = new csTurn();
            attackTurn.Turn = FightTurn;

            //Kagotchi turn
            attackTurn = ProcessAttack(kagotchi, bot, kagotchiIdx, attackTurn);

            kagotchiIdx = attackTurn.Idx;
            //Bot turn
            attackTurn = ProcessAttack(bot, kagotchi, botIdx, attackTurn);

            botIdx = attackTurn.Idx;

            TurnList.Add(attackTurn);

            FightTurn++;
            
        }

        if(kagotchi.Hitpoints > 0 && bot.Hitpoints <= 0)
        {
            Winner = kagotchi.Name;
            Loser = bot.Name;
            txtPlayerResult.text = "WINNER";
            txtPlayerResult.color = new Color(50.0f / 255, 167.0f / 255, 50.0f / 255);
            txtBotResult.text = "LOSER";
            txtBotResult.color = new Color(238.0f / 255, 50.0f / 255, 52.0f / 255);
            kagotchi.Experience += bot.Level * ENEMY_EXP_FACTOR;
            kagotchi.Fame += bot.Level * FAME_FACTOR;
            kagotchi.Money += bot.Level * ENEMY_MONEY_FACTOR;
            txtFame.text = kagotchi.Fame.ToString();
            txtExperience.text = kagotchi.Experience.ToString();
            txtMoney.text = kagotchi.Money.ToString();
            kagotchi.Wins += 1;
            kagotchi.CurrentCombat.Clear();
            bot.CurrentCombat.Clear();
        }
        else if (kagotchi.Hitpoints <= 0 && bot.Hitpoints > 0)
        {
            Winner = bot.Name;
            Loser = kagotchi.Name;
            txtPlayerResult.text = "LOSER";
            txtPlayerResult.color = new Color(238.0f / 255, 50.0f / 255, 52.0f / 255);
            txtBotResult.text = "WINNER" ;
            txtBotResult.color = new Color(50.0f / 255, 167.0f / 255, 50.0f / 255);
            kagotchi.Fame -= bot.Level * FAME_FACTOR / 2;
            txtFame.text = kagotchi.Fame.ToString();
            kagotchi.Losses += 1;
            kagotchi.CurrentCombat.Clear();
            bot.CurrentCombat.Clear();
        }
        else if (kagotchi.Hitpoints <= 0 && bot.Hitpoints <= 0)
        {
            Winner = kagotchi.Name;
            Loser = bot.Name;
            txtPlayerResult.text = "DRAW";
            txtPlayerResult.color = new Color(188.0f / 255, 183.0f / 255, 19.0f / 255);
            txtBotResult.text = "DRAW";
            txtBotResult.color = new Color(188.0f / 255, 183.0f / 255, 19.0f / 255);
            kagotchi.Experience += bot.Level * ENEMY_EXP_FACTOR / 2;
            kagotchi.Fame += bot.Level * FAME_FACTOR / 2;
            kagotchi.Money += bot.Level * ENEMY_MONEY_FACTOR / 2;
            txtFame.text = kagotchi.Fame.ToString();
            txtExperience.text = kagotchi.Experience.ToString();
            txtMoney.text = kagotchi.Money.ToString();
            kagotchi.Draws += 1;
            kagotchi.CurrentCombat.Clear();
            bot.CurrentCombat.Clear();
        }

        kagotchi.Knowledge.AddRange(knowledgeList);

        txtPlayerHit.text = TurnList[TurnList.Count - 1].Kagotchi.Hitpoints;
        txtPlayerSta.text = TurnList[TurnList.Count - 1].Kagotchi.Stamina;
        txtPlayerMan.text = TurnList[TurnList.Count - 1].Kagotchi.Mana;
        txtBotHit.text = TurnList[TurnList.Count - 1].Bot.Hitpoints;
        txtBotSta.text = TurnList[TurnList.Count - 1].Bot.Stamina;
        txtBotMan.text = TurnList[TurnList.Count - 1].Bot.Mana;

        var turnGrid = GameObject.Find("TurnGrid");
        GameObject prefab = (GameObject)Resources.Load("Prefabs/UI/Turn");
        if (prefab != null)
        {
            foreach(var turn in TurnList)
            {
                var clone = (GameObject)Instantiate(prefab);
                clone.name = prefab.name;
                var turnPrefab = clone.GetComponent<csTurnPrefab>();
                turnPrefab.Turn = turn.Turn.ToString();
                turnPrefab.Description = turn.Description;
                turnPrefab.PlayerHits = turn.Kagotchi.Hitpoints;
                turnPrefab.PlayerMana = turn.Kagotchi.Mana;
                turnPrefab.PlayerStamina = turn.Kagotchi.Stamina;
                turnPrefab.BotHits = turn.Bot.Hitpoints;
                turnPrefab.BotMana = turn.Bot.Mana;
                turnPrefab.BotStamina = turn.Bot.Stamina;

                clone.transform.SetParent(turnGrid.transform, false);
            }
        }
    }

    public void ShowCombatDetails()
    {
        pnlCombatDetails.GetComponent<CanvasGroup>().alpha = 1;
        pnlCombatDetails.GetComponent<CanvasGroup>().blocksRaycasts = true;
        pnlCombatDetails.GetComponent<CanvasGroup>().interactable = true;
    }

    public void HideCombatDetails()
    {
        pnlCombatDetails.GetComponent<CanvasGroup>().alpha = 0;
        pnlCombatDetails.GetComponent<CanvasGroup>().blocksRaycasts = false;
        pnlCombatDetails.GetComponent<CanvasGroup>().interactable = false;
    }
}
