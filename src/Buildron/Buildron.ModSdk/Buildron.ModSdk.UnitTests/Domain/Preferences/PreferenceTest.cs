using NUnit.Framework;
using System;
using Buildron.Domain.Mods;

namespace Buildron.ModSdk.UnitTests
{
	[TestFixture]
	public class PreferenceTest
	{
		[Test]
		public void Constructor_KindString_DefaultValue()
		{
			var target = new Preference("String1", "String 1", PreferenceKind.String);
			Assert.AreEqual("String1", target.Name);
			Assert.AreEqual("String 1", target.Title);
			Assert.AreEqual(PreferenceKind.String, target.Kind);
			Assert.AreEqual("", target.DefaultValue);
		}

		[Test]
		public void Constructor_KindBool_DefaultValue()
		{
			var target = new Preference("Bool1", "Bool 1", PreferenceKind.Bool);
			Assert.AreEqual("Bool1", target.Name);
			Assert.AreEqual("Bool 1", target.Title);
			Assert.AreEqual(PreferenceKind.Bool, target.Kind);
			Assert.AreEqual(false, target.DefaultValue);
		}

		[Test]
		public void Constructor_KindFloat_DefaultValue()
		{
			var target = new Preference("Float1", "Float 1", PreferenceKind.Float);
			Assert.AreEqual("Float1", target.Name);
			Assert.AreEqual("Float 1", target.Title);
			Assert.AreEqual(PreferenceKind.Float, target.Kind);
			Assert.AreEqual(0f, target.DefaultValue);
		}

		[Test]
		public void Constructor_KindInt_DefaultValue()
		{
			var target = new Preference("Int1", "Int 1", PreferenceKind.Int);
			Assert.AreEqual("Int1", target.Name);
			Assert.AreEqual("Int 1", target.Title);
			Assert.AreEqual(PreferenceKind.Int, target.Kind);
			Assert.AreEqual(0, target.DefaultValue);
		}
	}
}