using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 【テスト用仮スプリクト】動作確認用、確認後削除
/// キー操作とスコア計算、保存をConsoleに出力する
/// </summary>
public class ScoreTester : MonoBehaviour
{
    [SerializeField] private ScoreCalculator calculator = new ScoreCalculator();
    [SerializeField] private string bossId = "BOSS_01";
    [SerializeField] private float testClearTime = 30f;
    [Range(0f, 1f)]
    [SerializeField] private float tesuhpRatio = 0.8f;

    private void Update()
    {
        if (calculator == null) calculator = new ScoreCalculator();

        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        //1キー 計算して保存を試みる
        if (kb.digit1Key.wasPressedThisFrame)
        {
            ScoreResult r = calculator.CalculateClearScore(testClearTime, tesuhpRatio);
            bool updated = SaveManager.TrySaveHighScore(bossId, r.Total);
            Debug.Log($"[ScoreTester] 基本{r.basePoints} + 時間{r.timeBonus} + HP{r.hpBonus} = {r.Total} / 新記録:{updated} / 現在のハイスコア:{SaveManager.GetHighScore(bossId)}");
        }

        //2キー 記録を消す
        if (kb.digit2Key.wasPressedThisFrame)
        {
            SaveManager.ResetHighScore(bossId);
            Debug.Log($"[ScoreTester] {bossId} のハイスコアを消しました。");
        }
    }
}
