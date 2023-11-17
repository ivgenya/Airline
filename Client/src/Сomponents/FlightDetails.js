import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import styles from '../Styles/FlightSchedules.css';

function FlightDetails() {
  const { id } = useParams();
  const [flight, setFlight] = useState({});
  const [isEditing, setIsEditing] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    axios
      .get(`https://localhost:7125/api/flights/${id}`, { headers })
      .then((response) => {
        setFlight(response.data);
      })
      .catch((error) => {
        console.error('Ошибка получения данных:', error);
      });
  }, [id]);

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setFlight({
      ...flight,
      [name]: value,
    });
  };

  const handleDelete = () => {
    const shouldDelete = window.confirm('Вы уверены, что хотите удалить данный рейс?');

    if (shouldDelete) {
      const jwtToken = localStorage.getItem('jwtToken');
      const headers = {
        Authorization: `Bearer ${jwtToken}`,
      };

      axios
        .delete(`https://localhost:7125/api/flights/${id}`, { headers })
        .then((response) => {
          console.log('Успешно удалено:', response.data);
          navigate('/flights');
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
      .put(`https://localhost:7125/api/flights/${id}`, flight, { headers })
      .then((response) => {
        setIsEditing(false);
      })
      .catch((error) => {
        console.error('Ошибка при сохранении данных:', error);
      });
  };

  return (
    <div className='container'>
      <h2>Flight Details</h2>
      <form>
      <div className='form-group'>
        <label className='label'>
          Schedule ID:
          <input
          required
            type="text"
            name="scheduleId"
            value={flight.scheduleId}
            onChange={handleInputChange}
            disabled={!isEditing}
            className='input'
          />
        </label>
        <label className='label'>
          Date:
          <input
          required
            type="date"
            name="date"
            value={flight.date}
            onChange={handleInputChange}
            disabled={!isEditing}
            className='input'
          />
        </label>
        <label className='label'>
          Plane ID:
          <input
          required
            type="text"
            name="planeId"
            value={flight.planeId}
            onChange={handleInputChange}
            disabled={!isEditing}
            className='input'
          />
        </label>
        <label className='label'>
        Type:
        <select
        required
            name="type"
            value={flight.type}
            onChange={handleInputChange}
            disabled={!isEditing}
            className='select'
          >
            <option value="regular">Regular</option>
            <option value="charter">Charter</option>
          </select>
        </label>
        <label className='label'>
          Status:
          <select
          required
            name="status"
            value={flight.status}
            onChange={handleInputChange}
            disabled={!isEditing}
            className='select'
          >
            <option value="on_time">On Time</option>
            <option value="delayed">Delayed</option>
            <option value="departed">Departed</option>
            <option value="expected">Expected</option>
            <option value="landed">Landed</option>
            <option value="boarding">Boarding</option>
            <option value="cancelled">Cancelled</option>
            <option value="check_in">Check-In</option>
          </select>
        </label>
        <label className='label'>
          Gate:
          <input
          required
            type="text"
            name="gate"
            value={flight.gate}
            onChange={handleInputChange}
            disabled={!isEditing}
            className='input'
          />
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

export default FlightDetails;
