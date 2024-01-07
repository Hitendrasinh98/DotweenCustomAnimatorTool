using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// this is main class for Sequence animations.
/// </summary>
public class CustomAnimatorTool : MonoBehaviour
{
    //list of animation tweens which will be puted back into one sequence
    [SerializeField] private List<AnimationData> mSequenceDatas = new List<AnimationData>();
    private Sequence mSequence;

    private void Awake()
    {
        //setting alll tweens into one seqence
        Set_Sequence();
    }

    #region Sequence Handler

    [ContextMenu("Set Sequence")]
    public void Set_Sequence()
    {
        if (mSequence != null)
        {
            mSequence.Rewind(); //so  all the elements will get at their inital state before killing sequence
            mSequence.Kill();   //will kill sequnece
        }

        mSequence = DOTween.Sequence();
        mSequence.SetAutoKill(false); //we will keep it allive and reuse this sequence.

        for (int i = 0; i < mSequenceDatas.Count; i++)
        {
            Set_StartingPoint(mSequenceDatas[i]); //setting starting state of elements
            Add_TweenInSequnce(mSequenceDatas[i], mSequence); //creating tween and adding into sequence
        }

        mSequence.OnComplete(() => Debug.Log("Sequence Completed"));
        mSequence.Pause();

    }


    [ContextMenu("Test Animation")]
    public void Start_Animation()
    {
        if (mSequence == null)
            Set_Sequence();

        mSequence.Restart();
    }


    [ContextMenu("Rewind")]
    public void Set_SequenceRewindState()
    {
        if (mSequence != null)
            mSequence.Rewind();
    }

    #endregion

    #region Tween Handlers

    public void Set_StartingPoint(AnimationData _data)
    {
        if (_data.animType == AnimationData.AnimType.Callback)
            return;

        if ((_data.animType == AnimationData.AnimType.Fade && !_data.fadeData.canUpdateStart) ||
            (_data.animType == AnimationData.AnimType.FillIn && !_data.fillAmountData.canUpdateStart) ||
            (_data.animType == AnimationData.AnimType.Move && !_data.moveData.canUpdateStart) ||
            (_data.animType == AnimationData.AnimType.Rotate && !_data.rotateData.vectorData.canUpdateStart) ||
            (_data.animType == AnimationData.AnimType.Scale && !_data.scaleData.canUpdateStart))
            return;

        if (_data.refrenceList.canUseRT)
        {
            foreach (RectTransform targetObject in _data.refrenceList.targetObject_RT)
            {
                ApplyCommonSetup(targetObject);
            }
        }

        if (_data.refrenceList.canUseImage)
        {
            foreach (Image targetObject in _data.refrenceList.targetObject_Image)
            {
                ApplyCommonSetup(targetObject.rectTransform);
                switch (_data.animType)
                {
                    case AnimationData.AnimType.Fade:
                        Color color = targetObject.color;
                        color.a = _data.fadeData.startValue;
                        targetObject.color = color;
                        break;

                    case AnimationData.AnimType.FillIn:
                        targetObject.fillAmount = _data.fillAmountData.startValue;
                        break;
                    default:
                        Debug.Log("Default loaded");
                        break;
                }
            }
        }

        if (_data.refrenceList.canUseRawImage)
        {
            foreach (RawImage targetObject in _data.refrenceList.targetObject_RawImage)
            {
                ApplyCommonSetup(targetObject.rectTransform);
                switch (_data.animType)
                {
                    case AnimationData.AnimType.Fade:
                        Color color = targetObject.color;
                        color.a = _data.fadeData.startValue;
                        targetObject.color = color;
                        break;
                }
            }
        }

        if (_data.refrenceList.canUseText)
        {
            foreach (Text targetObject in _data.refrenceList.targetObject_Text)
            {
                ApplyCommonSetup(targetObject.rectTransform);
                if (_data.animType == AnimationData.AnimType.Fade)
                {
                    Color color = targetObject.color;
                    color.a = _data.fadeData.startValue;
                    targetObject.color = color;
                }
            }
        }

        if (_data.refrenceList.canUseTMPUi)
        {
            foreach (TextMeshProUGUI targetObject in _data.refrenceList.targetObject_TMPUI)
            {
                ApplyCommonSetup(targetObject.rectTransform);
                if (_data.animType == AnimationData.AnimType.Fade)
                {
                    Color color = targetObject.color;
                    color.a = _data.fadeData.startValue;
                    targetObject.color = color;
                }
            }
        }


        void ApplyCommonSetup(RectTransform targetRT)
        {
            switch (_data.animType)
            {
                case AnimationData.AnimType.Scale:
                    targetRT.localScale = _data.scaleData.startValue;
                    break;
                case AnimationData.AnimType.Move:
                    targetRT.anchoredPosition = _data.moveData.startValue;
                    break;
                case AnimationData.AnimType.Rotate:
                    if (_data.rotateData.isLocal)
                        targetRT.localRotation = Quaternion.Euler(_data.rotateData.vectorData.startValue);
                    else
                        targetRT.rotation = Quaternion.Euler(_data.rotateData.vectorData.startValue);
                    break;
            }
        }

    }

