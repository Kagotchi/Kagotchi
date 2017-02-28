using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Actor;
using System;
using System.Linq;

public class csKagotchi : MonoBehaviour, csIFighter
{
    #region Variables
    [SerializeField]
    private Sprite avatar;

    public Slider healthSlider;
    public Slider energySlider;
    public Slider foodSlider;
    public Slider happinessSlider;

    public Text txtHealth;
    public Text txtEnergy;
    public Text txtFood;
    public Text txtHappiness;

    private int levelFactor = 10;
    private int foodFactor = 3600;
    private int happinessExtraFactor = 300;
    private int happinessFactor = 36000;
    private int energyFactor = 3600;
    private int healthFactor = 3600;

    private float overallFactor = 0.01f;

    private Dictionary<csSkillEnum, csSkill> skillsDict = new Dictionary<csSkillEnum, csSkill>();

    private csMessageManager message;

    #endregion

    #region Properties
    public Sprite Avatar { get; set; }
    public int MaxLevel { get; set; }
    public int Experience { get; set; }
    public float Health { get; set; }
    public float OverallHealth { get; set; }
    public float MaxHealth { get; set; }
    public int Generation { get; set; }
    public int Breeds { get; set; }
    public int MaxBreeds { get; set; }
    public int Fame { get; set; }
    public int Deceases { get; set; }
    public int Resistance { get; set; }
    public int Rarity { get; set; }
    public float Happiness { get; set; }
    public float OverallHappiness { get; set; }
    public float MaxHappiness { get; set; }
    public float Energy { get; set; }
    public float OverallEnergy { get; set; }
    public float MaxEnergy { get; set; }
    public float Food { get; set; }
    public float OverallFood { get; set; }
    public float MaxFood { get; set; }
    public csLifeStagesEnum CurrentStage { get; set; }
    public float Money { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public int Level { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Hitpoints { get; set; }
    public int Stamina { get; set; }
    public int Mana { get; set; }
    public string Name { get; set; }
    public bool IsAwake { get; set; }
    public List<csWeakness> Weaknesses { get; set; }
    public List<csWeakness> AllWeaknesses { get; set; }
    public List<csIAttack> Powers { get; set; }
    public List<csIAttack> AllPowers { get; set; }
    public List<csIAttack> Attacks { get; set; }
    public List<csPhysicalDefense> PhysicalDefense { get; set; }
    public List<csMagicDefense> MagicalDefense { get; set; }
    public List<csMagicDefense> AllMagicalDefenses { get; set; }
    public List<csPassivePower> PassivePower { get; set; }
    public List<csMagicDefense> Knowledge { get; set; }
    public List<csIAttack> CurrentCombat { get; set; }

    #endregion

    // Use this for initialization
    void Start() 
    {
        LoadValues();

        CancelInvoke("FoodManagement");
        InvokeRepeating("FoodManagement", 0, 1.0f);
        CancelInvoke("HappinessManagment");
        InvokeRepeating("HappinessManagment", 0, 1.0f);
        CancelInvoke("EnergyManagment");
        InvokeRepeating("EnergyManagment", 0, 1.0f);
        CancelInvoke("HealthManagment");
        InvokeRepeating("HealthManagment", 0, 1.0f);

        IsAwake= true;
	}
	
    private void FoodManagement()
    {
        Food -= (MaxFood / (foodFactor * (int)CurrentStage));

        if (Food < 0)
        {
            Food = 0;
            Happiness -= MaxHappiness / (happinessExtraFactor * (int)CurrentStage);
            Health -= MaxHealth / (healthFactor * (int)CurrentStage);
        }

        if (Food > MaxFood)
            Food = MaxFood;

        if (foodSlider != null)
        {
            foodSlider.value = Food;
            txtFood.text = Mathf.Round(Food).ToString() + "%";
        }
    }

    private void HappinessManagment()
    {
        Happiness -= (MaxHappiness / (happinessFactor * (int)CurrentStage));

        if (Happiness < 0)
        {
            Happiness = 0;
            OverallHappiness -= overallFactor;
        }
            

        if (Happiness > MaxHappiness)
            Happiness = MaxHappiness;

        if (happinessSlider != null)
        {
            txtHappiness.text = Mathf.Round(Happiness).ToString() + "%";
            happinessSlider.value = Happiness;
        }
    }

    private void EnergyManagment()
    {
        if(IsAwake)
            Energy -= (MaxEnergy / (energyFactor * (8 + (int)CurrentStage - 1)));
        else
        {
            Energy += (MaxEnergy / (energyFactor * (8 + (int)CurrentStage - 1)));
            if(Energy > 20.0f)
                Happiness -= MaxHappiness / (happinessExtraFactor * (int)CurrentStage);
        }
            

        if (Energy < 0)
        {
            Energy = 0;
            Happiness -= MaxHappiness / (happinessExtraFactor * (int)CurrentStage);
            Health -= MaxHealth / (healthFactor * (int)CurrentStage);
        }

        if (Energy > MaxEnergy)
        {
            Happiness -= MaxHappiness / (happinessExtraFactor * (int)CurrentStage);
            Energy = MaxEnergy;
        }
            

        if (energySlider)
        {
            txtEnergy.text = Mathf.Round(Energy).ToString() + "%";
            energySlider.value = Energy;
        }
    }

    private void HealthManagment()
    {
        if (Health < 0)
            Health = 0;

        if (Health > MaxHealth)
            Health = MaxHealth;

        if (healthSlider != null)
        {
            txtHealth.text = Mathf.Round(Health).ToString() + "%";
            healthSlider.value = Health;
        }
    }

    public csSkill GetSkill(csSkillEnum skillEnum)
    {
        csSkill skill;
        skillsDict.TryGetValue(skillEnum, out skill);
        return skill;
    }

    public void IncreaseSkill(csSkillEnum skillEnum)
    {
        var skill = GetSkill(skillEnum);

        var goal = skill.Max - skill.Value;

        var rand = UnityEngine.Random.Range(skill.Min, skill.Max);

        if (rand <= goal)
            skill.Value += skill.Factor;


        var msg = new csMessage()
        {
            Enable = true,
            Message = "The " + skill.Name + " skill increased " + skill.Factor + "% it's now " + skill.Value + "%!",
            Status = csMessageStatusEnum.Visible,
            Timeout = 3.0f,
            Type = csMessageTypeEnum.Success
        };
        message.SetUIMessage(msg);
    }

    public void Init()
    {
        if (csGameController.control.LastTime == DateTime.MinValue)
        {
            Name = "Asghard";
            Level = 1;
            MaxLevel = Level * levelFactor;
            Avatar = avatar;
            IsAwake = true;

            Experience = 0;

            Strength = 100;
            Dexterity = 100;
            Intelligence = 100;

            Hitpoints = Strength;
            Stamina = Dexterity;
            Mana = Intelligence;

            MaxHealth = 100;
            MaxHappiness = 100;
            MaxEnergy = 100;
            MaxFood = 100;

            Health = MaxHealth;
            OverallHealth = MaxHealth;
            Happiness = MaxHappiness;
            OverallHappiness = MaxHappiness;
            Energy = MaxEnergy;
            OverallEnergy = MaxEnergy;
            Food = MaxFood;
            OverallFood = MaxFood;

            Generation = 1;
            Breeds = 0;
            MaxBreeds = 3;
            Fame = 0;
            Deceases = 0;
            Resistance = 20;
            Rarity = 1;

            CurrentStage = csLifeStagesEnum.Baby;

            csSkill cooking = new csSkill()
            {
                Name = "Cooking",
                Max = 100.0f,
                Min = 0.0f,
                Value = 0.0f,
                Factor = 0.01f
            };

            if(!skillsDict.ContainsKey(csSkillEnum.Cooking))
                skillsDict.Add(csSkillEnum.Cooking, cooking);

            csSkill medicine = new csSkill()
            {
                Name = "Medicine",
                Max = 100.0f,
                Min = 0.0f,
                Value = 0.0f,
                Factor = 0.01f
            };

            if (!skillsDict.ContainsKey(csSkillEnum.Medicine))
                skillsDict.Add(csSkillEnum.Medicine, medicine);

            csSkill science = new csSkill()
            {
                Name = "Science",
                Max = 100.0f,
                Min = 0.0f,
                Value = 0.0f,
                Factor = 0.01f
            };

            if (!skillsDict.ContainsKey(csSkillEnum.Science))
                skillsDict.Add(csSkillEnum.Science, science);

            csSkill tailoring = new csSkill()
            {
                Name = "Tailoring",
                Max = 100.0f,
                Min = 0.0f,
                Value = 0.0f,
                Factor = 0.01f
            };

            if (!skillsDict.ContainsKey(csSkillEnum.Tailoring))
                skillsDict.Add(csSkillEnum.Tailoring, tailoring);

            message = GameObject.FindObjectOfType<csMessageManager>();

            InitPowers();
            InitWeaknesses();
            InitMagicalDefences();
            InitPhysicalAttacks();
            InitPhysicalDefences();

            Weaknesses = new List<csWeakness>();
            Weaknesses.Add(GetRandomWeakness());
            var weakness = GetRandomWeakness();
            if(Weaknesses.Count > 0)
            {
                var result = Weaknesses.FirstOrDefault(s => s.Weakness == weakness.Weakness);
                if (result != null)
                {
                    Weaknesses.Remove(result);
                    weakness.Multiplier++;
                }
            }
            Weaknesses.Add(weakness);

            Powers = new List<csIAttack>();
            var power = GetRandomPower();
            Powers.Add(power);

            MagicalDefense = new List<csMagicDefense>();
            MagicalDefense.Add(GetRandomMagicalDefense());

            var meditation = new csPassivePower()
            {
                Factor = 0.01f,
                Name = "Meditation",
                CasterLevel = Level,
                BaseStatus = Intelligence,
                Value = 1.0f
            };
            meditation.Init();

            PassivePower = new List<csPassivePower>();
            PassivePower.Add(meditation);

            Knowledge = new List<csMagicDefense>();

            CurrentCombat = new List<csIAttack>();

            CancelInvoke("FoodManagement");
            InvokeRepeating("FoodManagement", 0, 1.0f);
            CancelInvoke("HappinessManagment");
            InvokeRepeating("HappinessManagment", 0, 1.0f);
            CancelInvoke("EnergyManagment");
            InvokeRepeating("EnergyManagment", 0, 1.0f);
            CancelInvoke("HealthManagment");
            InvokeRepeating("HealthManagment", 0, 1.0f);
        }
    }

    public void SetValues()
    {
        csGameController.control.Kagotchi = this;
     
        csGameController.control.LastTime = DateTime.Now;
        
    }

    public void LoadValues()
    {
        if(csGameController.control.LastTime != DateTime.MinValue)
        {
            var deltaTime = (System.DateTime.Now - csGameController.control.LastTime).TotalSeconds;

            Health = csGameController.control.Kagotchi.Health;
            MaxHappiness = csGameController.control.Kagotchi.MaxHappiness;
            MaxEnergy = csGameController.control.Kagotchi.MaxEnergy;
            MaxFood = csGameController.control.Kagotchi.MaxFood;
            MaxHealth = csGameController.control.Kagotchi.MaxHealth;

            Happiness = csGameController.control.Kagotchi.Happiness;
            Happiness -= ((MaxHappiness * float.Parse(deltaTime.ToString())) / (happinessFactor * (int)csGameController.control.Kagotchi.CurrentStage));

            Energy = csGameController.control.Kagotchi.Energy;
            Energy -= ((MaxEnergy * float.Parse(deltaTime.ToString())) / (energyFactor * (8 + (int)csGameController.control.Kagotchi.CurrentStage - 1)));

            Food = csGameController.control.Kagotchi.Food;
            Food -= ((MaxFood * float.Parse(deltaTime.ToString())) / (foodFactor * (int)csGameController.control.Kagotchi.CurrentStage));

            if (Energy <= 0 || Food <= 0)
            {
                Health -= (MaxHealth * float.Parse(deltaTime.ToString())) / (healthFactor * (int)csGameController.control.Kagotchi.CurrentStage);
                Happiness -= (MaxHappiness * float.Parse(deltaTime.ToString())) / (happinessExtraFactor * (int)csGameController.control.Kagotchi.CurrentStage);
            }

            if (Happiness < 0)
                Happiness = 0;

            if (Happiness > MaxHappiness)
                Happiness = MaxHappiness;

            if (Health < 0)
                Health = 0;

            if (Health > MaxHealth)
                Health = MaxHealth;

            if (Energy < 0)
                Energy = 0;

            if (Energy > MaxEnergy)
                Energy = MaxEnergy;

            if (Food < 0)
                Food = 0;

            if (Food > MaxFood)
                Food = MaxFood;

            Weaknesses = csGameController.control.Kagotchi.Weaknesses;
            Powers = csGameController.control.Kagotchi.Powers;
            Attacks = csGameController.control.Kagotchi.Attacks;
            PhysicalDefense = csGameController.control.Kagotchi.PhysicalDefense;
            MagicalDefense = csGameController.control.Kagotchi.MagicalDefense;
            Hitpoints = csGameController.control.Kagotchi.Hitpoints = Hitpoints;
            Stamina = csGameController.control.Kagotchi.Stamina;
            Mana = csGameController.control.Kagotchi.Mana;
            CurrentStage = csGameController.control.Kagotchi.CurrentStage;
        }
        else
        {
            Init();
        }
    }

    private void InitPowers()
    {
        AllPowers = new List<csIAttack>();

        var fire = new csMagicPower()
        {
            Base = 10.0f,
            CombinedPowers = new System.Collections.Generic.List<csMagicPower>(),
            Core = csCorePower.Fire,
            Effects = new System.Collections.Generic.List<csEffect>(),
            Factor = 0.01f,
            ManaBase = 10,
            Name = "Fire",
            Rarity = 1,
            CasterLevel = Level,
            BaseStatus = Intelligence,
            Level = 1
        };
        fire.Init();

        AllPowers.Add(fire);

        var energy = new csMagicPower()
        {
            Base = 10.0f,
            CombinedPowers = new System.Collections.Generic.List<csMagicPower>(),
            Core = csCorePower.Energy,
            Effects = new System.Collections.Generic.List<csEffect>(),
            Factor = 0.01f,
            ManaBase = 10,
            Name = "Energy",
            Rarity = 1,
            CasterLevel = Level,
            BaseStatus = Intelligence,
            Level = 1
        };
        energy.Init();

        AllPowers.Add(energy);

        var ice = new csMagicPower()
        {
            Base = 10.0f,
            CombinedPowers = new System.Collections.Generic.List<csMagicPower>(),
            Core = csCorePower.Ice,
            Effects = new System.Collections.Generic.List<csEffect>(),
            Factor = 0.01f,
            ManaBase = 10,
            Name = "Ice",
            Rarity = 1,
            CasterLevel = Level,
            BaseStatus = Intelligence,
            Level = 1
        };
        ice.Init();

        AllPowers.Add(ice);

        var water = new csMagicPower()
        {
            Base = 10.0f,
            CombinedPowers = new System.Collections.Generic.List<csMagicPower>(),
            Core = csCorePower.Water,
            Effects = new System.Collections.Generic.List<csEffect>(),
            Factor = 0.01f,
            ManaBase = 10,
            Name = "Water",
            Rarity = 1,
            CasterLevel = Level,
            BaseStatus = Intelligence,
            Level = 1
        };
        water.Init();

        AllPowers.Add(water);

        var wind = new csMagicPower()
        {
            Base = 10.0f,
            CombinedPowers = new System.Collections.Generic.List<csMagicPower>(),
            Core = csCorePower.Wind,
            Effects = new System.Collections.Generic.List<csEffect>(),
            Factor = 0.01f,
            ManaBase = 10,
            Name = "Wind",
            Rarity = 1,
            CasterLevel = Level,
            BaseStatus = Intelligence,
            Level = 1
        };
        wind.Init();

        AllPowers.Add(wind);
    }

    private void InitWeaknesses()
    {
        AllWeaknesses = new List<csWeakness>();

        var fire = new csWeakness()
        {
            Multiplier = 1,
            Weakness = csCorePower.Fire
        };

        AllWeaknesses.Add(fire);

        var energy = new csWeakness()
        {
            Multiplier = 1,
            Weakness = csCorePower.Energy
        };

        AllWeaknesses.Add(energy);

        var ice = new csWeakness()
        {
            Multiplier = 1,
            Weakness = csCorePower.Ice
        };

        AllWeaknesses.Add(ice);

        var water = new csWeakness()
        {
            Multiplier = 1,
            Weakness = csCorePower.Water
        };

        AllWeaknesses.Add(water);

        var wind = new csWeakness()
        {
            Multiplier = 1,
            Weakness = csCorePower.Wind
        };

        AllWeaknesses.Add(wind);
    }

    private void InitMagicalDefences()
    {
        AllMagicalDefenses = new List<csMagicDefense>();

        var fire = new csMagicDefense()
        {
            MagicalDefenseType = csCorePower.Fire,
            Factor = 0.01f,
            Name = "Fire Shield",
            Level = 1
        };

        AllMagicalDefenses.Add(fire);

        var energy = new csMagicDefense()
        {
            MagicalDefenseType = csCorePower.Energy,
            Factor = 0.01f,
            Name = "Energy Shield",
            Level = 1
        };

        AllMagicalDefenses.Add(energy);

        var ice = new csMagicDefense()
        {
            MagicalDefenseType = csCorePower.Ice,
            Factor = 0.01f,
            Name = "Ice",
            Level = 1
        };

        AllMagicalDefenses.Add(ice);

        var water = new csMagicDefense()
        {
            MagicalDefenseType = csCorePower.Water,
            Factor = 0.01f,
            Name = "Water",
            Level = 1
        };

        AllMagicalDefenses.Add(water);

        var wind = new csMagicDefense()
        {
            MagicalDefenseType = csCorePower.Wind,
            Factor = 0.01f,
            Name = "Wind",
            Level = 1
        };

        AllMagicalDefenses.Add(wind);
    }

    private void InitPhysicalDefences()
    {
        PhysicalDefense = new List<csPhysicalDefense>();

        var lowDefense = new csPhysicalDefense()
        {
            PhysicalDefenseType = csAttack.LowKick,
            Factor = 0.01f,
            Name = "Low Kick Defense"
        };
        PhysicalDefense.Add(lowDefense);

        var defense = new csPhysicalDefense()
        {
            PhysicalDefenseType = csAttack.Punch,
            Factor = 0.01f,
            Name = "Punch Defense"
        };
        PhysicalDefense.Add(defense);

        var highDefense = new csPhysicalDefense()
        {
            PhysicalDefenseType = csAttack.HighKick,
            Factor = 0.01f,
            Name = "High Kick Defense"
        };
        PhysicalDefense.Add(highDefense);
    }

    private void InitPhysicalAttacks()
    {
        Attacks = new List<csIAttack>();

        var highAttack = new csPhysicalPower()
        {
            Factor = 0.01f,
            Focus = 0.0f,
            Name = "High Kick",
            AttackType = csAttack.HighKick,
            CasterLevel = Level,
            BaseStatus = Strength,
            StaminaBase = 10.0f
        };
        highAttack.Init();
        Attacks.Add(highAttack);

        var attack = new csPhysicalPower()
        {
            Factor = 0.01f,
            Focus = 0.0f,
            Name = "Punch",
            AttackType = csAttack.Punch,
            CasterLevel = Level,
            BaseStatus = Strength,
            StaminaBase = 10.0f
        };
        attack.Init();
        Attacks.Add(attack);

        var lowAttack = new csPhysicalPower()
        {
            Factor = 0.01f,
            Focus = 0.0f,
            Name = "Low Kick",
            AttackType = csAttack.LowKick,
            CasterLevel = Level,
            BaseStatus = Strength,
            StaminaBase = 10.0f
        };
        lowAttack.Init();
        Attacks.Add(lowAttack);
    }

    public csMagicPower GetRandomPower()
    {
        var idx = UnityEngine.Random.Range(0, AllPowers.Count - 1);
        return (csMagicPower)AllPowers[idx];
    }

    public csWeakness GetRandomWeakness()
    {
        var idx = UnityEngine.Random.Range(0, AllWeaknesses.Count - 1);
        return AllWeaknesses[idx];
    }

    public csMagicDefense GetRandomMagicalDefense()
    {
        var idx = UnityEngine.Random.Range(0, AllMagicalDefenses.Count - 1);
        return AllMagicalDefenses[idx];
    }

	// Update is called once per frame
	void Update () 
    {
        
	}


    
    
}
