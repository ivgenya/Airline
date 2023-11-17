import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../Styles/FlightSchedules.css';
import { useNavigate } from 'react-router-dom';

function Flights() {
  const [flights, setFlights] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    axios
      .get('https://localhost:7125/api/flights/all', { headers })
      .then((response) => {
        setFlights(response.data);
      })
      .catch((error) => {
        console.error('Ошибка получения данных:', error);
      });
  }, []);

  const handleEditFlight = (flightId) => {
    navigate(`/flight/${flightId}`);
  }

  const handleCreateFlight = () => {
    navigate(`/flight/create`);
  };

  return (
    <div className='flight-container'>
      <h1>Список рейсов</h1>
      <button onClick={handleCreateFlight}>Создать</button>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Schedule ID</th>
            <th>Date</th>
            <th>Plane ID</th>
            <th>Type</th>
            <th>Status</th>
            <th>Gate</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {flights.map((flight) => (
            <tr key={flight.id}>
              <td>{flight.id}</td>
              <td>{flight.scheduleId}</td>
              <td>{new Date(flight.date).toLocaleDateString()}</td>
              <td>{flight.planeId}</td>
              <td>{flight.type}</td>
              <td>{flight.status}</td>
              <td>{flight.gate}</td>
              <td>
                <button onClick={() => handleEditFlight(flight.id)}>Редактировать</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Flights;
