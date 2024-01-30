using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject NextStageButton;
    [SerializeField] private GameObject Player;

    [SerializeField] private TextMeshProUGUI remainEnemyText;
    [SerializeField] private TextMeshProUGUI stageName;
    [SerializeField] private TextMeshProUGUI deadStageName;

    [SerializeField] private Slider HPSlider;

    private void Awake()
    {
        GameOverUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        stageName.text = RoomGenerateManager.instance.GetStageName();
        deadStageName.text = RoomGenerateManager.instance.GetStageName();
    }

    // Update is called once per frame
    void Update()
    {
        remainEnemyText.text = BattleManager.instance.currentMonsterCount.ToString();

        if (BattleManager.instance.bossDead)
        {
            StartCoroutine("GoNextButton");
        }

        UpdateHealth();
    }

    private void UpdateHealth()
    {
        PlayerStat hpStat = Player.GetComponent<PlayerStat>();
        HPSlider.value = hpStat.currentHealth / hpStat.maxHealth;
    }

    public void OnGoNextStageButton()//다음 스테이지 버튼 눌렀을 때
    {

    }

    public void OnGoEndingButton()//게임 클리어 버튼 눌렀을 때
    {

    }

    public void OnGameOverButton()//게임 오버 버튼 눌렀을 때
    {

    }

    IEnumerator GoNextButton()
    {
        NextStageButton.transform.position = Vector3.Lerp(NextStageButton.transform.position, new Vector3(NextStageButton.transform.position.x, 16, NextStageButton.transform.position.z), Time.deltaTime * 2f);

        yield return null;
    }

    IEnumerator GameOver()
    {
        GameOverUI.SetActive(true);
        GameObject panel = GameOverUI.transform.Find("GameOverPanel").gameObject;

        panel.transform.position = Vector3.Lerp(panel.transform.position, new Vector3(panel.transform.position.x, 0, panel.transform.position.z), Time.deltaTime * 5f);

        yield return null;
    }
}
