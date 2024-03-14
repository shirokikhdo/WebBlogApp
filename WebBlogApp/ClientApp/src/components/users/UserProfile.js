import React, { useEffect, useState } from 'react';
import { getUser, updateUser } from '../../services/usersService';
import ImageComponent from '../ImageComponent';
import ModalButton from '../ModalButton';
import UserProfileCreation from './UserProfileCreation';

const UserProfile = () => {
    const [user, setUser] = useState({
        name: '',
        email: '',
        password: '',
        description: '',
        photo: ''
    });

    useEffect(() => {
        const fetchUser = async () => {
            const data = await getUser();
            setUser(data);
        };

        fetchUser();
    }, []);

    const updateUserView = (newUser) => {
        setUser(newUser);
        updateUser(newUser);
    }

    return (
        <div>
            <h2>User Profile</h2>
            <p>Name: {user.name}</p>
            <p>Email: {user.email}</p>
            <p>Description: {user.description}</p>
            <ImageComponent byteArray={user.photo} />
            <ModalButton modalContent={<UserProfileCreation user={user} setAction={updateUserView} />} title='�������������� �������' />
        </div>
    );
};

export default UserProfile;