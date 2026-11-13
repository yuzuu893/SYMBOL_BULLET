using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>戦闘の状態</summary>
public enum BattleState
{
    Playing,
    Cleared,
    GameOver
}

/// <summary>
/// 戦闘の進行・勝敗判定・スコア確定を担当
/// 勝敗は最初の1回だけ確定する（二重判定防止）
/// </summary>
public class BattleManager : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private BossHealth boss;
    [SerializeField] private BossController bossController;
    [SerializeField] private PlayerHealth player;

    [Header("スコア")]
    [SerializeField] private ScoreCalculator scoreCalculator = new ScoreCalculator();

    [Header("結果画面への遷移")]
    [Tooltip("勝敗確定から結果画面へ移るまでの待ち時間（実時間・秒）。")]
    [SerializeField] private float resultDelay = 1.5f;

    private bool transitionStarted;

    public BattleState State { get; private set; } = BattleState.Playing;
    public float ElapsedTime { get; private set; }

    /// <summary>勝敗が確定したときに1回だけ呼ばれる</summary>
    public event Action<BattleState> OnBattleEnded;

    private void Awake()
    {
        // シーン再読み込み時の初期化
        Time.timeScale = 1f;
        State = BattleState.Playing;
        ElapsedTime = 0f;
        transitionStarted = false;

        // 前回の結果を消す（選択中のボスは残す）
        BattleContext.ClearResult();

        if (scoreCalculator == null) scoreCalculator = new ScoreCalculator();
        if (boss == null) Debug.LogError("[BattleManager] Boss が設定されていません。", this);
        if (player == null) Debug.LogError("[BattleManager] Player が設定されていません。", this);
    }

    private void OnEnable()
    {
        if (boss != null) boss.OnDefeated += HandleBossDefeated;
        if (player != null) player.OnDefeated += HandlePlayerDefeated;
    }

    private void OnDisable()
    {
        if (boss != null) boss.OnDefeated -= HandleBossDefeated;
        if (player != null) player.OnDefeated -= HandlePlayerDefeated;
    }

    private void OnDestroy()
    {
        // 他シーンへの持ち越し防止
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (State == BattleState.Playing)
        {
            ElapsedTime += Time.deltaTime;
        }
    }

    private void HandleBossDefeated() => EndBattle(BattleState.Cleared);
    private void HandlePlayerDefeated() => EndBattle(BattleState.GameOver);

    private void EndBattle(BattleState result)
    {
        if (State != BattleState.Playing) return; // 二重判定防止

        State = result;
        bool cleared = result == BattleState.Cleared;
        Debug.Log($"[BattleManager] 戦闘終了: {result} / {ElapsedTime:F1}秒");

        // スコア確定（ゲームオーバーは0点・記録更新なし）
        string bossId = GetBossId();
        ScoreResult score = default;
        bool newRecord = false;

        if (cleared)
        {
            score = scoreCalculator.CalculateClearScore(ElapsedTime, player.HpRatio);
            newRecord = SaveManager.TrySaveHighScore(bossId, score.Total);
        }

        BattleContext.SetResult(cleared, score, ElapsedTime,
            player.CurrentHP, player.MaxHP, newRecord, SaveManager.GetHighScore(bossId));

        Debug.Log($"[BattleManager] スコア {score.Total}（基本{score.basePoints}/時間{score.timeBonus}/HP{score.hpBonus}） 新記録:{newRecord}");

        if (cleared)
        {
            // 撃破：ボスを止め、残っている弾を消す
            if (boss != null) boss.gameObject.SetActive(false);
            foreach (Projectile p in FindObjectsByType<Projectile>(FindObjectsSortMode.None))
            {
                Destroy(p.gameObject);
            }
        }

        Time.timeScale = 0f; // 全体を停止（UIは影響を受けない）
        OnBattleEnded?.Invoke(result);

        if (!transitionStarted)
        {
            transitionStarted = true;
            StartCoroutine(GoToResult());
        }
    }

    private System.Collections.IEnumerator GoToResult()
    {
        // timeScale=0 でも進むよう実時間で待つ
        yield return new WaitForSecondsRealtime(resultDelay);
        SceneManager.LoadScene(SceneNames.Result);
    }

    // 選択中のボスID。単体テスト時はBossControllerのDataから取る
    private string GetBossId()
    {
        if (BattleContext.SelectedBoss != null) return BattleContext.SelectedBoss.BossId;
        if (bossController != null && bossController.Data != null) return bossController.Data.BossId;
        return "BOSS_UNKNOWN";
    }
}