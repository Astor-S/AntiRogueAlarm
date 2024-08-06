using System.Collections;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
[RequireComponent(typeof(House))]

public class Alarm : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _volume;

    private AudioSource _audioSource;
    private House _house;
    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private float _volumeChange = 0.1f;
    private float _volumeChangeInterval = 1f;
    private float _maxVolume = 1f;
    private float _minVolume = 0f;
    private bool _isIncreasingVolume;
    private bool _isDecreasingVolume;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _house = GetComponent<House>();
        _wait = new WaitForSeconds(_volumeChangeInterval);
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

    public void IncreaseVolume()
    {
        AdjustVolume(_volumeChange);
    }

    public void DecreaseVolume()
    {
        AdjustVolume(-_volumeChange);
    }

    private void TurnOnAlarm()
    {
        IncreaseVolume();
    }

    private void TurnOffAlarm()
    {
        DecreaseVolume();
    }

    private void AdjustVolume(float volumeChange)
    {
        if (volumeChange > 0)
        {
            if (_isIncreasingVolume == false)
            {
                StopCoroutineIfRunning();

                _isDecreasingVolume = false;

                _coroutine = StartCoroutine(ProgressiveAdjustmentVolume(volumeChange, _maxVolume));
                _audioSource.Play();
            }
        }
        else if (volumeChange < 0)
        {
            if (_isDecreasingVolume == false)
            {
                StopCoroutineIfRunning();

                _isIncreasingVolume = false;

                _coroutine = StartCoroutine(ProgressiveAdjustmentVolume(volumeChange, _minVolume));
            }
        }
    }

    private void StopCoroutineIfRunning()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator ProgressiveAdjustmentVolume(float volumeChange, float targetVolume)
    {
        if (volumeChange > 0)
            _isIncreasingVolume = true;
        else if (volumeChange < 0)
            _isDecreasingVolume = true;

        while ((_isIncreasingVolume && volumeChange > 0) || (_isDecreasingVolume && volumeChange < 0))
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, targetVolume, Mathf.Abs(volumeChange));

            yield return _wait;
        }
    }
}