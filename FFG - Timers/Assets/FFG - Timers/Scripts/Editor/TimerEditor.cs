using UnityEditor;
using FFG;


namespace FFG_Editors
{
    [CustomEditor(typeof(TimerComponent))]
    public class TimerEditor : EditorUtils<TimerComponent>
    {
        public override void CustomOnGUI()
        {
            SerializedProperty shouldStartOnAwake = GetProperty("_shouldStartOnAwake");
            SerializedProperty timerMode = GetProperty("_timerMode");
            SerializedProperty duration = GetProperty("_duration");
            SerializedProperty repeatCount = GetProperty("_repeatCount");
            SerializedProperty timerCompleteCallback = GetProperty("_onTimerCompleteCallback");
            SerializedProperty timerStartCallback = GetProperty("_onTimerStartCallback");

            Heading("Timer Settings");
            Space(10);
            PropertyField(shouldStartOnAwake, "Should start timer upon awake : ", "Set this as true if you want the timer to start automatically");
            PropertyField(timerMode, "Timer Mode : ", "Select the mode this timer has to be run.");
            PropertyField(timerStartCallback, "On timer start callback :", "Callback upon the start of timer");

            switch ((TimerMode)timerMode.enumValueIndex)
            {
                case TimerMode.Stopwatch:
                    break;
                case TimerMode.Repeating:
                    {
                        Space(15);
                        PropertyField(duration, "Timer duration : ", "Duration between timer repeat");
                        PropertyField(repeatCount, "Total repeat count : ", "Total number of repeatation");
                        Space(20);
                        PropertyField(timerCompleteCallback, "On timer complete callback : ", "Callback on each lap time");
                    }
                    break;
                case TimerMode.Countdown:
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
