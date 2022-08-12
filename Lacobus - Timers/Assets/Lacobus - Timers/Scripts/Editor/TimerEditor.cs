using UnityEditor;
using Lacobus.Timers;
using UnityEngine;


namespace Lacobus_Editor.Timers
{
    public class EditorUtils<TType> : Editor where TType : Object
    {
        public TType Root => (TType)target;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            CustomOnGUI();
            serializedObject.ApplyModifiedProperties();
        }

        public virtual void CustomOnGUI() { }

        public SerializedProperty GetProperty(string propertyName)
            => serializedObject.FindProperty(propertyName);

        public void PropertyField(SerializedProperty property)
            => PropertyField(property, "", "");

        public void PropertyField(SerializedProperty property, string propertyName, string tooltip)
            => EditorGUILayout.PropertyField(property, new GUIContent(propertyName, tooltip));

        public void Info(string info, MessageType type = MessageType.Info)
            => EditorGUILayout.HelpBox(info, type);

        public void PropertySlider(SerializedProperty property, float min, float max, string label)
            => EditorGUILayout.Slider(property, min, max, label);

        public void Space(float val)
            => GUILayout.Space(val);

        public void Heading(string label)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField(label, style, GUILayout.ExpandWidth(true));
        }
        public bool Button(string content)
            => GUILayout.Button(content);

        public bool Button(string content, float height)
            => GUILayout.Button(content, GUILayout.Height(height));

        public bool Button(string content, float height, float width)
            => GUILayout.Button(content, GUILayout.Height(height), GUILayout.Width(width));

        public int DropdownList(string label, int index, string[] choices)
            => EditorGUILayout.Popup(label, index, choices);

        public void BeginVertical()
            => EditorGUILayout.BeginVertical();

        public void EndVertical()
            => EditorGUILayout.EndVertical();

        public void BeginHorizontal()
            => EditorGUILayout.BeginHorizontal();

        public void EndHorizontal()
            => EditorGUILayout.EndHorizontal();

        public void Label(string labelContent)
            => EditorGUILayout.LabelField(labelContent);
    }


    [CustomEditor(typeof(TimerComponent))]
    public class TimerEditor : EditorUtils<TimerComponent>
    {
        public override void CustomOnGUI()
        {
            SerializedProperty shouldStartOnAwake = GetProperty("_shouldStartOnAwake");
            SerializedProperty timerMode = GetProperty("_timerType");
            SerializedProperty duration = GetProperty("_duration");
            SerializedProperty repeatCount = GetProperty("_repeatCount");
            SerializedProperty timerCompleteCallback = GetProperty("_onTimerCompleteCallback");
            SerializedProperty timerStartCallback = GetProperty("_onTimerStartCallback");
            SerializedProperty shouldUseUnscaledTime = GetProperty("_shouldUseUnscaledTime");

            Heading("Timer Settings");
            Space(10);
            PropertyField(shouldStartOnAwake, "Should start timer upon awake : ", "Set this as true if you want the timer to start automatically");
            PropertyField(timerMode, "Timer type : ", "Select the mode this timer has to be run.");
            PropertyField(shouldUseUnscaledTime, "Should use unscaled time ?", "If true unscaled time will be used");
            PropertyField(timerStartCallback, "On timer start callback :", "Callback upon the start of timer");

            switch ((TimerFactory.TimerType)timerMode.enumValueIndex)
            {
                case TimerFactory.TimerType.EndlessTimer:
                    break;
                case TimerFactory.TimerType.RepeatingTimer:
                    {
                        Space(15);
                        PropertyField(duration, "Timer duration : ", "Duration between timer repeat");
                        PropertyField(repeatCount, "Total repeat count : ", "Total number of repeatation");
                        Space(20);
                        PropertyField(timerCompleteCallback, "On timer complete callback : ", "Callback on each lap time");
                    }
                    break;
                case TimerFactory.TimerType.DefaultTimer:
                    {
                        Space(15);
                        PropertyField(duration, "Timer duration : ", "Duration before invoking the event");
                        Space(20);
                        PropertyField(timerCompleteCallback, "On timer complete callback : ", "Callback upon timer complete");
                    }
                    break;
            }
        }
    }
}
