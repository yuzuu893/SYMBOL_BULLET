using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ボス選択画面 BossCatalogの内容からボタンを生成、選んだBossCatalogをBattleContextに入れて戦闘シーンへ進む
/// </summary>
public class BossSelectManager : MonoBehaviour
{
    [Header("データ")]
    [SerializeField] private BossCatalog catalog;

    [Header("UI")]
    [Tooltip("ボタンを並べる親")]
    [SerializeField] private Transform buttonContainer;

    [Tooltip("ボスボタンのPrefub")]
    [SerializeField] private Button bossButtonPrefab;

    [SerializeField] private Button backButton;

    private void Awake()
    {
        Time.timeScale = 1f;

        if (backButton != null) backButton.onClick.AddListener(OnBackClicked);
        else Debug.LogError("[BossSelectManager] BackButtonが設定されてません",this);
    }

    private void Start()
    {
        if (catalog == null || catalog.Bosses == null || buttonContainer == null || bossButtonPrefab == null)
        {
            Debug.LogError("[BossSelectManager] Catalog / ButtonContainer / BossButton Prefab のいずれかが未設定です。", this);
            return;
        }

        foreach (BossData boss in catalog.Bosses)
        {
            if (boss == null) continue;
            CreateButton(boss);
        }
    }

    private void CreateButton(BossData boss)
    {
        Button button = Instantiate(bossButtonPrefab, buttonContainer);

        TMP_Text label = button.GetComponentInChildren<TMP_Text>();
        if (label != null)
        {
            int high = SaveManager.GetHighScore(boss.BossId);
            label.text = $"{boss.BossId}  {boss.DisplayName}    HI {high:N0}";
        }

        //ラムダが各ボスを正しく捕まえられるよう、ローカル変数に入れる
        BossData selected = boss;
        button.onClick.AddListener(() => OnBossSelected(selected));
    }

    private void OnBossSelected(BossData boss)
    {
        BattleContext.ClearResult();
        BattleContext.SelectedBoss = boss;
        SceneManager.LoadScene(SceneNames.Battle);
    }

    private void OnBackClicked()
    {
        SceneManager.LoadScene(SceneNames.Title);
    }
}
