using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class csBot :csIFighter
{
    public int Level { get; set; }
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Intelligence { get; set; }
    public int Hitpoints { get; set; }
    public int Stamina { get; set; }
    public int Mana { get; set; }
    public Sprite Avatar { get; set; }
    public string Name { get; set; }
    public List<csWeakness> Weaknesses { get; set; }
    public List<csWeakness> AllWeaknesses { get; set; }
    public List<csIAttack> Powers { get; set; }
    public List<csIAttack> AllPowers { get; set; }
    public List<csIAttack> Attacks { get; set; }
    public List<csPhysicalDefense> PhysicalDefense { get; set; }
    public List<csMagicDefense> MagicalDefense { get; set; }
    public List<csMagicDefense> AllMagicalDefenses { get; set; }
    public List<csPassivePower> PassivePower { get; set; }
    public List<csIAttack> CurrentCombat { get; set; }

    private List<Sprite> avatars = new List<Sprite>();
    private float levelPoints;
    private float currLevelPoints;

	// Use this for initialization
    public csBot(int level) 
    {

        Texture2D avatar = (Texture2D)Resources.Load("Sprites/Avatars/Adventure Avatar");
        Sprite sprite = Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), new Vector2(.5f, .5f), 100);
        avatars.Add(sprite);
        avatar = (Texture2D)Resources.Load("Sprites/Avatars/Knight Avatar");
        sprite = Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), new Vector2(.5f, .5f), 100);
        avatars.Add(sprite);
        avatar = (Texture2D)Resources.Load("Sprites/Avatars/Ninja Avatar");
        sprite = Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), new Vector2(.5f, .5f), 100);
        avatars.Add(sprite);
        avatar = (Texture2D)Resources.Load("Sprites/Avatars/Ninja Man Avatar");
        sprite = Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), new Vector2(.5f, .5f), 100);
        avatars.Add(sprite);
        

        var idx = Random.Range(0, avatars.Count);
        Avatar = avatars[idx];

        float points = 0;
        Level = level;

        var percentage = GetRandomPercentage(3);
        levelPoints = (Level - 1) * 50;

        points = (percentage[0] / 100) * levelPoints;
        var strengthPoints = Mathf.FloorToInt(float.Parse(points.ToString()));
        Strength = 100 + strengthPoints;
        currLevelPoints += strengthPoints;

        points = (percentage[1] / 100) * levelPoints;
        var dexterityPoints = Mathf.FloorToInt(float.Parse(points.ToString()));
        Dexterity = 100 + dexterityPoints;
        currLevelPoints += dexterityPoints;

        var inteligencePoints = Mathf.FloorToInt(levelPoints - currLevelPoints);
        Intelligence = 100 + inteligencePoints;

        Hitpoints = Strength;
        Stamina = Dexterity;
        Mana = Intelligence;

        var exampleNames = GetExampleNames();
        var nameGenerator = new csNameGenerator(exampleNames, 0, 4);

        nameGenerator.Reset();
        Name = nameGenerator.NextName;

        InitPowers();
        InitWeaknesses();
        InitMagicalDefences();
        InitPhysicalAttacks();
        InitPhysicalDefences();

        Weaknesses = new List<csWeakness>();
        Weaknesses.Add(GetRandomWeakness());
        var weakness = GetRandomWeakness();
        if (Weaknesses.Count > 0)
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
        Powers.Add(GetRandomPower());

        MagicalDefense = new List<csMagicDefense>();
        MagicalDefense.Add(GetRandomMagicalDefense());

        foreach (var attack in Attacks)
        {
            attack.CasterLevel = Level;
            attack.BaseStatus = Strength;
        }

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

        CurrentCombat = new List<csIAttack>();
    }

    private List<string> GetExampleNames()
    {
        List<string> lst = new List<string>();
        lst.Add("Aiden");
        lst.Add("Jackson");
        lst.Add("Mason");
        lst.Add("Liam");
        lst.Add("Jacob");
        lst.Add("Jayden");
        lst.Add("Ethan");
        lst.Add("Noah");
        lst.Add("Lucas");
        lst.Add("Logan");
        lst.Add("Caleb");
        lst.Add("Caden");
        lst.Add("Jack");
        lst.Add("Ryan");
        lst.Add("Connor");
        lst.Add("Michael");
        lst.Add("Elijah");
        lst.Add("Brayden");
        lst.Add("Benjamin");
        lst.Add("Nicholas");
        lst.Add("Alexander");
        lst.Add("William");
        lst.Add("Matthew");
        lst.Add("James");
        lst.Add("Landon");
        lst.Add("Nathan");
        lst.Add("Dylan");
        lst.Add("Evan");
        lst.Add("Luke");
        lst.Add("Andrew");
        lst.Add("Gabriel");
        lst.Add("Gavin");
        lst.Add("Joshua");
        lst.Add("Owen");
        lst.Add("Daniel");
        lst.Add("Carter");
        lst.Add("Tyler");
        lst.Add("Cameron");
        lst.Add("Christian");
        lst.Add("Wyatt");
        lst.Add("Henry");
        lst.Add("Eli");
        lst.Add("Joseph");
        lst.Add("Max");
        lst.Add("Isaac");
        lst.Add("Samuel");
        lst.Add("Anthony");
        lst.Add("Grayson");
        lst.Add("Zachary");
        lst.Add("David");
        lst.Add("Christopher");
        lst.Add("John");
        lst.Add("Isaiah");
        lst.Add("Levi");
        lst.Add("Jonathan");
        lst.Add("Oliver");
        lst.Add("Chase");
        lst.Add("Cooper");
        lst.Add("Tristan");
        lst.Add("Colton");
        lst.Add("Austin");
        lst.Add("Colin");
        lst.Add("Charlie");
        lst.Add("Dominic");
        lst.Add("Parker");
        lst.Add("Hunter");
        lst.Add("Thomas");
        lst.Add("Alex");
        lst.Add("Ian");
        lst.Add("Jordan");
        lst.Add("Cole");
        lst.Add("Julian");
        lst.Add("Aaron");
        lst.Add("Carson");
        lst.Add("Miles");
        lst.Add("Blake");
        lst.Add("Brody");
        lst.Add("Adam");
        lst.Add("Sebastian");
        lst.Add("Adrian");
        lst.Add("Nolan");
        lst.Add("Sean");
        lst.Add("Riley");
        lst.Add("Bentley");
        lst.Add("Xavier");
        lst.Add("Hayden");
        lst.Add("Jeremiah");
        lst.Add("Jason");
        lst.Add("Jake");
        lst.Add("Asher");
        lst.Add("Micah");
        lst.Add("Jace");
        lst.Add("Brandon");
        lst.Add("Josiah");
        lst.Add("Hudson");
        lst.Add("Nathaniel");
        lst.Add("Bryson");
        lst.Add("Ryder");
        lst.Add("Justin");
        lst.Add("Bryce");

        //—————female

        lst.Add("Sophia");
        lst.Add("Emma");
        lst.Add("Isabella");
        lst.Add("Olivia");
        lst.Add("Ava");
        lst.Add("Lily");
        lst.Add("Chloe");
        lst.Add("Madison");
        lst.Add("Emily");
        lst.Add("Abigail");
        lst.Add("Addison");
        lst.Add("Mia");
        lst.Add("Madelyn");
        lst.Add("Ella");
        lst.Add("Hailey");
        lst.Add("Kaylee");
        lst.Add("Avery");
        lst.Add("Kaitlyn");
        lst.Add("Riley");
        lst.Add("Aubrey");
        lst.Add("Brooklyn");
        lst.Add("Peyton");
        lst.Add("Layla");
        lst.Add("Hannah");
        lst.Add("Charlotte");
        lst.Add("Bella");
        lst.Add("Natalie");
        lst.Add("Sarah");
        lst.Add("Grace");
        lst.Add("Amelia");
        lst.Add("Kylie");
        lst.Add("Arianna");
        lst.Add("Anna");
        lst.Add("Elizabeth");
        lst.Add("Sophie");
        lst.Add("Claire");
        lst.Add("Lila");
        lst.Add("Aaliyah");
        lst.Add("Gabriella");
        lst.Add("Elise");
        lst.Add("Lillian");
        lst.Add("Samantha");
        lst.Add("Makayla");
        lst.Add("Audrey");
        lst.Add("Alyssa");
        lst.Add("Ellie");
        lst.Add("Alexis");
        lst.Add("Isabelle");
        lst.Add("Savannah");
        lst.Add("Evelyn");
        lst.Add("Leah");
        lst.Add("Keira");
        lst.Add("Allison");
        lst.Add("Maya");
        lst.Add("Lucy");
        lst.Add("Sydney");
        lst.Add("Taylor");
        lst.Add("Molly");
        lst.Add("Lauren");
        lst.Add("Harper");
        lst.Add("Scarlett");
        lst.Add("Brianna");
        lst.Add("Victoria");
        lst.Add("Liliana");
        lst.Add("Aria");
        lst.Add("Kayla");
        lst.Add("Annabelle");
        lst.Add("Gianna");
        lst.Add("Kennedy");
        lst.Add("Stella");
        lst.Add("Reagan");
        lst.Add("Julia");
        lst.Add("Bailey");
        lst.Add("Alexandra");
        lst.Add("Jordyn");
        lst.Add("Nora");
        lst.Add("Carolin");
        lst.Add("Mackenzie");
        lst.Add("Jasmine");
        lst.Add("Jocelyn");
        lst.Add("Kendall");
        lst.Add("Morgan");
        lst.Add("Nevaeh");
        lst.Add("Maria");
        lst.Add("Eva");
        lst.Add("Juliana");
        lst.Add("Abby");
        lst.Add("Alexa");
        lst.Add("Summer");
        lst.Add("Brooke");
        lst.Add("Penelope");
        lst.Add("Violet");
        lst.Add("Kate");
        lst.Add("Hadley");
        lst.Add("Ashlyn");
        lst.Add("Sadie");
        lst.Add("Paige");
        lst.Add("Katherine");
        lst.Add("Sienna");
        lst.Add("Piper");

        lst.Add("Smith");
        lst.Add("Johnson");
        lst.Add("Williams");
        lst.Add("Jones");
        lst.Add("Brown");
        lst.Add("Davis");
        lst.Add("Miller");
        lst.Add("Wilson");
        lst.Add("Moore");
        lst.Add("Taylor");
        lst.Add("Anderson");
        lst.Add("Thomas");
        lst.Add("Jackson");
        lst.Add("White");
        lst.Add("Harris");
        lst.Add("Martin");
        lst.Add("Thompson");
        lst.Add("Garcia");
        lst.Add("Martinez");
        lst.Add("Robinson");
        lst.Add("Clark");
        lst.Add("Rodriguez");
        lst.Add("Lewis");
        lst.Add("Lee");
        lst.Add("Walker");
        lst.Add("Hall");
        lst.Add("Allen");
        lst.Add("Young");
        lst.Add("Hernandez");
        lst.Add("King");
        lst.Add("Wright");
        lst.Add("Lopez");
        lst.Add("Hill");
        lst.Add("Scott");
        lst.Add("Green");
        lst.Add("Adams");
        lst.Add("Baker");
        lst.Add("Gonzalez");
        lst.Add("Nelson");
        lst.Add("Carter");
        lst.Add("Mitchell");
        lst.Add("Perez");
        lst.Add("Roberts");
        lst.Add("Turner");
        lst.Add("Phillips");
        lst.Add("Campbell");
        lst.Add("Parker");
        lst.Add("Evans");
        lst.Add("Edwards");
        lst.Add("Collins");
        lst.Add("Stewart");
        lst.Add("Sanchez");
        lst.Add("Morris");
        lst.Add("Rogers");
        lst.Add("Reed");
        lst.Add("Cook");
        lst.Add("Morgan");
        lst.Add("Bell");
        lst.Add("Murphy");
        lst.Add("Bailey");
        lst.Add("Rivera");
        lst.Add("Cooper");
        lst.Add("Richardson");
        lst.Add("Cox");
        lst.Add("Howard");
        lst.Add("Ward");
        lst.Add("Torres");
        lst.Add("Peterson");
        lst.Add("Gray");
        lst.Add("Ramirez");
        lst.Add("James");
        lst.Add("Watson");
        lst.Add("Brooks");
        lst.Add("Kelly");
        lst.Add("Sanders");
        lst.Add("Price");
        lst.Add("Bennett");
        lst.Add("Wood");
        lst.Add("Barnes");
        lst.Add("Ross");
        lst.Add("Henderson");
        lst.Add("Coleman");
        lst.Add("Jenkins");
        lst.Add("Perry");
        lst.Add("Powell");
        lst.Add("Long");
        lst.Add("Patterson");
        lst.Add("Hughes");
        lst.Add("Flores");
        lst.Add("Washington");
        lst.Add("Butler");
        lst.Add("Simmons");
        lst.Add("Foster");
        lst.Add("Gonzales");
        lst.Add("Bryant");
        lst.Add("Alexander");
        lst.Add("Russell");
        lst.Add("Griffin");
        lst.Add("Diaz");
        lst.Add("Hayes");

        return lst;
    }

    private List<float> GetRandomPercentage(int amount)
    {
        int currentPercentage = 100;
        List<float> percentages = new List<float>();
        int divider = 2;
        int min = 0;
        int rand = 0;

        for(var i = 0; i < amount - 1; i++)
        {
            if (i == (amount - 2))
                divider = 1;

            rand = Random.Range(min, Mathf.FloorToInt(currentPercentage / divider));
            min = rand + 1;
            currentPercentage -= rand;
            percentages.Add(rand);
        }
        percentages.Add(currentPercentage);
        return percentages;
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
}
