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


            Assert.That(sound.AsHierarchy().Valid);
            Assert.That(sound.AsVoice().Valid);
            Assert.That(soundBank.AsSoundBank().Valid);
            Assert.That(switchContainer.AsHierarchy().Valid);
            Assert.That(switchContainer.AsSwitchContainer().Valid);
            Assert.That(!switchContainer.AsMusicSwitchContainer().Valid);
            Assert.That(musicSwitchContainer.AsMusicSwitchContainer().Valid);
            Assert.That(musicSwitchContainer.AsHierarchy().Valid);
        }

        [Test]
        public async Task WwisePathBuilderTest()
        {
            WaapiLog.SetEnableInternalLog(false);
            
            var result = "\\Actor-Mixer Hierarchy\\<ActorMixer>TestMixer\\<SwitchContainer>TestSwitch\\<Sound>TestSound";
            var pureResult = "\\Actor-Mixer Hierarchy\\TestMixer\\TestSwitch\\TestSound";

            var builder = new WwisePathBuilder(new WwiseObject("", "", ""));
            
            builder.AppendHierarchy(WwiseObject.ObjectType.ActorMixer, "TestMixer");
            builder.AppendHierarchy(WwiseObject.ObjectType.SwitchContainer, "TestSwitch");
            
            // ActorMixer不能添加至SwitchContainer，期望值为false
            var res = builder.AppendHierarchy(WwiseObject.ObjectType.ActorMixer, "TestMixer2");
            
            builder.AppendHierarchy(WwiseObject.ObjectType.Sound, "TestSound");

            string builderResult = await builder.GetImportPathAsync();
            string builderPureResult = await builder.GetPurePathAsync();
            
            Assert.That(!res);
            Assert.That(builderResult == result);
            Assert.That(builderPureResult == pureResult);
        }
    }
}