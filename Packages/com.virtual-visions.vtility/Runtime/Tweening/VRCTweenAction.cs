
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;

namespace VirtualVisions.VTility
{

    public enum TweenTrigger
    {
        Start,
        Enable,
        EventCall,
    }
    
    [RequireComponent(typeof(VRCTweenActionUdon))]
    public class VRCTweenAction : MonoBehaviour
    {
        public TweenTrigger activation;
        [Min(0)] public float duration = 1;
        [Tooltip("For infinite loops, set to -1.")]
        public int loopCount;
        public VRCTweenLoopType loopType;
        public List<TweenAction> tweens;

        public void PopulateUdon()
        {
            VRCTweenActionUdon udon = GetComponent<VRCTweenActionUdon>();
            if (!udon) return;

            List<TweenType> type = new List<TweenType>();
            List<TweenEase> ease = new List<TweenEase>();
            List<bool> useStartValue = new List<bool>();

            List<Vector3[]> pathPoints = new List<Vector3[]>();
            List<TweenPathType> pathTypes = new List<TweenPathType>();
            List<bool> closePaths = new List<bool>();
            List<int> resolutions = new List<int>();
            List<string> colorNames = new List<string>();
            List<string> floatNames = new List<string>();

            List<Transform> transforms = new List<Transform>();
            List<GameObject> gameObjects = new List<GameObject>();
            List<Graphic> graphics = new List<Graphic>();
            List<CanvasGroup> canvasGroups = new List<CanvasGroup>();
            List<Slider> sliders = new List<Slider>();
            List<RectTransform> rectTransforms = new List<RectTransform>();
            List<Renderer> renderers = new List<Renderer>();
            List<Light> lights = new List<Light>();
            List<AudioSource> audioSources = new List<AudioSource>();
            List<SpriteRenderer> sprites = new List<SpriteRenderer>();
            List<float> startFloats = new List<float>();

            List<Vector2> startVector2s = new List<Vector2>();
            List<Vector3> startVector3s = new List<Vector3>();
            List<Color> startColors = new List<Color>();

            List<float> endFloats = new List<float>();
            List<Vector2> endVector2s = new List<Vector2>();
            List<Vector3> endVector3s = new List<Vector3>();
            List<Color> endColors = new List<Color>();

            foreach (TweenAction tween in tweens)
            {
                type.Add(tween.type);
                ease.Add((TweenEase)tween.ease);

                useStartValue.Add(tween.useStartValue);
                pathPoints.Add(tween.pathPoints);
                pathTypes.Add((TweenPathType)tween.pathType);
                closePaths.Add(tween.closePath);
                resolutions.Add(tween.resolution);
                colorNames.Add(tween.colorName);
                floatNames.Add(tween.floatName);

                transforms.Add(tween.transform);
                gameObjects.Add(tween.gameObject);
                graphics.Add(tween.graphic);
                canvasGroups.Add(tween.canvasGroup);
                sliders.Add(tween.slider);
                rectTransforms.Add(tween.rectTransform);
                renderers.Add(tween.renderer);
                lights.Add(tween.light);
                audioSources.Add(tween.audioSource);
                sprites.Add(tween.sprite);

                startFloats.Add(tween.startFloat);
                startVector2s.Add(tween.startVector2);
                startVector3s.Add(tween.startVector3);
                startColors.Add(tween.startColor);

                endFloats.Add(tween.endFloat);
                endVector2s.Add(tween.endVector2);
                endVector3s.Add(tween.endVector3);
                endColors.Add(tween.endColor);
            }

            udon.type = type.ToArray();
            udon.ease = ease.ToArray();

            udon.useStartValue = useStartValue.ToArray();
            udon.pathPoints = pathPoints.ToArray();
            udon.pathTypes = pathTypes.ToArray();
            udon.closePaths = closePaths.ToArray();
            udon.resolutions = resolutions.ToArray();
            udon.colorNames = colorNames.ToArray();
            udon.floatNames = floatNames.ToArray();

            udon.transforms = transforms.ToArray();
            udon.gameObjects = gameObjects.ToArray();
            udon.graphics = graphics.ToArray();
            udon.canvasGroups = canvasGroups.ToArray();
            udon.sliders = sliders.ToArray();
            udon.rectTransforms = rectTransforms.ToArray();
            udon.renderers = renderers.ToArray();
            udon.lights = lights.ToArray();
            udon.audioSources = audioSources.ToArray();
            udon.sprites = sprites.ToArray();

            udon.startFloats = startFloats.ToArray();
            udon.startVector2s = startVector2s.ToArray();
            udon.startVector3s = startVector3s.ToArray();
            udon.startColors = startColors.ToArray();
            udon.endFloats = endFloats.ToArray();
            udon.endVector2s = endVector2s.ToArray();
            udon.endVector3s = endVector3s.ToArray();
            udon.endColors = endColors.ToArray();

            udon.tweenCount = type.Count;
            udon.activation = activation;
            udon.duration = duration;
            udon.loopCount = loopCount;
            udon.loopType = loopType;
        }
    }
}