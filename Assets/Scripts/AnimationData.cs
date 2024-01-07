using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
[System.Serializable]
public class AnimationData
{

    [System.Serializable]
    public enum AnimType
    {
        Callback,
        Fade,
        Scale,
        Move,
        FillIn,
        Shake,
        Rotate
    }

    [System.Serializable]
    public struct floatData
    {
        public bool canUpdateStart;
        public float startValue;
        public float targetValue;
    }




    [System.Serializable]
    public struct VectorData
    {
        public bool canUpdateStart;
        public Vector3 startValue;
        public Vector3 targetValue;
    }



    [System.Serializable]
    public struct RotateData
    {
        public bool isLocal;
        public RotateMode rotateMode;
        public VectorData vectorData;
    }



    [System.Serializable]
    public struct ShakeData
    {
        [Range(0, 10)]
        public int strength;
        [Range(0, 20)]
        public int vibration;
        [Range(0, 90)]
        public int randomness;
    }

    [System.Serializable]
    public struct RefrenceList
    {
        public bool canUseRT;
        public bool canUseImage;
        public bool canUseText;
        public bool canUseTMPUi;
        public bool canUseRawImage;


        public List<RectTransform> targetObject_RT;
        public List<Image> targetObject_Image;
        public List<RawImage> targetObject_RawImage;
        public List<Text> targetObject_Text;
        public List<TextMeshProUGUI> targetObject_TMPUI;
    }











    public string title;

    public Ease ease = Ease.OutQuad;
    public float startAt;
    public float duration;
    public float delay;

    public RefrenceList refrenceList;

    public AnimType animType;
    public UnityEvent event_AnimType;   //in sequence will use some events as a callback 

    public floatData fadeData;
    public VectorData moveData;
    public RotateData rotateData;
    public VectorData scaleData;
    public floatData fillAmountData;
    public ShakeData shakeData;


    public bool canLoop;
    public int loopCycle;
    public LoopType loopType;

    public bool isActive_Start;
    public bool isActive_Update;
    public bool isActive_Complete;

    public UnityEvent event_OnStart;
    public UnityEvent event_OnUpdate;
    public UnityEvent event_OnComplete;

}
