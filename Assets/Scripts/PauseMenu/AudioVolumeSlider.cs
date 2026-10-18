using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    public AudioSource testAudio;

    private void Start()
    {
        testAudio.Play(0);
        // Sets the slider's initial volume
        volumeSlider.value = AudioListener.volume;

        // Adds the event to update the volume
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private void OnDestroy()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(ChangeVolume);
        }
    }
}
