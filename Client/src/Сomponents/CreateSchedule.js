import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function CreateSchedule() {
  const [schedule, setScheduleData] = useState({
    airlineId: '',
    number: '',
    departureAirportId: '',
    arrivalAirportId: '',
    departureTime: '',
    arrivalTime: '',
    flightDuration: '',
    terminal: '',
  });

  const navigate = useNavigate();

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setScheduleData({
      ...schedule,
      [name]: value,
    });
  };

  const handleCreateSchedule = async (e) => {
    e.preventDefault();
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    try {
      const response = await axios.post('https://localhost:7125/api/schedule', schedule, { headers });

      if (response.status === 201) {
        alert('Расписание успешно добавлено');
        navigate('/schedule');
      } else {
        console.error('Ошибка создания расписания');
      }
    } catch (error) {
      console.error('Произошла ошибка при отправке запроса:', error);
    }
  };

  return (
    <form>
    <div className='container'>
      <h2>Создание нового расписания</h2>
      <div className='form-group'>
        <label className='label'>
          Airline ID:
          <input
          required
            type="text"
            name="airlineId"
            value={schedule.airlineId}
            onChange={handleInputChange}
            className='input'
          />
        </label>
        <label className='label'>
          Number:
          <input
          required
            type="text"
            name="number"
            value={schedule.number}
            onChange={handleInputChange}
            className='input'
          />
        </label>
        <label className='label'>
          Departure Airport ID:
          <input
          required
            type="text"
            name="departureAirportId"
            value={schedule.departureAirportId}
            onChange={handleInputChange}
            className='input'
          />
        </label>
        <label className='label'>
          Arrival Airport ID:
          <input
          required
            type="text"
            name="arrivalAirportId"
            value={schedule.arrivalAirportId}
            onChange={handleInputChange}
            className='input'
          />
        </label>
        <label className='label'>
          Departure Time:
          <input
          required
            type="time"
            step="1"
            name="departureTime"
            value={schedule.departureTime}
            onChange={handleInputChange}
            className='input'
          />
        </label>
        <label className='label'>
          Arrival Time:
          <input
          required
            type="time"
            step="1"
            name="arrivalTime"
            value={schedule.arrivalTime}
            onChange={handleInputChange}
            className='input'
          />
        </label>
        <label className='label'>
          Flight Duration:
          <input
          required
            type="time"
            step="1"
            name="flightDuration"
            value={schedule.flightDuration}
            onChange={handleInputChange}
            className='input'
          />
        </label>
        <label className='label'>
          Terminal:
          <input
          required
            type="text"
            name="terminal"
            value={schedule.terminal}
            onChange={handleInputChange}
            className='input'
          />
        </label>
      </div>
      <button onClick={handleCreateSchedule} type="submit">Создать расписание</button>
    </div>
    </form>
  );

}

export default CreateSchedule;
