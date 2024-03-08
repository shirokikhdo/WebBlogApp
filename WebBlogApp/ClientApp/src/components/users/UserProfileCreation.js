import React, { useState } from 'react';
import ImageComponent from '../ImageComponent';
import ImageUploader from '../ImageUploader';

const UserProfileCreation = ({user, setAction }) => {
    const [userName, setUserName] = useState(user.name);
    const [userEmail, setUserEmail] = useState(user.email);
    const [userPassword, setUserPassword] = useState();
    const [userDescription, setUserDescription] = useState(user.description);
    const [userPhoto, setUserPhoto] = useState(user.photo);

    const endCreate = () => {
        if (userPassword.length === 0) return;
        const newUser = {
            name: userName,
            email: userEmail,
            password: userPassword,
            description: userDescription,
            photo: userPhoto
        }
        setAction(newUser);
    }

    return (
        <div style={{ display: 'flex', flexDirection: 'column' }}>
            <h2>User Profile</h2>
            <p>Name</p>
            <input type='text' defaultValue={userName} onChange={e => setUserName(e.target.value)} />
            <p>Email</p>
            <input type='text' defaultValue={userEmail} onChange={e => setUserEmail(e.target.value)} />
            <p>Password</p>
            <input type='text' defaultValue={userPassword} onChange={e => setUserPassword(e.target.value)} />
            <p>Description</p>
            <textarea defaultValue={userDescription} onChange={e => setUserDescription(e.target.value)} />
            <ImageUploader byteArrayAction={(bytes) => setUserPhoto(bytes)} />
            <ImageComponent byteArray={userPhoto} />
            <button onClick={endCreate}>Œ </button>
        </div>
    );
};

export default UserProfileCreation;