    public void Add_TweenInSequnce(AnimationData _data, Sequence _sequence)
    {
        if (_data.animType == AnimationData.AnimType.Callback)
        {
            if (_data.event_AnimType == null)
            {
                Debug.Log("[CustomAnimatorTool] no Event Assigned on Callback event");
                return;
            }

            _sequence.InsertCallback(_data.startAt, () => _data.event_AnimType?.Invoke());
            return;
        }

        if (_data.refrenceList.canUseRT)
        {
            foreach (RectTransform targetObject in _data.refrenceList.targetObject_RT)
            {
                ApplyCommonTween(targetObject);
            }
        }

        if (_data.refrenceList.canUseImage)
        {
            foreach (Image targetObject in _data.refrenceList.targetObject_Image)
            {
                ApplyCommonTween(targetObject.rectTransform);
                switch (_data.animType)
                {
                    case AnimationData.AnimType.Fade:
                        Tween tween = targetObject.DOFade(_data.fadeData.targetValue, _data.duration);
                        SetUp_Tween(_data, tween);
                        _sequence.Insert(_data.startAt, tween);
                        break;
                    case AnimationData.AnimType.FillIn:
                        Tween tweenFill = targetObject.DOFade(_data.fadeData.targetValue, _data.duration);
                        SetUp_Tween(_data, tweenFill);
                        _sequence.Insert(_data.startAt, tweenFill);
                        break;
                }

            }
        }



        if (_data.refrenceList.canUseRawImage)
        {
            foreach (RawImage targetObject in _data.refrenceList.targetObject_RawImage)
            {
                ApplyCommonTween(targetObject.rectTransform);
                switch (_data.animType)
                {
                    case AnimationData.AnimType.Fade:
                        Tween tween = targetObject.DOFade(_data.fadeData.targetValue, _data.duration);
                        SetUp_Tween(_data, tween);
                        _sequence.Insert(_data.startAt, tween);
                        break;
                    case AnimationData.AnimType.FillIn:
                        Tween tweenFill = targetObject.DOFade(_data.fadeData.targetValue, _data.duration);
                        SetUp_Tween(_data, tweenFill);
                        _sequence.Insert(_data.startAt, tweenFill);
                        break;
                }

            }
        }

        if (_data.refrenceList.canUseText)
        {
            foreach (Text targetObject in _data.refrenceList.targetObject_Text)
            {
                ApplyCommonTween(targetObject.rectTransform);
                switch (_data.animType)
                {
                    case AnimationData.AnimType.Fade:
                        Tween tween = targetObject.DOFade(_data.fadeData.targetValue, _data.duration);
                        SetUp_Tween(_data, tween);
                        _sequence.Insert(_data.startAt, tween);
                        break;
                }
            }
        }

        if (_data.refrenceList.canUseTMPUi)
        {
            foreach (TextMeshProUGUI targetObject in _data.refrenceList.targetObject_TMPUI)
            {
                ApplyCommonTween(targetObject.rectTransform);
                switch (_data.animType)
                {
                    case AnimationData.AnimType.Fade:
                        Tween tween = targetObject.DOFade(_data.fadeData.targetValue, _data.duration);
                        SetUp_Tween(_data, tween);
                        _sequence.Insert(_data.startAt, tween);
                        break;
                }

            }
        }



        void ApplyCommonTween(RectTransform targetRT)
        {
            Tween tween = null;
            switch (_data.animType)
            {
                case AnimationData.AnimType.Scale:
                    tween = targetRT.DOScale(_data.scaleData.targetValue, _data.duration);
                    break;
                case AnimationData.AnimType.Move:
                    tween = targetRT.DOAnchorPos(_data.moveData.targetValue, _data.duration);
                    break;
                case AnimationData.AnimType.Shake:
                    tween = targetRT.DOShakeAnchorPos(_data.duration, _data.shakeData.strength, _data.shakeData.vibration, _data.shakeData.randomness);
                    break;
                case AnimationData.AnimType.Rotate:
                    tween = _data.rotateData.isLocal
                            ? targetRT.DOLocalRotate(_data.rotateData.vectorData.targetValue,
                                _data.duration, _data.rotateData.rotateMode)
                            : targetRT.DORotate(_data.rotateData.vectorData.targetValue, _data.duration,
                                _data.rotateData.rotateMode);
                    break;
            }
            if (tween == null)
                return;
            SetUp_Tween(_data, tween);
            _sequence.Insert(_data.startAt, tween);
        }

    }


    void SetUp_Tween(AnimationData _data, Tween _tween)
    {
        if (_tween == null)
            return;

        _tween.SetEase(_data.ease).SetDelay(_data.delay);

        if (_data.canLoop)
            _tween.SetLoops(_data.loopCycle, _data.loopType);

        if (_data.isActive_Start)
            _tween.OnStart(() => _data.event_OnStart?.Invoke());
        if (_data.isActive_Update)
            _tween.OnUpdate(() => _data.event_OnUpdate?.Invoke());
        if (_data.isActive_Complete)
            _tween.OnComplete(() => _data.event_OnComplete?.Invoke());

    }



    #endregion


}
