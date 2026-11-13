using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>タイトル画面 ゲーム開始と終了のボタンを担当</summary>
public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        Time.timeScale = 1f;

        if (startButton != null ) startButton.onClick.AddListener(OnStartClicked);
        else Debug.LogError("[TitleManager] StartButtonが設定されてません");

        if (quitButton != null) quitButton.onClick.AddListener(OnQuitClicked);
        else Debug.LogError("[TitleManager] QuitButtonが設定されてません");
    }

    private void OnStartClicked()
    {
        SceneManager.LoadScene(SceneNames.BossSelect);
    }

    private void OnQuitClicked()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
