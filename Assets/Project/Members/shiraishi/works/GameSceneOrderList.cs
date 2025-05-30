using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameSceneOrderList", menuName = "Game/GameSceneOrderList")]
public class GameSceneOrderList : ScriptableObject
{
    public List<string> sceneNames;
}
