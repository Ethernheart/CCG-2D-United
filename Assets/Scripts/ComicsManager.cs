using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ComicsManger : MonoBehaviour
{
	public AudioClip[] audioClips;
	public Sprite[] images; // Массив для хранения спрайтов (картинок)
	public float fadeInDuration = 2.0f;
	public float timeBetweenClips = 2.0f;
	public float zoomDuration = 2.0f;
	public float zoomScale = 0.9f;
	public AudioSource audioSource;
	public Image image; // Ссылка на компонент Image для отображения спрайтов
	public CanvasGroup canvasGroup; // Ссылка на компонент CanvasGroup для управления прозрачностью
	public string nextSceneName; // Имя следующей сцены
	public Image background;

	private void Start()
	{
		StartCoroutine(PlayAudioSequentially());
	}

	IEnumerator PlayAudioSequentially()
	{
		for (int i = 0; i < audioClips.Length; i++)
		{
			// Затемнение
			canvasGroup.DOFade(0, fadeInDuration);
			yield return new WaitForSeconds(fadeInDuration);

			// Смена спрайта
			if (i < images.Length)
			{
				image.sprite = images[i];
			}

			// Воспроизведение аудио
			audioSource.clip = audioClips[i];
			audioSource.Play();

			// Постепенное появление
			canvasGroup.DOFade(1, fadeInDuration);
			yield return new WaitForSeconds(fadeInDuration + audioClips[i].length + timeBetweenClips);

			// Приближение
			image.rectTransform.DOScale(zoomScale, zoomDuration);
			yield return new WaitForSeconds(zoomDuration);

			// Возврат к обычному масштабу
			image.rectTransform.DOScale(1.0f, 0);

			// Задержка перед следующим аудио
			yield return new WaitForSeconds(timeBetweenClips);
		}

		// Переключение на другую сцену
		SceneManager.LoadScene(nextSceneName);

	}

}
