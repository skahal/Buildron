using Buildron.Domain;
using NUnit.Framework;

namespace Buildron.Domain.UnitTests
{
    [Category("Buildron.Domain")]
    public class BuildFilterTest
    {

        [Test]
        public void KeyWord_NotStartsWithMinus_Contains()
        {
            var target = new BuildFilter();
            target.KeyWord = "test";
            Assert.AreEqual(KeyWordFilterType.Contains, target.KeyWordType);
        }

        [Test]
        public void KeyWord_StartsWithMinus_DoesNotContains()
        {
            var target = new BuildFilter();
            target.KeyWord = "-test";
            Assert.AreEqual(KeyWordFilterType.DoesNotContains, target.KeyWordType);
        }

        [Test]
        public void IsEmpty_KeywordOrStatus_False()
        {
            var target = new BuildFilter();
            target.FailedEnabled = false;
            Assert.IsFalse(target.IsEmpty);

            target.FailedEnabled = true;
            target.KeyWord = "test";
            Assert.IsFalse(target.IsEmpty);
            
            target.KeyWord = null;
            target.QueuedEnabled = false;
            Assert.IsFalse(target.IsEmpty);

            target.QueuedEnabled = true;
            target.RunningEnabled = false;
            Assert.IsFalse(target.IsEmpty);

            target.RunningEnabled = true;
            target.SuccessEnabled = false;
            Assert.IsFalse(target.IsEmpty);
        }

        [Test]
        public void IsEmpty_NoKeywordOrStatus_True()
        {
            var target = new BuildFilter();            
            Assert.IsTrue(target.IsEmpty);
        }
    }
}