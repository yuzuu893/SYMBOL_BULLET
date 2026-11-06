/// <summary>
/// シーン間で受け渡す戦闘情報（static）
/// ボス選択 → 戦闘 → 結果 の間で値を保持する
/// </summary>
public static class BattleContext
{
    // --- ボス選択 → 戦闘 ---
    ///<summary>選択されたボス nullなら未選択（BattleScene単体テスト時）</summary>
    public static BossData SelectedBoss { get; set; }

    // --- 戦闘 → 結果 ---
    public static bool HasResult { get; private set; }
    public static bool Cleared { get; private set; }
    public static int FinalScore { get; private set; }
    public static ScoreResult ScoreDetail { get; private set; }
    public static float ClearTime { get; private set; }
    public static float RemainingHpRatio { get; private set; }
    public static int RemainingHp { get; private set; }
    public static int MaxHp { get; private set; }
    public static bool IsNewRecord { get; private set; }
    public static int HighScore { get; private set; }

    ///<summary>戦闘結果を保存する</summary>
    public static void SetResult(bool cleared, ScoreResult detail, float time,
        int remainingHp, int maxHp, bool newRecord, int highScore)
    {
        HasResult = true;
        Cleared = cleared;
        ScoreDetail = detail;
        FinalScore = detail.Total;
        ClearTime = time;
        RemainingHp = remainingHp;
        MaxHp = maxHp;
        RemainingHpRatio = maxHp > 0 ? (float)remainingHp / maxHp : 0f;
        IsNewRecord = newRecord;
        HighScore = highScore;
    }

    ///<summary>結果だけを消す（選択中のボスは残す）</summary>
    public static void ClearResult()
    {
        HasResult = false;
        Cleared = false;
        FinalScore = 0;
        ScoreDetail = default;
        ClearTime = 0f;
        RemainingHpRatio = 0f;
        RemainingHp = 0;
        MaxHp = 0;
        IsNewRecord = false;
        HighScore = 0;
    }
}
