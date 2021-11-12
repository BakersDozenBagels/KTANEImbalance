using UnityEngine;
using Imbalance;
using System.Collections;

public class ImbalanceScript : MonoBehaviour
{
    [SerializeField]
    private TextMesh _textA, _textB, _buttonText;
    [SerializeField]
    private GameObject _cap;
    [SerializeField]
    private KMSelectable _button;
    [SerializeField]
    private KMBombModule _module;
    [SerializeField]
    private KMBombInfo _info;
    [SerializeField]
    private KMAudio _audio;

    private static int _idCounter;
    private int _chosenA, _chosenB, _solution, _id;
    private bool _isSolved;

    private void Start()
    {
        _id = ++_idCounter;
        _chosenA = Random.Range(0, 128);
        _chosenB = Random.Range(0, 128);
        _textA.text = _chosenA.ToImbalance();
        _textB.text = _chosenB.ToImbalance();
        Debug.LogFormat("[Imbalance #{0}] The top screen is displaying: \"{1}\"", _id, _textA.text);
        Debug.LogFormat("[Imbalance #{0}] The bottom screen is displaying: \"{1}\"", _id, _textB.text);

        _solution = _chosenA * _chosenB;
        Debug.LogFormat("[Imbalance #{0}] The expected solution is: \"{1}\"", _id, _solution);

        _button.OnInteract += () => Press();
        _button.OnInteractEnded += () => StartCoroutine(Animate(false));
    }

    private IEnumerator Animate(bool v)
    {
        if(v)
            _audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, _button.transform);
        else
            _audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonRelease, _button.transform);

        float time = Time.time;
        while(Time.time < time + 0.125f)
        {
            _cap.transform.localPosition = new Vector3(0f, v ? -0.55f * (Time.time - time) / 0.125f : -0.55f + (Time.time - time) / 0.25f, 0f);
            yield return null;
        }
        _cap.transform.localPosition = new Vector3(0f, v ? -0.55f : 0f, 0f);
    }

    private void Update()
    {
        _buttonText.text = ((int)_info.GetTime() % 10).ToString();
    }

    private bool Press()
    {
        StartCoroutine(Animate(true));
        if(_isSolved)
            return false;
        if(_textA.text.EndsWith(".«"))
            _textA.text = _buttonText.text;
        else
            _textA.text += _buttonText.text;

        if(!_solution.ToString().StartsWith(_textA.text))
        {
            Debug.LogFormat("[Imbalance #{0}] You entered \"{1}\", which was incorrect.", _id, _textA.text);
            _module.HandleStrike();
            _textA.text = _chosenA.ToImbalance();
        }
        if(_solution.ToString() == _textA.text)
        {
            Debug.LogFormat("[Imbalance #{0}] You entered \"{1}\", which was correct. Solved.", _id, _textA.text);
            _module.HandlePass();
            _isSolved = true;
            StartCoroutine(FadeOut());
        }

        return false;
    }

    private IEnumerator FadeOut()
    {
        float time = Time.time;
        while(Time.time - time < 1f)
        {
            _textA.color = new Color(1f, 1f, 1f, 1f - (Time.time - time));
            _textB.color = new Color(1f, 1f, 1f, 1f - (Time.time - time));
            _buttonText.color = new Color(1f, 1f, 1f, 1f - (Time.time - time));
            yield return null;
        }
        _textA.color = new Color(1f, 1f, 1f, 0f);
        _textB.color = new Color(1f, 1f, 1f, 0f);
        _buttonText.color = new Color(1f, 1f, 1f, 0f);
    }
}