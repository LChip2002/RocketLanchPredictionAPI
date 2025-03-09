
from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report, confusion_matrix, accuracy_score, log_loss, precision_score, recall_score, f1_score
from sklearn.preprocessing import LabelEncoder, StandardScaler
import psycopg2
import pandas as pd
import numpy as np
import uuid
import json
import pickle

# Import TensorFlow and Keras
import tensorflow as tf
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense

# Import classification models that already exist
from sklearn.neighbors import KNeighborsClassifier
from sklearn.tree import DecisionTreeClassifier
from sklearn.ensemble import RandomForestClassifier
from sklearn.naive_bayes import GaussianNB
import os

# Predicition results class
class LaunchPrediction:
    def __init__(self, params_weather, params_rocket, result_id, results):
        self.prediction_id = str(uuid.uuid4())  # Generate a unique ID for each prediction
        self.params_weather = params_weather
        self.params_rocket = params_rocket
        self.result_id = result_id
        self.results = results
        
# Linked table that contains the results of each model       
class PredictionResults:
    def __init__(self, result_id, model_name, model_prediction, accuracy, loss, precision, recall, f1_score):
        self.result_id = result_id
        self.model_name = model_name
        self.model_prediction = model_prediction
        self.accuracy = accuracy
        self.loss = loss
        self.precision = precision
        self.recall = recall
        self.f1_score = f1_score


# Creates a model for predicting the success rate of rocket launches
def success_rate_model(X_train):
    # Define model
    model = Sequential([
    Dense(32, activation='relu', input_shape=(X_train.shape[1],)),
    Dense(16, activation='relu'),
    Dense(1, activation='sigmoid')  # Binary classification
    ])

    # Compile model
    model.compile(optimizer='adam', loss='binary_crossentropy', metrics=['accuracy'])

    return model

# Get the Launch data from the Postgres DB
def get_launch_data():
    try:
        # Connect to your postgres DB
        # TODO - Add this to app settings or environment variables
        conn = psycopg2.connect(
            dbname="postgres",
            user="postgres",
            password="postgres",
            host="localhost",
            port="5432"
        )
        
        # Create a cursor object
        cursor = conn.cursor()
        
        # Execute a query
        cursor.execute("SELECT * FROM launch_entries")  # Adjust the query based on your table structure
        
        # Retrieve query results
        launch_data = cursor.fetchall()

        # Define column headers based on your table structure
        column_headers = [desc[0] for desc in cursor.description]
        
        # Convert the fetched data into a list of dictionaries
        launch_data = [dict(zip(column_headers, row)) for row in launch_data]
        
        # Close the cursor and connection
        cursor.close()
        conn.close()
        
        return launch_data
    
    except Exception as e:
        print(f"Error: {e}")
        return None

# Save the trained model to a file
def save_trained_model(model, model_name):
    
    # Save the trained model to a file using pickle

    # Ensure the directory exists
    directory = os.path.join(os.path.dirname(__file__), "RLP_ML_Models")
    os.makedirs(directory, exist_ok=True)

    # Creates a file with the model
    with open(os.path.join(directory, f"{model_name}_model.pkl"), 'wb') as file:
        pickle.dump(model, file)

    print(f"Model {model_name} saved successfully")

def store_results(launch_predictions):
    
    # Connect to your postgres DB
    conn = psycopg2.connect(
        dbname="postgres",
        user="postgres",
        password="postgres",
        host="localhost",
        port="5432"
    )

    # Create a cursor object
    cursor = conn.cursor()
    
    # SQL to create tables for storing launch predictions and results of each model
    CREATE_LAUNCH_PREDICTIONS_TABLE = """
    CREATE TABLE IF NOT EXISTS launch_predictions (
        prediction_id UUID PRIMARY KEY,
        params_weather JSONB,
        params_rocket JSONB,
        result_id UUID,
        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
        FOREIGN KEY (result_id) REFERENCES prediction_results(result_id)
    )
    """

    CREATE_PREDICTION_RESULTS_TABLE = """
    CREATE TABLE IF NOT EXISTS prediction_results (
        result_id UUID PRIMARY KEY,
        model_name TEXT NOT NULL,
        model_prediction TEXT,
        accuracy FLOAT,
        loss FLOAT,
        precision FLOAT,
        recall FLOAT,
        f1_score FLOAT
    )
    """

    cursor.execute(CREATE_PREDICTION_RESULTS_TABLE)
    cursor.execute(CREATE_LAUNCH_PREDICTIONS_TABLE)
    
    print("Storing prediction results")

    # Ingest the resultant data into the DB
    for prediction in launch_predictions:
        try:
            cursor.execute(
                """
                INSERT INTO prediction_results (result_id, model_name, model_prediction, accuracy, loss, precision, recall, f1_score)
                VALUES (%s, %s, %s, %s, %s, %s, %s, %s)
                """,
                (prediction.result_id, prediction.results.model_name, prediction.results.model_prediction, prediction.results.accuracy, prediction.results.loss, prediction.results.precision, prediction.results.recall, prediction.results.f1_score)
            )

            cursor.execute(
                """
                INSERT INTO launch_predictions (prediction_id, params_weather, params_rocket, result_id)
                VALUES (%s, %s, %s, %s)
                """,
                (prediction.prediction_id, json.dumps(prediction.params_weather), json.dumps(prediction.params_rocket), prediction.result_id)
            )
        except Exception as e:
            print(f"Error: {e}")
            continue

        
    
    # Commit the transaction
    conn.commit()
    

