
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;

namespace VirtualVisions.VTility
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VRCTweenActionUdon : UdonSharpBehaviour
    {

        [HideInInspector] public TweenTrigger activation;
        [HideInInspector] public int tweenCount;
        [HideInInspector] public float duration = 1;
        [HideInInspector] public VRCTweenLoopType loopType;
        [HideInInspector] public int loopCount;
        
        [HideInInspector] public TweenType[] type;
        [HideInInspector] public TweenEase[] ease;
        [HideInInspector] public bool[] useStartValue;
        
        // Traits
        [HideInInspector] public Vector3[][] pathPoints;
        [HideInInspector] public TweenPathType[] pathTypes;
        [HideInInspector] public bool[] closePaths;
        [HideInInspector] public int[] resolutions;
        [HideInInspector] public string[] colorNames;
        [HideInInspector] public string[] floatNames;
        
        // References
        [HideInInspector] public Transform[] transforms;
        [HideInInspector] public GameObject[] gameObjects;
        [HideInInspector] public Graphic[] graphics;
        [HideInInspector] public CanvasGroup[] canvasGroups;
        [HideInInspector] public Slider[] sliders;
        [HideInInspector] public RectTransform[] rectTransforms;
        [HideInInspector] public Renderer[] renderers;
        [HideInInspector] public Light[] lights;
        [HideInInspector] public AudioSource[] audioSources;
        [HideInInspector] public SpriteRenderer[] sprites;
        
        // Starts
        [HideInInspector] public float[] startFloats;
        [HideInInspector] public Vector2[] startVector2s;
        [HideInInspector] public Vector3[] startVector3s;
        [HideInInspector] public Color[] startColors;

        // Ends
        [HideInInspector] public float[] endFloats;
        [HideInInspector] public Vector2[] endVector2s;
        [HideInInspector] public Vector3[] endVector3s;
        [HideInInspector] public Color[] endColors;

        private bool _initialized;
        private VRCTweenHandle[] _handles;

        public VRCTweenEase VRCEase(int index) => (VRCTweenEase)ease[index];
        public VRCTweenPathType VRCPath(int index) => (VRCTweenPathType)pathTypes[index];


        private void OnEnable()
        {
            if (activation == TweenTrigger.Enable) _RunTween();
        }

        private void Start()
        {
            if (activation == TweenTrigger.Start) _RunTween();
        }

        private void Init()
        {
            _initialized = true;
            _handles = new VRCTweenHandle[tweenCount];
        }

        public void _RunTween()
        {
            if (!_initialized) Init();

            foreach (VRCTweenHandle handle in _handles)
            {
                handle.TryComplete();
            }
            
            for (int i = 0; i < tweenCount; i++)
            {
                VRCTweenHandle handle;

                switch (type[i])
                {
                    default:
                    case TweenType.Position:
                        if (useStartValue[i]) transforms[i].position = startVector3s[i];
                        handle = transforms[i].TweenPosition(endVector3s[i], duration, VRCEase(i));
                        break;
                    case TweenType.LocalPosition:
                        if (useStartValue[i]) transforms[i].localPosition = startVector3s[i];
                        handle = transforms[i].TweenLocalPosition(endVector3s[i], duration, VRCEase(i));
                        break;
                    case TweenType.Rotation:
                        if (useStartValue[i]) transforms[i].eulerAngles = startVector3s[i];
                        handle = transforms[i].TweenRotation(endVector3s[i], duration, VRCEase(i));
                        break;
                    case TweenType.LocalRotation:
                        if (useStartValue[i]) transforms[i].localEulerAngles = startVector3s[i];
                        handle = transforms[i].TweenLocalRotation(endVector3s[i], duration, VRCEase(i));
                        break;
                    case TweenType.Scale:
                        if (useStartValue[i]) transforms[i].localScale = startVector3s[i];
                        handle = transforms[i].TweenScale(endVector3s[i], duration, VRCEase(i));
                        break;
                    case TweenType.Path:
                        if (useStartValue[i]) transforms[i].position = pathPoints[i][0];
                        handle = gameObjects[i].TweenPath(
                            pathPoints[i],
                            duration,
                            VRCPath(i),
                            closePaths[i],
                            resolutions[i],
                            VRCEase(i));
                        break;
                    case TweenType.LocalPath:
                        if (useStartValue[i]) transforms[i].localPosition = pathPoints[i][0];
                        handle = gameObjects[i].TweenLocalPath(
                            pathPoints[i],
                            duration,
                            VRCPath(i),
                            closePaths[i],
                            resolutions[i],
                            VRCEase(i));
                        break;
                    case TweenType.GraphicColor:
                        if (useStartValue[i]) graphics[i].color = startColors[i];
                        handle = graphics[i].TweenColor(endColors[i], duration, VRCEase(i));
                        break;
                    case TweenType.GraphicFade:
                        if (useStartValue[i])
                        {
                            Color color = graphics[i].color;
                            color.a = startFloats[i];
                            graphics[i].color = color;
                        }
                        handle = graphics[i].TweenFade(endFloats[i], duration, VRCEase(i));
                        break;
                    case TweenType.CanvasGroupFade:
                        if (useStartValue[i]) canvasGroups[i].alpha = startFloats[i];
                        handle = canvasGroups[i].TweenFade(endFloats[i], duration, VRCEase(i));
                        break;
                    case TweenType.SliderValue:
                        if (useStartValue[i]) sliders[i].SetValueWithoutNotify(startFloats[i]);
                        handle = sliders[i].TweenValue(endFloats[i], duration, VRCEase(i));
                        break;
                    case TweenType.AnchorPosition:
                        if (useStartValue[i]) rectTransforms[i].anchoredPosition = startVector2s[i];
                        handle = rectTransforms[i].TweenAnchorPos(endVector2s[i], duration, VRCEase(i));
                        break;
                    case TweenType.SizeDelta:
                        if (useStartValue[i]) rectTransforms[i].sizeDelta = startVector2s[i];
                        handle = rectTransforms[i].TweenSizeDelta(endVector2s[i], duration, VRCEase(i));
                        break;
                    case TweenType.RendererColor:
                        Renderer rendColor = renderers[i];
                        string colorName = colorNames[i];
                        if (useStartValue[i])
                        {
                            MaterialPropertyBlock block = new MaterialPropertyBlock();
                            rendColor.GetPropertyBlock(block);
                            block.SetColor(colorName, startColors[i]);
                            rendColor.SetPropertyBlock(block);
                        }

                        if (string.IsNullOrEmpty(colorName))
                            handle = rendColor.TweenColor(endColors[i], duration, VRCEase(i));
                        else
                            handle = rendColor.TweenColor(colorName, endColors[i], duration, VRCEase(i));
                        break;
                    case TweenType.RendererFloat:
                        Renderer rendFloat = renderers[i];
                        string floatName = floatNames[i];
                        if (useStartValue[i])
                        {
                            MaterialPropertyBlock block = new MaterialPropertyBlock();
                            rendFloat.GetPropertyBlock(block);
                            block.SetFloat(floatName, startFloats[i]);
                            rendFloat.SetPropertyBlock(block);
                        }
                        handle = rendFloat.TweenFloat(floatName, endFloats[i], duration, VRCEase(i));
                        break;
                    case TweenType.LightIntensity:
                        if (useStartValue[i]) lights[i].intensity = startFloats[i];
                        handle = lights[i].TweenIntensity(endFloats[i], duration, VRCEase(i));
                        break;
                    case TweenType.LightColor:
                        if (useStartValue[i]) lights[i].color = startColors[i];
                        handle = lights[i].TweenColor(endColors[i], duration, VRCEase(i));
                        break;
                    case TweenType.Volume:
                        if (useStartValue[i]) audioSources[i].volume = startFloats[i];
                        handle = audioSources[i].TweenVolume(endFloats[i], duration, VRCEase(i));
                        break;
                    case TweenType.Pitch:
                        if (useStartValue[i]) audioSources[i].pitch = startFloats[i];
                        handle = audioSources[i].TweenPitch(endFloats[i], duration, VRCEase(i));
                        break;
                    case TweenType.SpriteColor:
                        if (useStartValue[i]) sprites[i].color = startColors[i];
                        handle = sprites[i].TweenColor(endColors[i], duration, VRCEase(i));
                        break;
                    case TweenType.SpriteFade:
                        SpriteRenderer sprite = sprites[i];
                        if (useStartValue[i])
                        {
                            Color color = sprite.color;
                            color.a = startFloats[i];
                            sprite.color = color;
                        }
                        handle = sprites[i].TweenFade(endFloats[i], duration, VRCEase(i));
                        break;
                }

                if (loopCount != 0) handle = handle.SetLoops(loopCount, loopType);
                _handles[i] = handle;
            }
        }
        
        
    }
}