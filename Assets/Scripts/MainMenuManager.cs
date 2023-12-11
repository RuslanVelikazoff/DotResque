using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _newBestText;
    [SerializeField] private TMP_Text _bestScoreText;

    [SerializeField] private Button musicButton;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;

    [SerializeField] private Button soundButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    private void Awake()
    {
        _bestScoreText.text = GameManager.Instance.HighScore.ToString();

        if(!GameManager.Instance.IsInitialized)
        {
            _scoreText.gameObject.SetActive(false);
            _newBestText.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(ShowScore());
        }

        SetSprites();
        ButtonClickAction();
    }

    [SerializeField] private float _animationTime;
    [SerializeField] private AnimationCurve _speedCurve;

    private IEnumerator ShowScore()
    {
        int tempScore = 0;
        _scoreText.text = tempScore.ToString();

        int currentScore = GameManager.Instance.CurrentScore;
        int highScore = GameManager.Instance.HighScore;

        if(highScore < currentScore)
        {
            _newBestText.gameObject.SetActive(true);
            GameManager.Instance.HighScore = currentScore;
        }
        else
        {
            _newBestText.gameObject.SetActive(false);
        }

        _bestScoreText.text = GameManager.Instance.HighScore.ToString();
        float speed = 1 / _animationTime;
        float timeElapsed = 0f;

        while(timeElapsed < 1f)
        {
            timeElapsed += speed * Time.deltaTime;
            tempScore = (int)(_speedCurve.Evaluate(timeElapsed) * currentScore);
            _scoreText.text = tempScore.ToString();
            yield return null;
        }

        tempScore = currentScore;
        _scoreText.text = tempScore.ToString();

    }

    [SerializeField] private AudioClip _clickClip;

    public void ClickedPlay()
    {
        AudioManager.instance.Play("Click");
        GameManager.Instance.GotoGameplay();
    }

    private void ButtonClickAction()
    {
        if (musicButton != null)
        {
            musicButton.onClick.RemoveAllListeners();
            musicButton.onClick.AddListener(() =>
            {
                AudioManager.instance.Play("Click");

                if (PlayerPrefs.GetFloat("MusicVolume") == 1)
                {
                    AudioManager.instance.OffMusic();
                }
                else
                {
                    AudioManager.instance.OnMusic();
                }

                SetSprites();
            });
        }

        if (soundButton != null)
        {
            soundButton.onClick.RemoveAllListeners();
            soundButton.onClick.AddListener(() =>
            {
                AudioManager.instance.Play("Click");

                if (PlayerPrefs.GetFloat("SoundVolume") == 1)
                {
                    AudioManager.instance.OffSound();
                }
                else
                {
                    AudioManager.instance.OnSound();
                }

                SetSprites();
            });
        }
    }

    private void SetSprites()
    {
        if (PlayerPrefs.GetFloat("MusicVolume") == 1)
        {
            musicButton.GetComponent<Image>().sprite = musicOnSprite;
        }
        else if (PlayerPrefs.GetFloat("MusicVolume") == 0)
        {
            musicButton.GetComponent<Image>().sprite = musicOffSprite;
        }

        if (PlayerPrefs.GetFloat("SoundVolume") == 1)
        {
            soundButton.GetComponent<Image>().sprite = soundOnSprite;
        }
        else if (PlayerPrefs.GetFloat("SoundVolume") == 0)
        {
            soundButton.GetComponent<Image>().sprite = soundOffSprite;
        }
    }
}

