import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { useLocation } from 'react-router-dom';
import '../Styles/Search.css';

function SearchFlight() {
  const navigate = useNavigate();
const [flightFullName, setFlightFullName] = useState('');
const [date, setDate] = useState('');
const [flight, setFlight] = useState([]);
const [searchButtonClicked, setSearchButtonClicked] = useState(false);
const [flightsNotFound, setFlightsNotFound] = useState(false);


  const handleSearch = async (e) => {
    e.preventDefault(); 
    try {
        setSearchButtonClicked(true);
      const response = await axios.get('https://localhost:7125/api/flights/fullname', {
        params: {
            flightFullName,
          date,
        },
      });
      

      if (response.status === 200) {
        setFlight(response.data);
        setFlightsNotFound(false);
      } else {
        console.error('Ошибка при получении данных');
        setFlightsNotFound(true);
        
      }
    } catch (error) {
    } 
  }

  return (
    <div>
    <div className='container-search'>
      <form onSubmit={(e) => handleSearch(e)}>
        <div className="input-group-search">
          <input
            required
            type="text"
            value={flightFullName}
            onChange={(e) => setFlightFullName(e.target.value)}
            placeholder="Название рейса"
          />
        </div>
        <div className="input-group-search">
          <input
            required
            type="date"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            placeholder="Дата"
          />
        </div>
        <button className='search-btn' type="submit">
          Поиск
        </button>
      </form>
    </div>
    {flightsNotFound && <p>Рейсы не найдены</p>}
    {searchButtonClicked && !flightsNotFound && (
        <div>
            <h3>Результаты:</h3>
            <div className="results">
            <div className='flight-card'>
            <div className='flight-name'>{flight.airlineShortName}{flight.scheduleNumber}</div>
            <div>
              <div>{flight.departureAirportShortName} ({flight.departureAirportCity})</div>
              <ion-icon name="airplane-outline"></ion-icon>
              <div>{flight.arrivalAirportShortName} ({flight.arrivalAirportCity})</div>
            </div>
            <div>
              <div>{new Date(flight.date).toLocaleDateString()}</div>
              <div><ion-icon name="calendar-outline"></ion-icon></div>
            </div>
            <div>
              <div>{flight.departureTime} - {flight.arrivalTime}</div>
              <div>В пути - {flight.flightDuration}</div>
            </div>
              <div>
                <div className='flight-price'>{flight.status}</div>
              </div>
            </div>
        </div>
        </div>
      )}
      </div>
  );
}

export default SearchFlight;
