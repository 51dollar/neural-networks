namespace NeuralNetworks.Tests
{
    [TestClass()]
    public class PictureConverterTests
    {
        [TestMethod()]
        public void ConvertTest()
        {
            var converter = new PictureConverter();
            var inputs = converter.Convert(@"C:\Users\borus\source\repos\NeuralNetworks\NeuralNetworksTests\Images\Parasitized.png");
            converter.Save("d:\\image.png", inputs);
        }
    }
}