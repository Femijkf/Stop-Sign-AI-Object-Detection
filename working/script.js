const container = document.getElementById('container');
const marquee = document.getElementById('marquee');
let isDrawing = false;
let startCoords = {};

container.addEventListener('mousedown', (e) => {
    isDrawing = true;
    startCoords.x = e.clientX - container.offsetLeft;
    startCoords.y = e.clientY - container.offsetTop;
    marquee.style.left = startCoords.x + 'px';
    marquee.style.top = startCoords.y + 'px';
});

container.addEventListener('mousemove', (e) => {
    if (!isDrawing) return;
    const currentX = e.clientX - container.offsetLeft;
    const currentY = e.clientY - container.offsetTop;
    const width = currentX - startCoords.x;
    const height = currentY - startCoords.y;
    marquee.style.width = Math.abs(width) + 'px';
    marquee.style.height = Math.abs(height) + 'px';
    marquee.style.left = (width > 0) ? startCoords.x + 'px' : (startCoords.x + width) + 'px';
    marquee.style.top = (height > 0) ? startCoords.y + 'px' : (startCoords.y + height) + 'px';
});

container.addEventListener('mouseup', () => {
    isDrawing = false;
    marquee.style.width = '0';
    marquee.style.height = '0';
});
