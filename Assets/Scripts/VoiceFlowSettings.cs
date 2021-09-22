using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "VoiceFlowSetting", menuName = "Scriptable/VoiceFlow", order = 1)]
public class VoiceFlowSettings : ScriptableObject
{
    public string VoiceFlowApiKey;
    public string VoiceFlowVersionId;
}
