import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function CreateUser() {
  const [user, setUserData] = useState({
    userName: '',
    email: '',
    password: '',
    role: 'admin', 
  });

  const navigate = useNavigate();

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setUserData({
      ...user,
      [name]: value,
    });
  };

  const handleCreateUser = async (e) => {
    e.preventDefault();
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    try {
      const response = await axios.post('https://localhost:7125/api/User/add', user, { headers });

      if (response.status === 200) {
        alert('Пользователь успешно создан');
        navigate('/users');
      } else {
        console.error('Ошибка создания пользователя');
      }
    } catch (error) {
      console.error('Произошла ошибка при отправке запроса:', error);
    }
  };

  return (
    <form>
    <div className='container'>
      <h2>Создание нового пользователя</h2>
      <div className='form-group'>
        <label className='label'>
          Username:
          <input
            type="text"
            name="userName"
            value={user.userName}
            onChange={handleInputChange}
            className='input'
          />
        </label>
        <label className='label'>
          Email:
          <input
            type="email"
            name="email"
            value={user.email}
            onChange={handleInputChange}
            className='input'
          />
        </label>
        <label className='label'>
          Password:
          <input
            type="password"
            name="password"
            value={user.password}
            onChange={handleInputChange}
            className='input'
          />
        </label>
        <label className='label'>
          Role:
          <select
            name="role"
            value={user.role}
            onChange={handleInputChange}
            className='select'
          >
            <option value="dispatcher">Dispatcher</option>
            <option value="admin">Admin</option>
          </select>
        </label>
      </div>
      <button type="submit" onClick={handleCreateUser}>Создать пользователя</button>
    </div>
    </form>
  );
}

export default CreateUser;
