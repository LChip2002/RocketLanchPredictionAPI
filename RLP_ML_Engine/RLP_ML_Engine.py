import tensorflow as tf
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense, Dropout
import psycopg2

# TODO - Ammend AI code to tailor to rocket launch data and db ingest and rertieval
def success_rate_model(input_shape):
    model = Sequential()
    model.add(Dense(128, activation='relu', input_shape=(input_shape,)))
    model.add(Dropout(0.2))
    model.add(Dense(64, activation='relu'))
    model.add(Dropout(0.2))
    model.add(Dense(32, activation='relu'))
    model.add(Dense(1, activation='sigmoid'))  # Adjust the output layer based on your problem (e.g., regression or classification)

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

    # Example usage
    input_shape = 10  # Replace with your actual input shape
    model = success_rate_model(input_shape)
    model.summary()