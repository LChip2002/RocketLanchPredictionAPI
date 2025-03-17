using System.ComponentModel;

namespace RLP_API.Models
{
    public enum ModelType
    {
        NaiveBayes, // Naive Bayes = 0
        DecisionTree, // Decision Tree = 1
        NeuralNetwork, // Custom Neural Network = 2
        KNeighbours, // K-Nearest Neighbors = 3
        RandomForest, // Random Forest = 4
    }

    // Checks the user input and returns the status in the form of what to expect in the DB
    public static class ModelTypeFunctions
    {
        public static string GetModelType(ModelType status)
        {
            switch (status)
            {
                case ModelType.NaiveBayes:
                    return "Naive Bayes";
                case ModelType.DecisionTree:
                    return "Decision Tree";
                case ModelType.NeuralNetwork:
                    return "Custom Neural Network";
                case ModelType.KNeighbours:
                    return "K-Nearest Neighbors";
                case ModelType.RandomForest:
                    return "Random Forest";
                default:
                    return "Linear Regression";
            }
        }
    }
}