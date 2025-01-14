import tensorflow as tf
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense, Dropout

# TODO - Ammend AI code to tailor to rocket launch data and db ingest and rertieval
def create_model(input_shape):
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

if __name__ == "__main__":
    # Example usage
    input_shape = 10  # Replace with your actual input shape
    model = create_model(input_shape)
    model.summary()