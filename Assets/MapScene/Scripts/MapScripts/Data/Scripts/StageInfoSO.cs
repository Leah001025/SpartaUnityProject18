using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage_", menuName = "info/StageData")]
public class StageInfoSO : ScriptableObject
{
    public string stageName;

    public List<GameObject> monsterList;
    public List<GameObject> bossList;

    public GameObject boss_Room;
}
