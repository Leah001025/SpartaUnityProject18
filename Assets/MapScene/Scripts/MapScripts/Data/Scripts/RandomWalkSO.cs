using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomWalkParameters_", menuName = "RandomWalkData")]
public class RandomWalkSO : ScriptableObject
{
    public int iterations = 1;// �ݺ� Ƚ��
    public int walkLength = 10;//������ �̵� Ƚ��
    public bool startRandomlyEachIteration = true;
}
