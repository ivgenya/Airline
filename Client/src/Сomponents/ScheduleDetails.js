import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import '../Styles/FlightSchedules.css';

function ScheduleDetails() {
  const { id } = useParams();
  const [schedule, setSchedule] = useState({});
  const [isEditing, setIsEditing] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    axios
      .get(`https://localhost:7125/api/schedule/${id}`, { headers })
      .then((response) => {
        setSchedule(response.data);
      })
      .catch((error) => {
        console.error('Ошибка получения данных:', error);
      });
  }, [id]);

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setSchedule({
      ...schedule,
      [name]: value,
    });
  };

  const handleDelete = () => {
    const shouldDelete = window.confirm('Вы уверены, что хотите удалить данное расписание?');

    if (shouldDelete) {
      const jwtToken = localStorage.getItem('jwtToken');
      const headers = {
        Authorization: `Bearer ${jwtToken}`,
      };

      axios
        .delete(`https://localhost:7125/api/schedule/${id}`, { headers })
        .then((response) => {
          console.log('Успешно удалено:', response.data);
          navigate('/schedule');
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
      .put(`https://localhost:7125/api/schedule/${id}`, schedule, { headers })
      .then((response) => {
        setIsEditing(false);
      })
      .catch((error) => {
        console.error('Ошибка при сохранении данных:', error);
      });
  };

  return (
    <div className='container'>
      <h2>Schedule Details</h2>
      <form>
      <div className='form-group'>
        <label className='label'>
          Airline ID:
          <input
            type="text"
            name="airlineId"
            value={schedule.airlineId}
            onChange={handleInputChange}
            disabled={!isEditing}
            className='input'
          />
        </label>
        <label className='label'>
          Number:
          <input
            type="text"
            name="number"
            value={schedule.number}
            onChange={handleInputChange}
            disabled={!isEditing}
            className='input'
          />
        </label>
        <label className='label'>
          Departure Airport ID:
          <input
            type="text"
            name="departureAirportId"
            value={schedule.departureAirportId}
            onChange={handleInputChange}
            disabled={!isEditing}
            className='input'
          />
        </label>
        <label className='label'>
          Arrival Airport ID:
          <input
            type="text"
            name="arrivalAirportId"
            value={schedule.arrivalAirportId}
            onChange={handleInputChange}
            disabled={!isEditing}
            className='input'
          />
        </label>
        <label className='label'>
            Departure Time:
            <input
                type="time"
                name="departureTime"
                value={schedule.departureTime}
                onChange={handleInputChange}
                disabled={!isEditing}
                className='input'
                pattern="(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]"
                title="Введите время в формате HH:mm:ss"
            />
            </label>

            <label className='label'>
            Arrival Time:
            <input
                type="time"
                name="arrivalTime"
                value={schedule.arrivalTime}
                onChange={handleInputChange}
                disabled={!isEditing}
                className='input'
                title="Введите время в формате HH:mm:ss"
            />
            </label>

            <label className='label'>
            Flight Duration:
            <input
                type="time"
                name="flightDuration"
                value={schedule.flightDuration}
                onChange={handleInputChange}
                disabled={!isEditing}
                className='input'
                title="Enter time in HH:mm:ss format"
            />
            </label>
        <label className='label'>
          Terminal:
          <input
            type="text"
            name="terminal"
            value={schedule.terminal}
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

export default ScheduleDetails;
