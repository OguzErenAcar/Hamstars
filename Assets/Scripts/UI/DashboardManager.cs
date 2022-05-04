using UnityEngine;
using TMPro;

public class DashboardManager : MonoBehaviour {
    [Header("Managers")]
    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private Timer _timer;
    [Header("Dashboard Content")]
    [SerializeField]
    private TMP_Text _gameTimerUI;
    [SerializeField]
    private TMP_Text _p1ScoreUI;
    [SerializeField]
    private TMP_Text _p2ScoreUI;
    [SerializeField]
    private TMP_Text _p3ScoreUI;
    [SerializeField]
    private TMP_Text _p4ScoreUI;

    private void Awake() => ResetDashboard();

    private void Update() {
        _gameTimerUI.text = _timer.GetTimeString();

        _p1ScoreUI.text = _scoreManager.PlayerOneScoreArea.CurrentScore.ToString();
        _p2ScoreUI.text = _scoreManager.PlayerTwoScoreArea.CurrentScore.ToString();
        _p3ScoreUI.text = _scoreManager.PlayerThreeScoreArea.CurrentScore.ToString();
        _p4ScoreUI.text = _scoreManager.PlayerFourScoreArea.CurrentScore.ToString();
    }

    public void ResetDashboard() {
        _timer.ResetTimer();
        _timer.StartTimer = true;

        _p1ScoreUI.text = "0";
        _p2ScoreUI.text = "0";
        _p3ScoreUI.text = "0";
        _p4ScoreUI.text = "0";
    }
}
