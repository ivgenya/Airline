import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { useAuth } from './AuthContext';
import '../Styles/LoginStyle.css';


const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const { login } = useAuth();
  const navigate = useNavigate();
  const redirectFrom = localStorage.getItem('redirectFrom');

  const handleLogin = async () => {
    try {
      const response = await axios.post('https://localhost:7125/api/Auth/login', {
        userName: username,
        password: password,
      });

      if (response.status === 200) {
        const token = response.data.token;
        localStorage.setItem('jwtToken', token);
        login();
        if (redirectFrom) {
            navigate(redirectFrom);
          } else {
            navigate('/');
          }
      } else {
        console.error('Ошибка входа');
      }
    } catch (error) {
      console.error('Произошла ошибка при отправке запроса:', error);
    }
  };

  return (
    <section>
    <div className="form-box">
    <div className="form-value">
            <h2>Войти</h2>
            <div className="inputbox">
                <ion-icon name="mail-outline"></ion-icon>
                <input
                required
                  type="text"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                />
                <label for="">Username</label>
            </div>
            <div className="inputbox">
                <ion-icon name="lock-closed-outline"></ion-icon>
                <input
                required
                className='input-class'
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
                <label for="">Password</label>
            </div>
            <div className="forget">
                <label for=""><input className='white-color' type="checkbox"/>Remember Me</label>
              
            </div>
            <button onClick={handleLogin}>Войти</button>
            <div class="register">
                <p className='white-color'>Don't have a account <a href="#">Register</a></p>
            </div>
    </div>
</div>
</section>

  );
};

export default Login;
