using UnityEditor;
using UnityEngine;

namespace Unity.Animations.SpringBones
{
    namespace Inspector
    {
        public class AngleLimitPropertyInfo : PropertyInfo
        {
            public AngleLimitPropertyInfo(string newName, string labelText)
                : base(newName, labelText)
            {
                minSlider = new FloatSlider(L10n.Tr("Lower Limit"), 0f, -180f);
                maxSlider = new FloatSlider(L10n.Tr("Upper Limit"), 0f, 180f);
            }

            public override void Show()
            {
                GUILayout.Space(14f);

                GUILayout.BeginVertical("box");

                var propertyIterator = serializedProperty.Copy();

                if (propertyIterator.NextVisible(true))
                {
                    EditorGUILayout.PropertyField(propertyIterator, label, true, null);
                }

                SerializedProperty minProperty = null;
                SerializedProperty maxProperty = null;
                if (propertyIterator.NextVisible(true))
                {
                    minProperty = propertyIterator.Copy();
                }

                if (propertyIterator.NextVisible(true))
                {
                    maxProperty = propertyIterator.Copy();
                }

                if (minProperty != null
                    && maxProperty != null)
                {
                    const float SubSpacing = 3f;
                    GUILayout.Space(SubSpacing);
                    var minChanged = minSlider.Show(minProperty);
                    GUILayout.Space(SubSpacing);
                    var maxChanged = maxSlider.Show(maxProperty);
                    GUILayout.Space(SubSpacing);
                    GUILayout.BeginHorizontal();

                    updateValuesTogether = GUILayout.Toggle(updateValuesTogether, L10n.Tr("Uniform"));
                    if (updateValuesTogether)
                    {
                        if (minChanged)
                        {
                            maxProperty.floatValue = -minProperty.floatValue;
                        }
                        else if (maxChanged)
                        {
                            minProperty.floatValue = -maxProperty.floatValue;
                        }
                    }

                    if (GUILayout.Button(L10n.Tr("Uniform Lower Limit")))
                    {
                        maxProperty.floatValue = -minProperty.floatValue;
                    }

                    if (GUILayout.Button(L10n.Tr("Uniform Upper Limit")))
                    {
                        minProperty.floatValue = -maxProperty.floatValue;
                    }

                    if (GUILayout.Button(L10n.Tr("Inversion")))
                    {
                        var minValue = minProperty.floatValue;
                        minProperty.floatValue = -maxProperty.floatValue;
                        maxProperty.floatValue = -minValue;
                    }

                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
            }

            private FloatSlider minSlider;
            private FloatSlider maxSlider;
            private bool updateValuesTogether = false;
        }
    }
}