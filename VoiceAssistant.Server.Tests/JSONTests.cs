using NUnit.Framework;
using System.Text;

namespace VoiceAssistant.Server.Tests
{
    public class JSONTests
    {
        [Test]
        public void ReadRootProperty__WHEN__PassedCorrectJson__THEN__ReturnsValueFromProperty()
        {
            var json = Encoding.UTF8.GetBytes(@"{ ""name"": ""foo"" }");

            var value = JSON.ReadRootProperty(json, "name");

            Assert.AreEqual("foo", value);
        }

        [Test]
        public void Serialize__WHEN__PassedExampleObject__THEN__RetursProperString()
        {
            var car = new CarDto() { Weight = 2300, Color = "red" };

            var value = Encoding.UTF8.GetString(JSON.Serialize(car));
            
            Assert.AreEqual(@"{""color"":""red"",""weight"":2300}", value);
        }

        [Test]
        public void Deserialize__WHEN_PassedCorrectJSON__THEN__ReturnsCorrectObject()
        {
            var json = Encoding.UTF8.GetBytes(@"{""color"":""red"",""weight"":2300}");

            var dto = JSON.Deserialize<CarDto>(json);

            Assert.AreEqual(2300, dto.Weight);
            Assert.AreEqual("red", dto.Color);
        }

        private class CarDto
        {
            public string Color { get; set; }
            public int Weight { get; set; }
        }
    }
}