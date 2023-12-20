using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ComicsManger : MonoBehaviour
{
	public AudioClip[] audioClips;
	public Sprite[] images; // ������ ��� �������� �������� (��������)
	public float fadeInDuration = 2.0f;
	public float timeBetweenClips = 2.0f;
	public float zoomDuration = 2.0f;
	public float zoomScale = 0.9f;
	public AudioSource audioSource;
	public Image image; // ������ �� ��������� Image ��� ����������� ��������
	public CanvasGroup canvasGroup; // ������ �� ��������� CanvasGroup ��� ���������� �������������
	public string nextSceneName; // ��� ��������� �����
	public Image background;

	private void Start()
	{
		StartCoroutine(PlayAudioSequentially());
	}

	IEnumerator PlayAudioSequentially()
	{
		for (int i = 0; i < audioClips.Length; i++)
		{
			// ����������
			canvasGroup.DOFade(0, fadeInDuration);
			yield return new WaitForSeconds(fadeInDuration);

			// ����� �������
			if (i < images.Length)
			{
				image.sprite = images[i];
			}

			// ��������������� �����
			audioSource.clip = audioClips[i];
			audioSource.Play();

			// ����������� ���������
			canvasGroup.DOFade(1, fadeInDuration);
			yield return new WaitForSeconds(fadeInDuration + audioClips[i].length + timeBetweenClips);

			// �����������
			image.rectTransform.DOScale(zoomScale, zoomDuration);
			yield return new WaitForSeconds(zoomDuration);

			// ������� � �������� ��������
			image.rectTransform.DOScale(1.0f, 0);

			// �������� ����� ��������� �����
			yield return new WaitForSeconds(timeBetweenClips);
		}

		// ������������ �� ������ �����
		SceneManager.LoadScene(nextSceneName);

	}

}
