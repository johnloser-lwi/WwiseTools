using System;
using System.Threading.Tasks;
using NUnit.Framework;
using WwiseTools.Objects;
using WwiseTools.Utils;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void CastingTest()
        {
            WwiseObject sound = new WwiseObject("TestObject", "null", "Sound");
            WwiseObject soundBank = new WwiseObject("TestObject", "null", "SoundBank");
            WwiseObject switchContainer = new WwiseObject("TestObject", "null", "SwitchContainer");
            WwiseObject musicSwitchContainer = new WwiseObject("TestObject", "null", "MusicSwitchContainer");


            Assert.That(sound.AsContainer().Valid);
            Assert.That(sound.AsVoice().Valid);
            Assert.That(soundBank.AsSoundBank().Valid);
            Assert.That(switchContainer.AsContainer().Valid);
            Assert.That(switchContainer.AsSwitchContainer().Valid);
            Assert.That(!switchContainer.AsMusicSwitchContainer().Valid);
            Assert.That(musicSwitchContainer.AsMusicSwitchContainer().Valid);
            Assert.That(musicSwitchContainer.AsContainer().Valid);
        }
        
    }
}