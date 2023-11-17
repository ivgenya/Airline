import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import '../Styles/FlightSchedules.css';

function Users() {
  const [users, setUsers] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    axios
      .get('https://localhost:7125/api/User/all', { headers })
      .then((response) => {
        setUsers(response.data);
      })
      .catch((error) => {
        console.error('Ошибка получения данных:', error);
      });
  }, []);

  const handleEditUser = (userName) => {
    navigate(`/user/${userName}`);
  }

  const handleCreateUser = () => {
    navigate(`/user/create`);
  }

  return (
    <div className='flight-container'>
      <h1>Список пользователей</h1>
      <button onClick={handleCreateUser}>Создать</button>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Username</th>
            <th>Email</th>
            <th>Role</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id}>
              <td>{user.id}</td>
              <td>{user.userName}</td>
              <td>{user.email}</td>
              <td>{user.role}</td>
              <td>
                <button onClick={() => handleEditUser(user.userName)}>Редактировать</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Users;
