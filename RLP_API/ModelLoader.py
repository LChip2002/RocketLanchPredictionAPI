import os
import joblib
import tensorflow as tf
import sys
import json
import pandas as pd
from sklearn.preprocessing import LabelEncoder

class WeatherParams:
    def __init__(self, params):
        self.rain = params["Rain"]
        self.showers = params["Showers"]
        self.snowfall = params["Snowfall"]
        self.cloud_cover = params["CloudCover"]
        self.cloud_cover_low = params["CloudCoverLow"]
        self.cloud_cover_mid = params["CloudCoverMid"]
        self.cloud_cover_high = params["CloudCoverHigh"]
        self.visibility = params["Visibility"]
        self.temperature = params["Temperature"]
        self.wind_speed_10m = params["WindSpeed10m"]
        self.wind_speed_80m = params["WindSpeed80m"]
        self.wind_speed_120m = params["WindSpeed120m"]
        self.wind_speed_180m = params["WindSpeed180m"]
        self.temperature_80m = params["Temperature80m"]
        self.temperature_120m = params["Temperature120m"]
        self.temperature_180m = params["Temperature180m"]

class RocketParams:
    def __init__(self, params):
        self.to_thrust = params["ToThrust"]
        self.launch_mass = params["LaunchMass"]
        self.rocket_length = params["RocketLength"]
        self.rocket_diameter = params["RocketDiameter"]
        self.successful_rocket_launches = params["SuccessfulRocketLaunches"]
        self.failed_rocket_launches = params["FailedRocketLaunches"]

# Load the model from the file system
def load_model(model_type):
    # Load the model from the file system
    model_path = os.path.abspath(os.path.join(os.getcwd(), '..', 'RLP_ML_Engine', 'RLP_ML_Models'))
    
    if not os.path.exists(model_path):
        raise FileNotFoundError(f"The directory {model_path} does not exist.")
    
    model_file = os.path.join(model_path, f'{model_type}_model.pkl')
    if not os.path.exists(model_file):
        raise FileNotFoundError(f"The model file {model_file}_model.pkl does not exist.")
    model = joblib.load(model_file)
    
    return model

if __name__ == "__main__":

    try:
        # Get parsed parameter from RLP_API
        arg = sys.argv[1]

        # Deserialize the json string to a dictionary
        params = json.loads(arg)

        # Checks if the model type is null or empty
        if params["ModelType"] is None or params["ModelType"] == "":
            raise ValueError("Model type cannot be null or empty.")

        # Load selected model
        model = load_model(params["ModelType"])

        # # Create an empty dataframe
        input_data = pd.DataFrame()
        
        # Check that the weather parameters are not null
        if params["WeatherParams"] is not None:

            # Map the weather params to the model input
            weather_params = WeatherParams(params["WeatherParams"])

            # Add the weather parameters to the input_data dataframe
            input_data["Rain"] = [weather_params.rain]
            input_data["Showers"] = [weather_params.showers]
            input_data["Snowfall"] = [weather_params.snowfall]
            input_data["CloudCover"] = [weather_params.cloud_cover]
            input_data["CloudCoverLow"] = [weather_params.cloud_cover_low]
            input_data["CloudCoverMid"] = [weather_params.cloud_cover_mid]
            input_data["CloudCoverHigh"] = [weather_params.cloud_cover_high]
            input_data["Visibility"] = [weather_params.visibility]
            input_data["Temperature"] = [weather_params.temperature]
            input_data["WindSpeed10m"] = [weather_params.wind_speed_10m]
            input_data["WindSpeed80m"] = [weather_params.wind_speed_80m]
            input_data["WindSpeed120m"] = [weather_params.wind_speed_120m]
            input_data["WindSpeed180m"] = [weather_params.wind_speed_180m]
            input_data["Temperature80m"] = [weather_params.temperature_80m]
            input_data["Temperature120m"] = [weather_params.temperature_120m]
            input_data["Temperature180m"] = [weather_params.temperature_180m]

        # Check that the rocket parameters are not null
        if params["RocketParams"] is not None:

            # Map the weather params to the model input
            rocket_params = RocketParams(params["RocketParams"])

            # Add the rocket parameters to the input_data dataframe
            input_data["ToThrust"] = [rocket_params.to_thrust]
            input_data["LaunchMass"] = [rocket_params.launch_mass]
            input_data["RocketLength"] = [rocket_params.rocket_length]
            input_data["RocketDiameter"] = [rocket_params.rocket_diameter]
            input_data["SuccessfulRocketLaunches"] = [rocket_params.successful_rocket_launches]
            input_data["FailedRocketLaunches"] = [rocket_params.failed_rocket_launches]

        # Checks if any parameters have been added to the input data
        if input_data.empty:
            raise ValueError("No parameters have been added to the input data.")
        
        # Replace any NaN values with 0
        input_data.fillna(0, inplace=True)

        # Generate Predictions of the model using the input data from RLP_API
        pred = model.predict(input_data).astype("int32")

        # Interpret the prediction
        le = LabelEncoder()
        le.fit(["Launch Failure", "Launch Success"])
        output = le.inverse_transform(pred)

        # Remove the brackets from the output
        output = output[0]

        # Find the start of the JSON object
        json_start = output.find('{')

        # Extract only the JSON part
        response_json = output[json_start:].strip()

        # Create response object
        response = {
            "Prediction": output,
            "Status": "Success",
            "Message": "Prediction was successful"
        }

        # Convert the response object to a JSON string
        response_json = json.dumps(response)

        # Add the prediction object to the response and outputs to the console or API
        print(response_json)

    except Exception as e:
        print(f"Error: {e}")