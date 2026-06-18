using System;
using System.Linq;

namespace DeteksiBanjir.Services;

public class NeuralNetworkService
{
    private double[,] weightsInputHidden;
    private double[,] weightsHiddenOutput;
    private double[] biasHidden;
    private double[] biasOutput;

    private readonly int inputNodes = 3;  // WaterLevel, Rainfall, Discharge
    private readonly int hiddenNodes = 4;
    private readonly int outputNodes = 4; // No Alert, Yellow, Orange, Red

    public NeuralNetworkService()
    {
        InitializeWeights();
    }

    private void InitializeWeights()
    {
        // Fixed seed for deterministic results
        Random random = new Random(42);

        weightsInputHidden = new double[inputNodes, hiddenNodes];
        biasHidden = new double[hiddenNodes];
        for (int i = 0; i < inputNodes; i++)
        {
            for (int j = 0; j < hiddenNodes; j++)
            {
                weightsInputHidden[i, j] = random.NextDouble() * 2 - 1; // -1 to 1
            }
        }
        for (int i = 0; i < hiddenNodes; i++)
        {
            biasHidden[i] = random.NextDouble() * 2 - 1;
        }

        weightsHiddenOutput = new double[hiddenNodes, outputNodes];
        biasOutput = new double[outputNodes];
        for (int i = 0; i < hiddenNodes; i++)
        {
            for (int j = 0; j < outputNodes; j++)
            {
                weightsHiddenOutput[i, j] = random.NextDouble() * 2 - 1;
            }
        }
        for (int i = 0; i < outputNodes; i++)
        {
            biasOutput[i] = random.NextDouble() * 2 - 1;
        }
    }

    private double Sigmoid(double x)
    {
        return 1.0 / (1.0 + Math.Exp(-x));
    }

    private double[] Softmax(double[] x)
    {
        double max = x.Max();
        double[] exp = new double[x.Length];
        double sum = 0;
        for (int i = 0; i < x.Length; i++)
        {
            exp[i] = Math.Exp(x[i] - max);
            sum += exp[i];
        }
        for (int i = 0; i < x.Length; i++)
        {
            exp[i] /= sum;
        }
        return exp;
    }

    public (double RiskScore, string Category) Predict(double waterLevel, double rainfall, double discharge)
    {
        // Normalize inputs (naive normalization for demonstration)
        double[] inputs = new double[] 
        { 
            waterLevel / 10.0, 
            rainfall / 100.0, 
            discharge / 150.0 
        };

        // Feedforward
        // Hidden Layer
        double[] hiddenLayer = new double[hiddenNodes];
        for (int j = 0; j < hiddenNodes; j++)
        {
            double sum = biasHidden[j];
            for (int i = 0; i < inputNodes; i++)
            {
                sum += inputs[i] * weightsInputHidden[i, j];
            }
            hiddenLayer[j] = Sigmoid(sum);
        }

        // Output Layer
        double[] outputLayer = new double[outputNodes];
        for (int j = 0; j < outputNodes; j++)
        {
            double sum = biasOutput[j];
            for (int i = 0; i < hiddenNodes; i++)
            {
                sum += hiddenLayer[i] * weightsHiddenOutput[i, j];
            }
            outputLayer[j] = sum;
        }

        // Softmax Activation
        double[] probabilities = Softmax(outputLayer);

        // Find max probability index
        int maxIndex = 0;
        double maxProb = probabilities[0];
        for (int i = 1; i < outputNodes; i++)
        {
            if (probabilities[i] > maxProb)
            {
                maxProb = probabilities[i];
                maxIndex = i;
            }
        }

        string[] categories = { "No Alert", "Yellow Alert", "Orange Alert", "Red Alert" };
        
        // For risk score, we can use the probability of max or weighted average
        double riskScore = maxIndex * 3.33; // roughly 0, 3.33, 6.66, 10

        return (riskScore, categories[maxIndex]);
    }
}
