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
        _house.HouseBreakInDetected += IncreaseVolume;
        _house.HouseBreakOutDetected += DecreaseVolume;
    }

    private void OnDisable()
    {
        _house.HouseBreakInDetected -= IncreaseVolume;
        _house.HouseBreakOutDetected -= DecreaseVolume;
    }

    private void IncreaseVolume()
    {
        if (_isIncreasingVolume == false)
        {
            StopCoroutine(ProgressiveDecreaseVolume());
            _isDecreasingVolume = false;

            StartCoroutine(ProgressiveIncreaseVolume());
            _audioSource.Play();
        }
    }

    private void DecreaseVolume()
    {
        if (_isDecreasingVolume == false)
        {
            StopCoroutine(ProgressiveIncreaseVolume());
            _isIncreasingVolume = false;

            StartCoroutine(ProgressiveDecreaseVolume());
        }
    }

    private IEnumerator ProgressiveIncreaseVolume()
    {
        _isIncreasingVolume = true;

        while (_isIncreasingVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _maxVolume, _volumeChange);

            yield return _wait;
        }
    }

    private IEnumerator ProgressiveDecreaseVolume()
    {
        _isDecreasingVolume = true;

        while (_isDecreasingVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _minVolume, _volumeChange);
            
            yield return _wait;
        }
    }
}