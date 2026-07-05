using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;

namespace VirtualVisions.VTility
{
    public enum TweenType
    {
        Position,
        LocalPosition,
        Rotation,
        LocalRotation,
        Scale,
        Path,
        LocalPath,
        GraphicColor,
        GraphicFade,
        CanvasGroupFade,
        SliderValue,
        AnchorPosition,
        SizeDelta,
        RendererColor,
        RendererFloat,
        LightIntensity,
        LightColor,
        Volume,
        Pitch,
        SpriteColor,
        SpriteFade,
    }
    
    [Serializable]
    public class TweenAction
    {
        // Settings
        public TweenType type;
        public VRCTweenEase ease = VRCTweenEase.OutSine;
        public bool useStartValue;
        
        // References
        public Transform transform;
        public GameObject gameObject;
        public Graphic graphic;
        public CanvasGroup canvasGroup;
        public Slider slider;
        public RectTransform rectTransform;
        public Renderer renderer;
        public Light light;
        public AudioSource audioSource;
        public SpriteRenderer sprite;
        
        // Traits
        public Vector3[] pathPoints;
        public VRCTweenPathType pathType;
        public bool closePath;
        public int resolution;
        
        [Tooltip("The name of the color property. Leave empty to default to the base color of the renderer.")]
        public string colorName;
        public string floatName;
        
        // Starts
        public float startFloat;
        public Vector2 startVector2;
        public Vector3 startVector3;
        public Color startColor;

        // Ends
        public float endFloat;
        public Vector2 endVector2;
        public Vector3 endVector3;
        public Color endColor;
        
    }
}