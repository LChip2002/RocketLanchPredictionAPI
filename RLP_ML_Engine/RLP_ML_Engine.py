import tensorflow as tf
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense, Dropout
from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report, confusion_matrix
from sklearn.preprocessing import LabelEncoder
import psycopg2
import pandas as pd
import numpy as np

# TODO - Ammend AI code to tailor to rocket launch data and db ingest and rertieval
# Creates a model for predicting the success rate of rocket launches
def success_rate_model(X_train):
    model = Sequential()
    model.add(Dense(128, activation='relu', input_shape=(X_train.shape[1],)))
    model.add(Dropout(0.2))
    model.add(Dense(64, activation='relu'))
    model.add(Dropout(0.2))
    model.add(Dense(1, activation='sigmoid'))  # Adjust the output layer based on your problem (e.g., regression or classification)

    # Compile the model
    model.compile(optimizer='adam',
                  loss='binary_crossentropy',  # Adjust the loss function based on your problem
                  metrics=['accuracy'])

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

if __name__ == "__main__":

    launch_data = get_launch_data()
    if launch_data:
        print("Launch data retrieved successfully")
    else:
        print("Failed to retrieve launch data")

    # TODO - Maybe perform some data preprocessing

    # Convert the retrieved data into a pandas DataFrame
    launch_df = pd.DataFrame(launch_data)

    # Split the data into features and labels
    X = np.array([launch_df['Temperature'], launch_df['Rain'], launch_df['Showers'], launch_df['Snowfall'], launch_df['CloudCover']
                 , launch_df['CloudCoverLow'], launch_df['CloudCoverMid'], launch_df['CloudCoverHigh'], launch_df['Visibility']
                 , launch_df['WindSpeed10m'], launch_df['WindSpeed80m'], launch_df['WindSpeed120m'], launch_df['WindSpeed180m']
                 , launch_df['Temperature80m'], launch_df['Temperature120m'], launch_df['Temperature180m']]).T  # Features
    
    y = np.array(launch_df['Status'])  # Labels

    # Transforms the string ground truth into a numerical value
    le = LabelEncoder()
    y = le.fit_transform(y)

    # Split the data into training and testing sets for model training
    # TODO - Maybe change split technique
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

    # Training the model on the launch data training parameters
    model = success_rate_model(X_train)

    model.summary()

    # Train the model using the launch training data
    model.fit(X_train, y_train, epochs=10)

    # Create predictions using the model
    y_pred = (model.predict(X_test) > 0.5).astype("int32")
    
    # Evaluate the model using evaluation metrics
    loss, accuracy = model.evaluate(X_test, y_test)
    print(f"Test Accuracy: {accuracy:.2f}")

    # Confusion matrix
    print("Confusion Matrix:")
    print(confusion_matrix(y_test, y_pred))

    # Classification report
    print("\nClassification Report:")
    print(classification_report(y_test, y_pred))


