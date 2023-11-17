import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import styles from '../Styles/FlightSchedules.css';

function UserDetails() {
  const { username } = useParams();
  const [user, setUser] = useState({});
  const [isEditing, setIsEditing] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    axios
      .get(`https://localhost:7125/api/User/${username}`, { headers })
      .then((response) => {
        setUser(response.data);
      })
      .catch((error) => {
        console.error('Ошибка получения данных:', error);
      });
  }, [username]);

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setUser({
      ...user,
      [name]: value,
    });
  };

  const handleDelete = () => {
    const shouldDelete = window.confirm('Вы уверены, что хотите удалить этого пользователя?');

    if (shouldDelete) {
      const jwtToken = localStorage.getItem('jwtToken');
      const headers = {
        Authorization: `Bearer ${jwtToken}`,
      };

      axios
        .delete(`https://localhost:7125/api/User/${username}`, { headers })
        .then((response) => {
          console.log('Успешно удалено:', response.data);
          navigate('/users');
        })
        .catch((error) => {
          console.error('Ошибка при удалении данных:', error);
        });
    }
  };

  const handleSaveChanges = () => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };
    axios
      .put(`https://localhost:7125/api/User/${username}`, user, { headers })
      .then((response) => {
        setIsEditing(false);
      })
      .catch((error) => {
        console.error('Ошибка при сохранении данных:', error);
      });
  };

  return (
    <div className='container'>
      <h2>User Details</h2>
      <form>
        <div className='form-group'>
          <label className='label'>
            Username:
            <input
              type="text"
              name="userName"
              value={user.userName}
              onChange={handleInputChange}
              disabled={!isEditing}
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
              disabled={!isEditing}
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
          
          <button type="button" onClick={handleDelete} className='button'>
            Удалить
          </button>
          {!isEditing ? (
            <button type="button" onClick={() => setIsEditing(true)} className='button'>
              Редактировать
            </button>
          ) : (
            <button type="button" onClick={handleSaveChanges} className='button'>
              Сохранить изменения
            </button>
          )}
        </div>
      </form>
    </div>
  );
}

export default UserDetails;
