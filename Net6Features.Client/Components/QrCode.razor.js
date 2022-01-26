export function bufferToCanvas(elem, buffer, width, height) {
    let imageData = new ImageData(new Uint8ClampedArray(buffer.buffer, 0, width * height * 4), width, height);
    elem.width = width;
    elem.height = height;
    elem.getContext('2d').putImageData(imageData, 0, 0);
}