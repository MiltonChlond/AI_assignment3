using UnityEngine;

public class NeuralNetwork
{
    public int inputSize;
    public int hiddenSize;
    public int outputSize;

    public float[,] w1; //weights between input to hidden
    public float[,] w2; //weigths between hideen to output

    public float[] b1; // bias for hidden
    public float[] b2; //bias for output

    public NeuralNetwork(int inputSize, int hiddenSize, int outputSize)
    {
        this.inputSize = inputSize;
        this.hiddenSize = hiddenSize;
        this.outputSize = outputSize;

        w1 = InitRandomMatrix(inputSize, hiddenSize);
        w2 = InitRandomMatrix(hiddenSize, outputSize);

        b1 = new float[hiddenSize];
        b2 = new float[outputSize];
    }

    public float[] Run(float[] inputs)
    {
        float[] hidden = new float[hiddenSize];
        float[] outputLayer = new float[outputSize];

        for(int i = 0;  i < hiddenSize; i++)
        {
            float activation = b1[i]; //activation = hur mycket neuronen aktiveras, startar med endast bias
            for(int j = 0; j < inputSize; j++)
            {
                activation += inputs[j] * w1[j, i];
            }

            hidden[i] = Mathf.Max(0, activation);
        }

        for(int i = 0; i < outputSize; i++)
        {
            float activation = b2[i];
            for(int j = 0;j < hiddenSize; j++)
            {
                activation += hidden[j] * w2[j, i];
            }

            outputLayer[i] = Mathf.Max(0, activation);
        }

        return outputLayer; 
    }

    float[,] InitRandomMatrix(int x, int y)
    {
        float[,] matrix = new float[x, y];
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                matrix[i, j] = Random.Range(-1, 1);
            }
        }
        return matrix;
    }
}
