using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Buildron.Domain.Mods;
using Buildron.Infrastructure.CameraProxies;
using Buildron.Infrastructure.PreferencesProxies;
using NUnit.Framework;
using UnityEngine;

namespace Buildron.Infrastructure.FunctionalTests.PreferenceProxies
{
    [Category("Buildron.Infrastructure")]
    [Category("Unity")]
    public class ModPreferenceProxyTest
    {
        [Test]
        public void Register_Preferences_Registered()
        {
            var info = new ModInfo("test");
            var target = new ModPreferencesProxy(info);
            var preferences = new Preference[]
            {
                new Preference("P1", "Preference one", PreferenceKind.String, "one"),
                new Preference("P2", "Preference two", PreferenceKind.String, "two"),
            };

            target.Register(preferences);
            var actual = target.GetRegisteredPreferences();
            Assert.AreEqual(preferences, actual);
        }

        [Test]
        public void GetValue_NotRegisteredPreference_Exception()
        {
            var info = new ModInfo("test");
            var target = new ModPreferencesProxy(info);
            var preferences = new Preference[]
            {
                new Preference("P1", "Preference one", PreferenceKind.String, "one"),
            };

			target.Register(preferences);

            Assert.Catch<ArgumentException>(() => target.GetValue<string>("two"));
        }

        [Test]
        public void GetValue_RegisteredPreferenceDefaultInvalidKind_Exception()
        {
            var info = new ModInfo("test");
            var target = new ModPreferencesProxy(info);
            var preferences = new Preference[]
            {
				new Preference("P1", "Preference one", PreferenceKind.Int, 1),
            };
			target.Register(preferences);
			target.RemoveValue<int> ("P1");
		
            Assert.Catch<InvalidCastException>(() => target.GetValue<string>("P1"));
        }

        [Test]
        public void GetValue_RegisteredPreferenceBool_Value()
        {
            var info = new ModInfo("test");
            var target = new ModPreferencesProxy(info);
            var preferences = new Preference[]
            {
                new Preference("P1", "Preference one", PreferenceKind.Bool, true),
            };
			target.Register(preferences);
			target.RemoveValue<bool> ("P1");

            Assert.AreEqual(true, target.GetValue<bool>("P1"));
            target.SetValue("P1", false);
            Assert.AreEqual(false, target.GetValue<bool>("P1"));
        }

        [Test]
        public void GetValue_RegisteredPreferenceFloat_Value()
        {
            var info = new ModInfo("test");
            var target = new ModPreferencesProxy(info);            
            var preferences = new Preference[]
            {
                new Preference("P1", "Preference one", PreferenceKind.Float, 1.1f),
            };
			target.Register(preferences);
			target.RemoveValue<float> ("P1");

            Assert.AreEqual(1.1f, target.GetValue<float>("P1"));
            
            target.SetValue("P1", 2.2f);
            Assert.AreEqual(2.2f, target.GetValue<float>("P1"));
        }

        [Test]
        public void GetValue_RegisteredPreferenceInt_Value()
        {
            var info = new ModInfo("test");
            var target = new ModPreferencesProxy(info);
            var preferences = new Preference[]
            {
                new Preference("P1", "Preference one", PreferenceKind.Int, 1),
            };
			target.Register(preferences);
			target.RemoveValue<int> ("P1");

            Assert.AreEqual(1, target.GetValue<int>("P1"));

            target.SetValue("P1", 2);
            Assert.AreEqual(2, target.GetValue<int>("P1"));
        }

        [Test]
        public void GetValue_RegisteredPreferenceString_Value()
        {
            var info = new ModInfo("test");
            var target = new ModPreferencesProxy(info);
            var preferences = new Preference[]
            {
                new Preference("P1", "Preference one", PreferenceKind.String, "test1"),
            };
			target.Register(preferences);
			target.RemoveValue<string> ("P1");

            Assert.AreEqual("test1", target.GetValue<string>("P1"));
            target.SetValue("P1", "test2");
            Assert.AreEqual("test2", target.GetValue<string>("P1"));
        }

        [Test]
        public void SetValue_NotRegisteredPreference_Exception()
        {
            var info = new ModInfo("test");
            var target = new ModPreferencesProxy(info);
            var preferences = new Preference[]
            {
                new Preference("P1", "Preference one", PreferenceKind.String, "one"),
            };
			target.Register(preferences);

            Assert.Catch<ArgumentException>(() => target.SetValue("two", "2"));
        }

        [Test]
        public void RemoveValue_NotRegisteredPreference_Exception()
        {
            var info = new ModInfo("test");
            var target = new ModPreferencesProxy(info);
            var preferences = new Preference[]
            {
                new Preference("P1", "Preference one", PreferenceKind.String, "one"),
            };
			target.Register(preferences);

            Assert.Catch<ArgumentException>(() => target.RemoveValue<string>("two"));
        }
    }
}
