import { useState } from "react"
import { getToken } from "../../services/commonService";
import React from 'react';


const Login = () => {
    const [username, setUserName] = useState();
    const [password, setPassword] = useState();

    const enterClick = () => {
        getToken(username, password);
    }

    return (
        <div>
            <p>Login</p>
            <input type='text' onChange={e => setUserName(e.target.value)} />
            <p>Password</p>
            <input type='password' onChange={e => setPassword(e.target.value)} />
            <button className='btn btn-primary' onClick={enterClick}>Enter</button>
        </div>
    );
}

export default Login;