if __name__ == "__main__":

    launch_data = get_launch_data()
    if launch_data:
        print("Launch data retrieved successfully")
    else:
        print("Failed to retrieve launch data")

    # Convert the retrieved data into a pandas DataFrame
    launch_df = pd.DataFrame(launch_data)

    # Fills any null values with 0
    launch_df.fillna(0, inplace=True)

    # Replaces partial failure as failure
    launch_df['Status'] = launch_df['Status'].replace('Launch was a Partial Failure', 'Launch Failure')

    # Split the data into features and labels
    X = np.array([launch_df['Temperature'], launch_df['Rain'], launch_df['Showers'], launch_df['Snowfall'], launch_df['CloudCover']
                 , launch_df['CloudCoverLow'], launch_df['CloudCoverMid'], launch_df['CloudCoverHigh'], launch_df['Visibility']
                 , launch_df['WindSpeed10m'], launch_df['WindSpeed80m'], launch_df['WindSpeed120m'], launch_df['WindSpeed180m']
                 , launch_df['Temperature80m'], launch_df['Temperature120m'], launch_df['Temperature180m']
                 , launch_df['ToThrust'], launch_df['LaunchMass'], launch_df['RocketLength'], launch_df['RocketDiameter']
                 , launch_df['SuccessfulRocketLaunches'], launch_df['FailedRocketLaunches']]).T  # Features
    
    y = launch_df['Status']  # Label for the status of the launch


    # Transforms the string ground truth into a numerical value
    le = LabelEncoder()
    y = le.fit_transform(y)


    # Split the data into training and testing sets for model training
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=42)

    # Normalize features
    scaler = StandardScaler()
    X_train = scaler.fit_transform(X_train)
    X_test = scaler.transform(X_test)

    # Training the model on the launch data training parameters
    model = success_rate_model(X_train)

    model.summary()

    modelList = [model, KNeighborsClassifier(n_neighbors=4), DecisionTreeClassifier(), RandomForestClassifier(), GaussianNB()]
    modelNames = ["Custom Neural Network", "K-Nearest Neighbors", "Decision Tree", "Random Forest", "Naive Bayes"]

    # Create a list of objects to store the results of each model that can be stored in the DB
    launch_predictions = []

    # Iterate through each model in the list, train it and evaluate it
    for i in range(len(modelList)):

        print(f"Training {modelNames[i]}")
        
        # Train the model using the launch training data
        modelList[i].fit(X_train, y_train)

        # Create predictions using the model
        y_pred = (modelList[i].predict(X_test) > 0.5).astype("int32")

        # Check the prediction
        print(y_pred)

        print(f"Evaluating model: {modelNames[i]}")
        
        # Evaluate accuracy of the model
        accuracy = accuracy_score(y_test, y_pred)
        print(f"Test Accuracy: {accuracy:.2f}")

        # Evaluate loss
        loss = log_loss(y_test, y_pred)
        print(f"Log Loss: {loss:.2f}")

        # Evaluate precision
        precision = precision_score(y_test, y_pred)
        print(f"Precision: {precision:.2f}")

        # Evaluate recall
        recall = recall_score(y_test, y_pred)
        print(f"Recall: {recall:.2f}")

        # Evaluate F1 score
        f1 = f1_score(y_test, y_pred)
        print(f"F1 Score: {f1:.2f}")

        # Confusion matrix
        print("Confusion Matrix:")
        print(confusion_matrix(y_test, y_pred))

        # Classification report
        print("\nClassification Report:")
        print(classification_report(y_test, y_pred))

        # Interpret the prediction
        y_pred = le.inverse_transform(y_pred)
        
        # Saves the trained model for future use by RLP_API
        save_trained_model(modelList[i], modelNames[i])

        # Check the prediction
        print(y_pred)

        # Create a LaunchPrediction object for each row in the dataframe used in the test data
        for index, row in enumerate(X_test):
            params_weather = {
                "Temperature": row[0],
                "Rain": row[1],
                "Showers": row[2],
                "Snowfall": row[3],
                "CloudCover": row[4],
                "CloudCoverLow": row[5],
                "CloudCoverMid": row[6],
                "CloudCoverHigh": row[7],
                "Visibility": row[8],
                "WindSpeed10m": row[9],
                "WindSpeed80m": row[10],
                "WindSpeed120m": row[11],
                "WindSpeed180m": row[12],
                "Temperature80m": row[13],
                "Temperature120m": row[14],
                "Temperature180m": row[15]
            }
            params_rocket = {
                "ToThrust": row[16],
                "LaunchMass": row[17],
                "RocketLength": row[18],
                "RocketDiameter": row[19],
                "SuccessfulRocketLaunches": row[20],
                "FailedRocketLaunches": row[21]
            }

            # Create a new PredictionResults object
            result = PredictionResults(
                result_id=str(uuid.uuid4()),
                model_name=modelNames[i],
                model_prediction = y_pred[index], 
                accuracy=accuracy, 
                loss=loss, 
                precision=precision, 
                recall=recall, 
                f1_score=f1
            )
            
            # Create a new LaunchPrediction object with the results
            launch_prediction = LaunchPrediction(
                params_weather=params_weather,
                params_rocket=params_rocket,
                result_id = result.result_id,
                results=result
            )
            launch_predictions.append(launch_prediction)
    
    # TODO - Save the model with the best accuracy that can be used for future predictions by RLP_API
    # model.save("success_rate_model.h5")
    # print("Model saved successfully")

    # Ingest the results of each model into the Postgres DB
    store_results(launch_predictions)



