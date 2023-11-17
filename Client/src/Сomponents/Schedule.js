import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../Styles/FlightSchedules.css';
import { useNavigate } from 'react-router-dom';

function Schedules() {
  const [schedules, setSchedules] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    axios
      .get('https://localhost:7125/api/schedule', { headers })
      .then((response) => {
        setSchedules(response.data);
      })
      .catch((error) => {
        console.error('Ошибка получения данных:', error);
      });
  }, []);

  const handleEditSchedule = (scheduleId) => {
    navigate(`/schedule/${scheduleId}`);
  }

  const handleCreateSchedule = () => {
    navigate(`/schedule/create`);
  };


  return (
    <div className='flight-container'>
      <h1>Расписание</h1>
      <button onClick={handleCreateSchedule}>Создать</button>
      <table>
        <thead>
          <tr>
            <th>Airline ID</th>
            <th>Number</th>
            <th>Departure Airport ID</th>
            <th>Arrival Airport ID</th>
            <th>Departure Time</th>
            <th>Arrival Time</th>
            <th>Flight Duration</th>
            <th>Terminal</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {schedules.map((schedule) => (
            <tr key={schedule.id}>
              <td>{schedule.airlineId}</td>
              <td>{schedule.number}</td>
              <td>{schedule.departureAirportId}</td>
              <td>{schedule.arrivalAirportId}</td>
              <td>{schedule.departureTime}</td>
              <td>{schedule.arrivalTime}</td>
              <td>{schedule.flightDuration}</td>
              <td>{schedule.terminal}</td>
              <td>
                <button onClick={() => handleEditSchedule(schedule.id)}>Редактировать</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Schedules;
