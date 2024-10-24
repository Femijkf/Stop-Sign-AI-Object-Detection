let map;
let drawingManager;
let selectedShape;

function initMap() {
    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: -34.397, lng: 150.644 },
        zoom: 8,
    });

    drawingManager = new google.maps.drawing.DrawingManager({
        drawingMode: google.maps.drawing.OverlayType.RECTANGLE,
        drawingControl: true,
        drawingControlOptions: {
            position: google.maps.ControlPosition.TOP_CENTER,
            drawingModes: ['rectangle']
        },
        rectangleOptions: {
            editable: true,
            draggable: true
        }
    });
    drawingManager.setMap(map);

    google.maps.event.addListener(drawingManager, 'overlaycomplete', function(event) {
        if (selectedShape) {
            selectedShape.setMap(null);
        }
        selectedShape = event.overlay;
    });

    document.getElementById('process-selection').addEventListener('click', () => {
        if (selectedShape) {
            const bounds = selectedShape.getBounds();
            const ne = bounds.getNorthEast();
            const sw = bounds.getSouthWest();

            const selection = {
                north: ne.lat(),
                east: ne.lng(),
                south: sw.lat(),
                west: sw.lng()
            };

            fetch('/process-selection', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(selection)
            })
            .then(response => response.json())
            .then(data => {
                const resultsDiv = document.getElementById('results');
                resultsDiv.innerHTML = `Detected area: ${data.area.toFixed(2)} square feet`;
            })
            .catch(error => console.error('Error:', error));
        }
    });
}

window.initMap = initMap;
