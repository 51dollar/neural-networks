using System.Drawing;

namespace NeuralNetworks
{
    public class PictureConverter
    {
        public int Boundary { get; set; } = 128;
        public int Height { get; set; }
        public int Width { get; set; }
        public double[] Convert(string path)
        {

            var result = new List<double>();

            var image = new Bitmap(path);
            var resuzeImage = new Bitmap(image, new Size(20, 20));
            Height = resuzeImage.Height;
            Width = resuzeImage.Width;

            for (int y = 0; y < resuzeImage.Height; y++)
            {
                for (int x = 0; x < resuzeImage.Width; x++)
                {
                    var pixel = resuzeImage.GetPixel(x, y);
                    var value = Brightness(pixel);
                    result.Add(value);
                }
            }

            return result.ToArray();
        }

        private double Brightness(Color pixel)
        {
            var result = 0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B;
            return result < Boundary ? 0 : 1;
        }

        public void Save(string path, List<int> pixels)
        {
            var image = new Bitmap(Width, Height);
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var color = pixels[y * Width + x] == 1 ? Color.White : Color.Black;
                    image.SetPixel(x, y, color);
                }
            }

            image.Save(path);
        }
    }
}
