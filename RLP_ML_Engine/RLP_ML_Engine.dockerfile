# Use the official TensorFlow image as a base image
FROM tensorflow/tensorflow:latest

# Set the working directory in the container
WORKDIR /app

# Copy the current directory contents into the container at /app
COPY . /app

# Install any needed packages specified in requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

# Make port 80 available to the world outside this container
EXPOSE 80

# Define environment variable
ENV NAME RLP_ML_Engine

# Run the application
CMD ["python", "app.py"]
# Set the entry point for the container
ENTRYPOINT ["python", "app.py"]