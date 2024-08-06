using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(House))]

public class Alarm : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume;

    private AudioSource _audioSource;
    private House _house;
    private Coroutine _coroutine;

    private float _volumeChangeRate = 0.1f;
    private float _maxVolume = 1f;
    private float _minVolume = 0f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _house = GetComponent<House>();
        _audioSource.clip = _audioClip;
        _audioSource.volume = _volume;
    }

    private void OnEnable()
    {
        _house.HouseBreakInDetected += TurnOnAlarm;
        _house.HouseBreakOutDetected += TurnOffAlarm;
    }

    private void OnDisable()
    {
        _house.HouseBreakInDetected -= TurnOnAlarm;
        _house.HouseBreakOutDetected -= TurnOffAlarm;
    }

    private void TurnOnAlarm()
    {
        HandleVolumeChange(_maxVolume);
    }

    private void TurnOffAlarm()
    {
        HandleVolumeChange(_minVolume);
    }

    private void HandleVolumeChange(float targetVolume)
    {
        StopCoroutineIfRunning();
        _coroutine = StartCoroutine(ProgressiveAdjustmentVolume(targetVolume));
    }

    private void StopCoroutineIfRunning()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator ProgressiveAdjustmentVolume(float targetVolume)
    {
        while (_audioSource.volume != targetVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetVolume, _volumeChangeRate * Time.deltaTime);
            
            yield return null;
        }
    }
}