namespace RLP_API.Models
{
    public enum ModelType
    {
        NaiveBayes = 0,
        DecisionTree = 1,
        NeuralNetwork = 2,
        KNeighbours = 3,
        RandomForest = 4,
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
                    return "K-Nearest Neighbours";
                case ModelType.RandomForest:
                    return "Random Forest";
                default:
                    return "Linear Regression";
            }
        }
    }
}