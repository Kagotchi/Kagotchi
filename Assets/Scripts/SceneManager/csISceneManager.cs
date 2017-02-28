using UnityEngine;
using System.Collections;

public interface csISceneManager 
{
    void OnApplicationPause(bool paused);
    void UpdateScenery(bool scroll, float direction);
    void SetNoItemVisibility(bool visible);
}
