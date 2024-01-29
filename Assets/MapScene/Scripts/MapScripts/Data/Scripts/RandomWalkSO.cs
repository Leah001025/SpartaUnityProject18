using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomWalkParameters_", menuName = "RandomWalkData")]
public class RandomWalkSO : ScriptableObject
{
    public int iterations = 1;// 반복 횟수
    public int walkLength = 10;//무작위 이동 횟수
    public bool startRandomlyEachIteration = true;
}
