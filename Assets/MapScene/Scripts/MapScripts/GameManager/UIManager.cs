using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject NextStageButton;
    [SerializeField] TextMeshProUGUI remainEnemyText;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        remainEnemyText.text = BattleManager.instance.currentMonsterCount.ToString();

        if (BattleManager.instance.bossDead)
        {
            StartCoroutine("GoNextButton");
        }
    }

    IEnumerator GoNextButton()
    {
        NextStageButton.transform.position = Vector3.Lerp(NextStageButton.transform.position, new Vector3(NextStageButton.transform.position.x, 16, NextStageButton.transform.position.z), Time.deltaTime * 2f);

        yield return null;
    }
}
