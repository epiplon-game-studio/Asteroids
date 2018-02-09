using Asteroids.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.UI
{
    public class UIGame : MonoBehaviour,
        IEventListener<GameEvent>,
        IEventListener<GameStateChangedEvent>
    {
        [Header("Menu")]
        public Button startButton;
        public Button resumeButton;
        public Button quitButton;
        public UIGroup menuGroup;

        [Header("HUD")]
        public Text Points;
        public Text Lives;
        public UIGroup hudGroup;

        [Header("Game Over")]
        public UIGroup gameOverGroup;
        public InputField highscoreInput;

        [Header("Highscores")]
        public UIGroup highscoresGroup;
        public RectTransform highscoreContent;
        public UIHighscore highscoreTemplate;
        public Button highscoreContinue;

        private void Awake()
        {
            this.Listen<GameEvent>();
            this.Listen<GameStateChangedEvent>();

            startButton.onClick.AddListener(() =>
            {
                EventManager.Trigger(new GameStateChangedEvent(GameState.Started));

            });
            resumeButton.onClick.AddListener(() =>
            {
                EventManager.Trigger(new GameStateChangedEvent(GameState.Resumed));

            });
            highscoreInput.onEndEdit.AddListener((name) =>
            {
                EventManager.Trigger(new CreateHighscore(name));
            });
            highscoreContinue.onClick.AddListener(() =>
            {
                EventManager.Trigger(new GameStateChangedEvent(GameState.NotStarted));
            });

            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                quitButton.gameObject.SetActive(false);
            }
            else
            {
                quitButton.onClick.AddListener(() => Application.Quit());
            }
            
            for (int i = 0; i < Game.current.Highscores.Entries.Length; i++)
                Instantiate(highscoreTemplate, highscoreContent, worldPositionStays: false);
        }

        private void OnDisable()
        {
            this.Unlisten<GameEvent>();
            this.Unlisten<GameStateChangedEvent>();
        }

        public void OnEvent(GameEvent e)
        {
            Points.text = e.Points.ToString("00000");
            Lives.text = e.Lives.ToString();
        }

        public void OnEvent(GameStateChangedEvent e)
        {
            switch (e.CurrentState)
            {
                case GameState.NotStarted:
                    startButton.gameObject.SetActive(true);
                    resumeButton.gameObject.SetActive(false);
                    highscoresGroup.Hide();
                    menuGroup.Show();
                    hudGroup.Hide();
                    break;
                case GameState.GameOver:
                    gameOverGroup.Show();
                    highscoreInput.Select();
                    break;
                case GameState.Paused:
                    menuGroup.Show();
                    resumeButton.gameObject.SetActive(true);
                    startButton.gameObject.SetActive(false);
                    break;
                case GameState.Started:
                    startButton.gameObject.SetActive(true);
                    resumeButton.gameObject.SetActive(false);
                    menuGroup.Hide();
                    hudGroup.Show();
                    break;
                case GameState.Resumed:
                    resumeButton.gameObject.SetActive(true);
                    startButton.gameObject.SetActive(false);
                    menuGroup.Hide();
                    hudGroup.Show();
                    break;
                case GameState.Highscores:
                    highscoreInput.text = string.Empty;
                    gameOverGroup.Hide();
                    highscoresGroup.Show();
                    for (int i = 0; i < highscoreContent.childCount; i++)
                    {
                        var entry = highscoreContent.GetChild(i).GetComponent<UIHighscore>();
                        entry.SetMessage(i, Game.current.Highscores.Entries[i]);
                    }
                    break;

            }
        }
    }
}
