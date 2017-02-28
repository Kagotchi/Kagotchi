using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class csFightListSceneManager : MonoBehaviour, csISceneManager
{
    List<csBot> bots = new List<csBot>();
    
	// Use this for initialization
	void Start () 
    {
        var enemyGrid = GameObject.Find("EnemyGrid");

        for(var i = 1; i <= 10; i++)
        {
            var bot = new csBot(i);
            bots.Add(bot);
            GameObject prefab = (GameObject)Resources.Load("Prefabs/UI/Bot Profile");
            if (prefab != null)
            {
                var clone = (GameObject)Instantiate(prefab);
                clone.name = prefab.name;
                var profList = clone.GetComponent<csBotProfileList>();
                profList.Name = bot.Name;
                profList.Level = bot.Level;
                profList.Avatar.sprite = bot.Avatar;
                clone.transform.SetParent(enemyGrid.transform, false);
                var buttons = clone.GetComponentsInChildren<Button>();
                foreach(var button in buttons)
                {
                    if(button.name == "btnBotFight")
                    {
                        button.gameObject.AddComponent<csButtonData>().Data = bot;
                    }
                }
            }

        }
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
}
