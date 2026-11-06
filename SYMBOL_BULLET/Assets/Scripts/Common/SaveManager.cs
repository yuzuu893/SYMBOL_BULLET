using UnityEngine;

/// <summary>
/// ボスごとのハイスコア保存を担当。
/// ボスIDをキーに使うので、ボスごとの記録は混ざらない
/// </summary>
public static class SaveManager
{
    private const string KeyPrefix = "HighScore_";

    ///<summary>ボスのハイスコアを取得する 記録がなければ0</summary>
    public static int GetHighScore(string bossId)
    {
        if (string.IsNullOrEmpty(bossId)) return 0;
        return PlayerPrefs.GetInt(KeyPrefix + bossId, 0);
    }

    /// <summary>
    /// スコアを記録する。既存より高い場合だけ更新、更新したらtrueを返す
    /// 新記録でない場合は、既存の記録を維持
    /// </summary>
    public static bool TrySaveHighScore(string bossId, int score)
    {
        if (string.IsNullOrEmpty(bossId)) return false;

        int current = GetHighScore(bossId);
        if (score <= current) return false;

        PlayerPrefs.SetInt(KeyPrefix + bossId, score);
        PlayerPrefs.Save();
        return true;
    }

    /// <summary>【テスト用】ボスのハイスコアを消す</summary>
    public static void ResetHighScore(string bossId)
    {
        PlayerPrefs.DeleteKey(KeyPrefix + bossId);
        PlayerPrefs.Save();
    }
}
