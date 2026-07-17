using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private ProfileView _profileView;

    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OpenProfile()
    {
        _profileView.Show(
            GameManager.Instance.Progress,
            GameManager.Instance.Statistics);
    }
}
