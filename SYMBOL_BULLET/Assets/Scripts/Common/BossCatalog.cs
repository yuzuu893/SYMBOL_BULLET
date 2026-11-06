using UnityEngine;

/// <summary>
/// 5体のBossDataの一覧 ボス選択画面で使う
/// </summary>
[CreateAssetMenu(fileName = "BossCatalog", menuName ="SymbolBullet/Boss Catalog")]
public class BossCatalog : ScriptableObject
{
    [Tooltip("ボスデータの一覧(選択画面に表示する順)")]
    [SerializeField] private BossData[] bosses;

    public BossData[] Bosses => bosses;
}
