import os
import joblib
import tensorflow as tf
import sys

def load_model(model_type):
    model_path = os.path.join(os.path.dirname(__file__), '..', 'RLP_ML_Engine', 'RLP_ML_Models')
    
    if not os.path.exists(model_path):
        raise FileNotFoundError(f"The directory {model_path} does not exist.")
    
    model_file = os.path.join(model_path, f'{model_type}.pkl')
    if not os.path.exists(model_file):
        raise FileNotFoundError(f"The model file {model_file}.pkl does not exist.")
    model = joblib.load(model_file)
    
    return model

if __name__ == "__main__":

    # Get parsed parameter from RLP_API
    arg = sys.argv[1]
    print(arg)

    # # Example usage:
    # model = load_model(arg)

    # # TODO - Generate Predictions of the model using the input data from RLP_API
    # pred = model.predict([[1, 2, 3, 4]])