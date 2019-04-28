using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOL
{
	[RequireComponent(typeof(CanvasGroup))]
	public class MainMenu : MonoBehaviour
	{
		[SerializeField]
		InputField seedField, worldSizeField;
		[SerializeField]
		Slider speedControl;
		[SerializeField]
		Button startButton, closeButton, quitButton;
		[SerializeField]
		Text notification;

		private CanvasGroup canvasGroup;

		IWorld world;
		private const int DEFAULT_WORLD_SIZE = 100;
		private const string SIZE_WARNING_MSG = "Only integer are allowed.Size defaulted to ";
		private int size;
		private float speed;

		private bool isGameStarted = false;

		private void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
			worldSizeField.onEndEdit.AddListener(VerifySize);
			startButton.onClick.AddListener(StartGame);
			closeButton.onClick.AddListener(CloseMenu);
			quitButton.onClick.AddListener(QuitGame);
			speedControl.onValueChanged.AddListener(SetSpeed);
			world = FindObjectOfType<World>() as IWorld;
			closeButton.gameObject.SetActive(false);

		}

		private void SetSpeed(float value)
		{
			speed = value;
		}

		private void QuitGame()
		{
			Application.Quit();
		}

		private void CloseMenu()
		{
			ResumeGame();
		}

		private void ResumeGame()
		{
			Time.timeScale = 1;
			Hide();
		}

		private void VerifySize(string worldSize)
		{
			if (!int.TryParse(worldSize, out size))
			{
				notification.text = SIZE_WARNING_MSG + DEFAULT_WORLD_SIZE;
				notification.color = Color.yellow;
				size = DEFAULT_WORLD_SIZE;
			}
		}
		private void Update()
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				PauseGame();
			}
		}

		private void PauseGame()
		{
			closeButton.gameObject.SetActive(true);
			Show();
			Time.timeScale = 0;
		}

		private void StartGame()
		{
			world.StartWorld(seedField.text, size, speed);
			isGameStarted = true;
			ResumeGame();
		}

		private void Hide()
		{
			canvasGroup.alpha = 0;
			canvasGroup.blocksRaycasts = false;
		}
		private void Show()
		{
			canvasGroup.alpha = 1;
			canvasGroup.blocksRaycasts = true;
		}
	}
}

