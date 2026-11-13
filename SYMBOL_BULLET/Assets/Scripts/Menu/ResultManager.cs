using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 結果画面。BattleContext の結果を表示し、再挑戦・ボス選択への遷移を担当
/// </summary>
public class ResultManager : MonoBehaviour
{
    [Header("表示")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bossText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text detailText;
    [SerializeField] private TMP_Text newRecordText;

    [Header("ボタン")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button bossSelectButton;

    private void Awake()
    {
        Time.timeScale = 1f;

        if (retryButton != null) retryButton.onClick.AddListener(OnRetryClicked);
        else Debug.LogError("[ResultManager] RetryButton が設定されていません", this);

        if (bossSelectButton != null) bossSelectButton.onClick.AddListener(OnBossSelectClicked);
        else Debug.LogError("[ResultManager] BossSelectButton が設定されていません", this);
    }

    private void Start()
    {
        if (!BattleContext.HasResult)
        {
            // 単体テスト等で結果なしに開いた場合
            Debug.LogWarning("[ResultManager] 戦闘結果がありません", this);
            if (titleText != null) titleText.text = "NO RESULT";
            if (newRecordText != null) newRecordText.gameObject.SetActive(false);
            return;
        }

        BossData boss = BattleContext.SelectedBoss;
        if (bossText != null) bossText.text = boss != null ? $"{boss.BossId}  {boss.DisplayName}" : "";

        if (titleText != null) titleText.text = BattleContext.Cleared ? "CLEAR" : "GAME OVER";

        if (scoreText != null) scoreText.text = $"SCORE  {BattleContext.FinalScore:N0}";

        if (detailText != null)
        {
            ScoreResult s = BattleContext.ScoreDetail;
            float t = BattleContext.ClearTime;
            detailText.text =
                $"TIME  {(int)t / 60:00}:{t % 60f:00.0}\n" +
                $"HP  {BattleContext.RemainingHp} / {BattleContext.MaxHp}\n" +
                $"BASE {s.basePoints:N0}   TIME {s.timeBonus:N0}   HP {s.hpBonus:N0}\n" +
                $"HIGH SCORE  {BattleContext.HighScore:N0}";
        }

        if (newRecordText != null) newRecordText.gameObject.SetActive(BattleContext.IsNewRecord);
    }

    private void OnRetryClicked()
    {
        // 同じボスで再挑戦（SelectedBossは保持されている）
        SceneManager.LoadScene(SceneNames.Battle);
    }

    private void OnBossSelectClicked()
    {
        SceneManager.LoadScene(SceneNames.BossSelect);
    }
}
