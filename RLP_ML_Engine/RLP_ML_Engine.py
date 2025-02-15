import tensorflow as tf
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense, Dropout
from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report, confusion_matrix
from sklearn.preprocessing import LabelEncoder, StandardScaler
import psycopg2
import pandas as pd
import numpy as np

# TODO - Compare classification models and other models potentially
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

def store_results():
    
    # Connect to your postgres DB
    conn = psycopg2.connect(
        dbname="postgres",
        user="postgres",
        password="postgres",
        host="localhost",
        port="5432"
    )

    # TODO - Create an object with all model results to ingest into the DB
    model_results = {
        "model_name": "success_rate_model",
        "accuracy": 0.85,
        "loss": 0.15
        }

    # Create a cursor object
    cursor = conn.cursor()

    # TODO - Edit this to accomodate the data I want to store
    # Ensure the table exists
    cursor.execute("""
    CREATE TABLE IF NOT EXISTS model_results (
        id SERIAL PRIMARY KEY,
        model_name VARCHAR(255),
        accuracy FLOAT,
        loss FLOAT,
        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    )
    """)
    
    # Execute a query
    cursor.execute("INSERT INTO model_results (model_name, accuracy, loss) VALUES ('success_rate_model', 0.85, 0.15)")  # Adjust the query based on your table structure

    

if __name__ == "__main__":

    launch_data = get_launch_data()
    if launch_data:
        print("Launch data retrieved successfully")
    else:
        print("Failed to retrieve launch data")

    # Convert the retrieved data into a pandas DataFrame
    launch_df = pd.DataFrame(launch_data)

    # Split the data into features and labels
    X = np.array([launch_df['Temperature'], launch_df['Rain'], launch_df['Showers'], launch_df['Snowfall'], launch_df['CloudCover']
                 , launch_df['CloudCoverLow'], launch_df['CloudCoverMid'], launch_df['CloudCoverHigh'], launch_df['Visibility']
                 , launch_df['WindSpeed10m'], launch_df['WindSpeed80m'], launch_df['WindSpeed120m'], launch_df['WindSpeed180m']
                 , launch_df['Temperature80m'], launch_df['Temperature120m'], launch_df['Temperature180m']]).T  # Features
    
    y = launch_df['Status']  # Labels


    # Transforms the string ground truth into a numerical value
    le = LabelEncoder()
    y = le.fit_transform(y)

    # Split the data into training and testing sets for model training
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

    # Normalize features
    scaler = StandardScaler()
    X_train = scaler.fit_transform(X_train)
    X_test = scaler.transform(X_test)

    # Training the model on the launch data training parameters
    model = success_rate_model(X_train)

    model.summary()

    # Train the model using the launch training data
    model.fit(X_train, y_train, epochs=50, batch_size=8, validation_data=(X_test, y_test))

    # Create predictions using the model
    y_pred = (model.predict(X_test) > 0.5).astype("int32")

    # Check the prediction
    print(y_pred)
    
    # Evaluate the model using evaluation metrics
    loss, accuracy = model.evaluate(X_test, y_test)
    print(f"Test Accuracy: {accuracy:.2f}")

    # Confusion matrix
    print("Confusion Matrix:")
    print(confusion_matrix(y_test, y_pred))

    # Classification report
    print("\nClassification Report:")
    print(classification_report(y_test, y_pred))

    # TODO - Save the model and ingest results into the Postgres DB
    # Save the model
    model.save("success_rate_model.h5")
    print("Model saved successfully")

    # TODO - Ingest the results into the Postgres DB
    store_results()



