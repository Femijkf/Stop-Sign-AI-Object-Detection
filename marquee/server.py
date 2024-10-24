from flask import Flask, request, jsonify
import cv2
import numpy as np
import requests
from io import BytesIO
from PIL import Image

app = Flask(__name__)

@app.route('/process-selection', methods=['POST'])
def process_selection():
    data = request.json
    north = data['north']
    east = data['east']
    south = data['south']
    west = data['west']

    # Use Google Maps Static API to get the satellite image of the selected area
    api_key = 'YOUR_GOOGLE_MAPS_API_KEY'
    url = f"https://maps.googleapis.com/maps/api/staticmap?center={(north+south)/2},{(east+west)/2}&zoom=18&size=640x640&maptype=satellite&key={api_key}"
    response = requests.get(url)
    img = Image.open(BytesIO(response.content))
    img = np.array(img)

    # Convert to grayscale
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

    # Apply GaussianBlur to reduce noise and improve edge detection
    blurred = cv2.GaussianBlur(gray, (5, 5), 0)

    # Edge detection using Canny
    edges = cv2.Canny(blurred, 50, 150)

    # Find contours
    contours, _ = cv2.findContours(edges, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)

    # Filter contours by area (to remove small objects)
    min_area = 500  # You can adjust this threshold
    filtered_contours = [cnt for cnt in contours if cv2.contourArea(cnt) > min_area]

    # Calculate and sum areas of detected buildings
    total_area = 0
    for cnt in filtered_contours:
        total_area += cv2.contourArea(cnt)

    # Convert pixel area to square meters
    pixel_to_meter_conversion = 0.25  # Example conversion factor
    total_area_meters = total_area * (pixel_to_meter_conversion ** 2)

    # Convert square meters to square feet
    total_area_feet = total_area_meters * 10.7639

    return jsonify({'area': total_area_feet})

if __name__ == '__main__':
    app.run(debug=True)
