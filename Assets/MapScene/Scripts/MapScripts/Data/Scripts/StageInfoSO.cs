using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage_", menuName = "info/StageData")]
public class StageInfoSO : ScriptableObject
{
    public List<GameObject> monsterList;
    public List<GameObject> bossList;
    public List<GameObject> itemList;

    public GameObject boss_Room;
}
