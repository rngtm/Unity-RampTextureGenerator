namespace RampTextureGenerator
{
    using UnityEngine;

    public class ColorRampObject : ScriptableObject
    {
        [SerializeField] private Gradient _gradient = new Gradient
        {
            colorKeys = new GradientColorKey[] { new GradientColorKey(Color.black, 0f), new GradientColorKey(Color.white, 1f), }
        };
        [SerializeField, Range(-1, 1)] private float _hueShift = 0f;
        [SerializeField, Range(-1, 1)] private float _saturateShift = 0f;
        [SerializeField, Range(-1, 1)] private float _valueShift = 0f;
        [SerializeField, HideInInspector] private Texture2D _texture = null;
        public Texture2D Texture => _texture;

        public Gradient Gradient
        {
            get => _gradient;
            set => _gradient = value;
        }

        public void Initialize(Texture2D texture)
        {
            _texture = texture;
        }


        public void ApplyGradient()
        {
            if (_texture == null) return;
            for (int xIndex = 0; xIndex < _texture.width; xIndex++)
            {
                float time = (float) xIndex / (_texture.width - 1);
                var color = _gradient.Evaluate(time);
                float a = color.a;
                Color.RGBToHSV(color, out float h, out float s, out float v);
                h = (h + _hueShift + 1f) % 1f;
                s = Mathf.Clamp01(s + _saturateShift);
                v = Mathf.Clamp01(v + _valueShift);
                color = Color.HSVToRGB(h, s, v);
                color.a = a;

                for (int yIndex = 0; yIndex < _texture.height; yIndex++)
                {
                    _texture.SetPixel(xIndex, yIndex, color);
                }
            }

            _texture.Apply();
        }
    }
}