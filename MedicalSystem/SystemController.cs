﻿using NeuralNetworks;

namespace MedicalSystem
{
    public class SystemController
    {
        public NeuralNetwork DataNetwork { get; }
        public NeuralNetwork ImageNetwork { get; }

        public SystemController()
        {
            var dataTopology = new Topology(14, 1, 0.1, 7);
            DataNetwork = new NeuralNetwork(dataTopology);

            var imageTopology = new Topology(400, 1, 0.1, 200);
            ImageNetwork = new NeuralNetwork(imageTopology);
        }
    }
}
