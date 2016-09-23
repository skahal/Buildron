using UnityEngine;
using UnityEngine.UI;
using Skahal.Logging;
using Zenject;
using Buildron.Domain.Mods;
using Buildron.Infrastructure.PreferencesProxies;
using System.Collections.Generic;
using System.Globalization;

public class ModsPanelController : MonoBehaviour
{
    #region Fields
    [Inject]
    private ISHLogStrategy m_log;

    [Inject]
    private IModLoader m_modLoader;

    private List<GameObject> m_modUIGameObjects = new List<GameObject>();

    private Vector3 m_firstControlPosition;
    #endregion

    #region Properties
    public Dropdown ModsDropdown;
    private ModInstanceInfo m_currentMod;
    public GameObject PreferenceTextTemplate;
    public GameObject PreferenceBoolTemplate;
    public GameObject PreferenceStringTemplate;
    public GameObject PreferenceIntTemplate;
    public GameObject PreferenceFloatTemplate;
    public float DistanceBetweenControls = 30;
    #endregion

    #region Methods
    private void Start()
    {
        PreferenceTextTemplate.SetActive(false);
        PreferenceBoolTemplate.SetActive(false);
        PreferenceStringTemplate.SetActive(false);
        PreferenceIntTemplate.SetActive(false);
        PreferenceFloatTemplate.SetActive(false);

        m_firstControlPosition = ModsDropdown.gameObject.transform.position;
        m_firstControlPosition.x = 100;
        m_firstControlPosition.y -= 200;

        foreach (var mod in m_modLoader.LoadedMods)
        {
            ModsDropdown.options.Add(new Dropdown.OptionData(mod.Info.Name));
        }

        ModsDropdown.value = 0;
        SelectMod(0);

        ModsDropdown.onValueChanged.AddListener((index) =>
        {
            SelectMod(index);
        });
    }

    void SelectMod(int index)
    {
        while (m_modUIGameObjects.Count > 0)
        {
            var go = m_modUIGameObjects[0];
            m_modUIGameObjects.RemoveAt(0);

            Object.Destroy(go);
        }

        m_currentMod = m_modLoader.LoadedMods[index];

        m_log.Debug("Mod selected '{0}'", m_currentMod.Info.Name);

        var preferencesProxy = m_currentMod.Preferences as ModPreferencesProxy;
        var preferences = preferencesProxy.GetRegisteredPreferences();

        m_log.Debug("Preferences: {0}", preferences.Length);

        if (preferences.Length == 0)
        {
            CreatePreferenceUI(null, 0);
        }
        else
        {
            for (int i = 0; i < preferences.Length; i++)
            {
                CreatePreferenceUI(preferences[i], i);
            }
        }
    }

    private void CreatePreferenceUI(Preference preference, int index = 0)
    {
        var preferenceName = preference == null ? "No preferences" : preference.Name;
        var line = new GameObject(preferenceName);
        var group = line.AddComponent<HorizontalLayoutGroup>();
        group.padding.left = -70;
        group.padding.right = -200;

        var sizeFilter = line.AddComponent<ContentSizeFitter>();
        sizeFilter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

        line.transform.SetParent(transform);
        m_modUIGameObjects.Add(line);

        var label = CreateUIElement<Text>(line.transform, PreferenceTextTemplate, preferenceName, index);

        if(preference == null)
        {
            label.text = preferenceName;
            return;
        }

        label.text = preference.Title;        
        var preferences = m_currentMod.Preferences;

        switch (preference.Kind)
        {
            case PreferenceKind.Bool:
                var boolControl = CreateUIElement<Toggle>(line.transform, PreferenceBoolTemplate, preference.Name, index);
                boolControl.isOn = preferences.GetValue<bool>(preference.Name);
                boolControl.onValueChanged.AddListener((v) =>
                {
                    preferences.SetValue<bool>(preference.Name, v);
                });                
                break;

            case PreferenceKind.Float:
                var floatControl = CreateUIElement<InputField>(line.transform, PreferenceFloatTemplate, preference.Name, index);
                floatControl.text = preferences.GetValue<float>(preference.Name).ToString(CultureInfo.InvariantCulture);
                floatControl.onValueChanged.AddListener((v) =>
                {
                    preferences.SetValue<float>(preference.Name, string.IsNullOrEmpty(v) ? 0f : System.Convert.ToSingle(v, CultureInfo.InvariantCulture));
                });                
                break;

            case PreferenceKind.Int:
                var intControl = CreateUIElement<InputField>(line.transform, PreferenceFloatTemplate, preference.Name, index);
                intControl.text = preferences.GetValue<int>(preference.Name).ToString(CultureInfo.InvariantCulture);
                intControl.onValueChanged.AddListener((v) =>
                {
                    preferences.SetValue<int>(preference.Name, string.IsNullOrEmpty(v) ? 0 : System.Convert.ToInt32(v, CultureInfo.InvariantCulture));
                });                
                break;

            case PreferenceKind.String:
                var stringControl = CreateUIElement<InputField>(line.transform, PreferenceStringTemplate, preference.Name, index);
                stringControl.text = preferences.GetValue<string>(preference.Name) ?? string.Empty;
                stringControl.onValueChanged.AddListener((v) =>
                {
                    preferences.SetValue<string>(preference.Name, v);
                });                
                break;
        }
    }

    private TScript CreateUIElement<TScript>(Transform parent, GameObject template, string elementName, int index)
        where TScript : MonoBehaviour
    {
        var obj = Object.Instantiate(template);
        obj.transform.SetParent(parent, true);
        parent.transform.position = m_firstControlPosition + new Vector3(0, index * -DistanceBetweenControls, 0);
        var control = obj.GetComponentInChildren<TScript>();
        control.enabled = true;
        control.name = elementName;
                
        obj.SetActive(true);

        return control;
    }
    #endregion
}