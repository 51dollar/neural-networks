namespace NeuralNetworks.Tests
{
    [TestClass()]
    public class NeuralNetworkTests
    {
        [TestMethod()]
        public void FeedForwardTest()
        {
            var outputs = new double[] { 0, 0, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1 };
            var inputs = new double[,]
            {
                // Результат - Болен - 1
                //             Здоров - 0

                // Tемпература - Т
                // Возраст - А
                // Курит - S
                // Правильно питается - F
                //T  A  S  F
                { 0, 0, 0, 0 },
                { 0, 0, 0, 1 },
                { 0, 0, 1, 0 },
                { 0, 0, 1, 1 },
                { 0, 1, 0, 0 },
                { 0, 1, 0, 1 },
                { 0, 1, 1, 0 },
                { 0, 1, 1, 1 },
                { 1, 0, 0, 0 },
                { 1, 0, 0, 1 },
                { 1, 0, 1, 0 },
                { 1, 0, 1, 1 },
                { 1, 1, 0, 0 },
                { 1, 1, 0, 1 },
                { 1, 1, 1, 0 },
                { 1, 1, 1, 1 }
            };

            var topology = new Topology(4, 1, 0.1, 2);
            var neuralNetwork = new NeuralNetwork(topology);
            var difference = neuralNetwork.Learn(outputs, inputs, 100000);

            var results = new List<double>();
            for (int i = 0; i < outputs.Length; i++)
            {
                var row = NeuralNetwork.GetRow(inputs, i);
                var res = neuralNetwork.Predict(row).Output;
                results.Add(res);
            }

            for (int i = 0; i < results.Count; i++)
            {
                var expected = Math.Round(outputs[i], 3);
                var actual = Math.Round(results[i], 3);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        public void DatasetTest()
        {
            var outputs = new List<double>();
            var inputs = new List<double[]>();

            using (var sr = new StreamReader("heart.csv"))
            {
                var header = sr.ReadToEnd();

                while (!sr.EndOfStream)
                {
                    var row = sr.ReadLine();
                    var values = row.Split(',').Select(v => Convert.ToDouble(v.Replace(".", ","))).ToList();
                    var output = values.Last();
                    var input = values.Take(values.Count - 1).ToArray();

                    outputs.Add(output);
                    inputs.Add(input);
                }
            }

            var inputSignals = new double[inputs.Count, inputs[0].Length];
            for (int i = 0; i < inputSignals.GetLength(0); i++)
            {
                for (var j = 0;  j < inputSignals.GetLength(1); j++)
                {
                    inputSignals[i, j] = inputs[i][j];
                }
            }

            var topology = new Topology(outputs.Count, 1, 1, outputs.Count / 2);
            var neuralNetwork = new NeuralNetwork(topology);
            var difference = neuralNetwork.Learn(outputs.ToArray(), inputSignals, 10);

            var results = new List<double>();
            for (int i = 0; i < outputs.Count; i++)
            {
                var res = neuralNetwork.Predict(inputs[i]).Output;
                results.Add(res);
            }

            for (int i = 0; i < results.Count; i++)
            {
                var expected = Math.Round(outputs[i], 2);
                var actual = Math.Round(results[i], 2);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        public void RecognizeImages()
        {
            var size = 1000;
            var parasitizedPath = @"C:\Users\borus\Downloads\cell_images\Parasitized\";
            var unparasitizedPath = @"C:\Users\borus\Downloads\cell_images\Uninfected\";

            var converter = new PictureConverter();
            var testParasitizedImageInput = converter.Convert(@"C:\Users\borus\source\repos\NeuralNetworks\NeuralNetworksTests\Images\Parasitized.png");
            var testUnparasitizedImageInput = converter.Convert(@"C:\Users\borus\source\repos\NeuralNetworks\NeuralNetworksTests\Images\Unparasitized.png");

            var topology = new Topology(testParasitizedImageInput.Length, 1, 0.1, testParasitizedImageInput.Length / 2);
            var neuralNetwork = new NeuralNetwork(topology);

            double[,] parasitezedInputs = GetData(parasitizedPath, converter, testParasitizedImageInput, size);
            neuralNetwork.Learn(new double[] { 1 }, parasitezedInputs, 1);

            double[,] unparasitezedInputs = GetData(unparasitizedPath, converter, testParasitizedImageInput, size);
            neuralNetwork.Learn(new double[] { 0 }, unparasitezedInputs, 1);

            var par = neuralNetwork.Predict(testParasitizedImageInput.Select(t => (double)t).ToArray());
            var unpar = neuralNetwork.Predict(testUnparasitizedImageInput.Select(t => (double)t).ToArray());

            Assert.AreEqual(1, Math.Round(par.Output, 2));
            Assert.AreEqual(0, Math.Round(unpar.Output, 2));
        }

        private static double[,] GetData(string parasitizedPath, PictureConverter converter, double[] testImageInput, int size)
        {
            var images = Directory.GetFiles(parasitizedPath);
            var result = new double[size, testImageInput.Length];
            for (int i = 0; i < size; i++)
            {
                var image = converter.Convert(images[i]);
                for (int j = 0; j < image.Length; j++)
                {
                    result[i, j] = image[j];
                }
            }

            return result;
        }
    }
}