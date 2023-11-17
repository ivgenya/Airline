import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import '../Styles/FlightSchedules.css';

function CreateFlight() {
  const [flight, setFlightData] = useState({
    scheduleId: '',
    date: '',
    planeId: '',
    type: 'regular',
    status: 'on_time',
    gate: '',
  });

  const navigate = useNavigate();

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setFlightData({
      ...flight,
      [name]: value,
    });
  };

  const handleCreateFlight = async (e) => {
e.preventDefault();
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    try {
      const response = await axios.post('https://localhost:7125/api/flights', flight, { headers });
      if (response.status === 201) {
        alert('Рейс успешно добавлен')
        navigate('/flights');
      } else {
        console.error('Ошибка создания рейса');
      }
    } catch (error) {
      console.error('Произошла ошибка при отправке запроса:', error);
    }
  };

  return (
    <form>
    <div className='container'>
      <h2>Создание нового рейса</h2>
      <div className='form-group'>
        <label className='label'>
          Schedule ID:
          <input
          required
            type="text"
            name="scheduleId"
            value={flight.scheduleId}
            onChange={handleInputChange}
            className='input'
          />
        </label>
        <label className='label'>
          Date:
          <input
          required
            type="text"
            name="date"
            value={flight.date}
            onChange={handleInputChange}
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
            className='input'
          />
        </label>
        </div>
      <button onClick={handleCreateFlight} type="submit">Создать рейс</button>
    </div>
    </form>
  );
}

export default CreateFlight;