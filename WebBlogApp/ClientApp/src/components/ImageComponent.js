import React from 'react';

const ImageComponent = ({ byteArray }) => {
    if (byteArray === null) return <img alt='Image' />;
    const base64String = btoa(String.fromCharCode(...byteArray));
    const imageUrl = `data:image/jpeg;base64,${base64String}`;
    return <img src={imageUrl} alt='Image'/>;
}

export default ImageComponent;