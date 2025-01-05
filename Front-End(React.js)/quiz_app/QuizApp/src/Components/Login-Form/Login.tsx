import React from 'react';
import './Login.css';
import { FaRegUserCircle } from "react-icons/fa";
import { RiLockPasswordFill } from "react-icons/ri";
const Login = () =>{
    return(
        <div className='wrapper'>
            <form action="">
                <h1>Login</h1>
                <div className="input-box">
                    <input type="text" required placeholder='Username' /> 
                    <FaRegUserCircle className='icon'/>
                </div>
                <div className="input-box">
                    <input type="password" required placeholder='Password' /> 
                    <RiLockPasswordFill className='icon'/>
                </div>
                <div className="remember-forgot">
                    <label><input type="checkbox"  />Remember Me</label>
                    <a href="#">Forgot password?</a>
                </div>
                <button type='submit'>Login</button>

                <div className="register-link">
                    <p>Don't have an Account? <a href="#">Regester</a></p>
                </div>
            </form>
        </div>
    );
};

export default Login;