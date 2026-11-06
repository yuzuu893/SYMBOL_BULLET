using System;
using UnityEngine;

/// <summary>スコアの内訳</summary>
public struct ScoreResult
{
    public int basePoints;
    public int timeBonus;
    public int hpBonus;
    public int Total => basePoints + timeBonus + hpBonus;
}

/// <summary>
/// スコア計算専用クラス 配点はInspectorから変更可
/// 他のシステムには干渉しない
/// </summary>
[Serializable]
public class ScoreCalculator
{
    [Tooltip("ボス撃破時の基本得点")]
    [SerializeField] private int basePoints = 10000;

    [Tooltip("時間ボーナスの最大点(クリア時間0秒時)")]
    [SerializeField] private int maxTimeBonus = 10000;

    [Tooltip("この時間かかると時間ボーナスは0")]
    [Min(1f)]
    [SerializeField] private float timeLimitForBonus = 120f;

    [Tooltip("HPボーナスの最大点")]
    [SerializeField] private int maxHpBonus = 10000;

    ///<summary>
    ///クリア時のスコアを計算する
    ///clearTIme = クリアにかかった秒数、hpRatio = 残りHP割合
    /// </summary>
    public ScoreResult CalculateClearScore(float clearTIme, float hpRatio)
    {
        float timeRatio = 1f - Mathf.Clamp01(clearTIme / Mathf.Max(1f, timeLimitForBonus));
        float hp = Mathf.Clamp01(hpRatio);

        ScoreResult r = new ScoreResult
        {
            basePoints = basePoints,
            timeBonus = Mathf.RoundToInt(maxTimeBonus * timeRatio),
            hpBonus = Mathf.RoundToInt(maxHpBonus * timeRatio),
        };
        return r;
    }
}
