import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { useAuth } from './AuthContext';
import '../Styles/LoginStyle.css';

const Registration = () => {
  const [userData, setUserData] = useState({
    userName: '',
    email: '',
    password: '',
    confirmPassword: '',
  });

  const [errors, setErrors] = useState({});
  const navigate = useNavigate();
  const { login } = useAuth();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUserData((prevUserData) => ({
      ...prevUserData,
      [name]: value,
    }));
  };

  const handleRegistration = async () => {
    try {
      const validationErrors = validateForm();
      if (Object.keys(validationErrors).length > 0) {
        setErrors(validationErrors);
        return;
      }

      const response = await axios.post('https://localhost:7125/api/Auth/register', {
        userName: userData.userName,
        email: userData.email,
        password: userData.password,
        confirmPassword: userData.confirmPassword,
      });

      if (response.status === 200) {
        const token = response.data.token;
        localStorage.setItem('jwtToken', token);
        login();
        navigate('/');
      } else {
        console.error('Ошибка регистрации');
      }
    } catch (error) {
      console.error('Произошла ошибка при отправке запроса:', error);
    }
  };

  const validateForm = () => {
    const errors = {};

    if (!userData.userName) {
      errors.userName = 'Имя пользователя обязательно';
    }

    if (!userData.email) {
      errors.email = 'Email обязателен';
    } else if (!/\S+@\S+\.\S+/.test(userData.email)) {
      errors.email = 'Некорректный формат email';
    }

    if (!userData.password) {
      errors.password = 'Пароль обязателен';
    }

    if (!userData.confirmPassword) {
      errors.confirmPassword = 'Подтверждение пароля обязательно';
    } else if (userData.confirmPassword !== userData.password) {
      errors.confirmPassword = 'Пароли не совпадают';
    }

    return errors;
  };

  return (
    <section>
      <div className="form-box-registration">
        <div className="form-value">
          <h2>Регистрация</h2>
          <div className="inputbox">
            <ion-icon name="person-outline"></ion-icon>
            <input
              className='input-class'
              required
              type="text"
              name="userName"
              value={userData.userName}
              onChange={handleChange}
            />
            <label for="">Username</label>
          </div>
          <div className="inputbox">
            <ion-icon name="mail-outline"></ion-icon>
            <input
              className='input-class'
              required
              type="email"
              name="email"
              value={userData.email}
              onChange={handleChange}
            />
            <label for="">Email</label>
          </div>
          <div className="inputbox">
            <ion-icon name="lock-closed-outline"></ion-icon>
            <input
              className='input-class'
              required
              type="password"
              name="password"
              value={userData.password}
              onChange={handleChange}
            />
            <label for="">Password</label>
          </div>
          <div className="inputbox">
            <ion-icon name="lock-closed-outline"></ion-icon>
            <input
              className='input-class'
              required
              type="password"
              name="confirmPassword"
              value={userData.confirmPassword}
              onChange={handleChange}
            />
            <label for="">Confirm Password</label>
          </div>
          {errors.confirmPassword && (
              <div></div>
            )}

          <div className="forget">
            <label htmlFor="rememberMe">
              <input className="white-color" type="checkbox" /> Remember Me
            </label>
          </div>
          <button onClick={handleRegistration}>Зарегистрироваться</button>
        </div>
      </div>
    </section>
  );
};

export default Registration;
