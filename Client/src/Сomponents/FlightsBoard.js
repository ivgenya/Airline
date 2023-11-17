import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import '../Styles/BoardStyle.css';
import { useLocation } from 'react-router-dom';

function FlightSearch() {
    const location = useLocation();
    localStorage.setItem('redirectFrom', location.pathname);
  const [departureCity, setDepartureCity] = useState('');
  const [arrivalCity, setArrivalCity] = useState('');
  const [date, setDate] = useState('');
  const [flights, setFlights] = useState([]);
  const [loading, setLoading] = useState(false);
  const [searchButtonClicked, setSearchButtonClicked] = useState(false);
  const navigate = useNavigate();

  const handleSearch = async (e) => {
    e.preventDefault(); 
    try {
      setSearchButtonClicked(true);
      setLoading(true);
      const response = await axios.get('https://localhost:7125/api/flights/board', {
        params: {
          departureCity,
          arrivalCity,
          date,
        },
      });
      

      if (response.status === 200) {
        setFlights(response.data);
      } else {
        console.error('Ошибка при получении данных');
      }
    } catch (error) {
      console.error('Произошла ошибка при отправке запроса:', error);
    } finally {
      setLoading(false);
    }
  }

  const handleSwapText = (e) => {
    e.preventDefault();
    const temp = departureCity;
    setDepartureCity(arrivalCity);
    setArrivalCity(temp);
  }


  const handleBuyTicket = (flightId) => {
    if(localStorage.getItem('jwtToken') != null){
        navigate(`/buy/${flightId}`);
    }else{
        navigate(`/login`);
    }

  }

  const flightsNotFound = flights.length === 0 && searchButtonClicked;

  return (
    <div className="flight-search">
      <h2>Поиск рейсов</h2>
      <form onSubmit={handleSearch}>
      <div className="input-group">
        <input
          required
          type="text"
          value={departureCity}
          onChange={(e) => setDepartureCity(e.target.value)}
          placeholder="Откуда"
        />
      </div>
      <button className='board' onClick={handleSwapText}>
      <ion-icon name="repeat-outline"></ion-icon>
      </button>
      <div className="input-group">
        <input
          required
          type="text"
          value={arrivalCity}
          onChange={(e) => setArrivalCity(e.target.value)}
          placeholder="Куда"
        />
      </div>
      <div className="input-group">
        <input
          required
          type="date"
          value={date}
          onChange={(e) => setDate(e.target.value)}
          placeholder="Туда"
        />
      </div>
      <button className='board' type="submit" disabled={loading}>
        Поиск
      </button>
      </form>
      {loading && <p>Идет поиск рейсов...</p>}
      {flightsNotFound && <p>Рейсы не найдены</p>}
      {searchButtonClicked && !flightsNotFound && (
        <div>
                  <h3>Результаты:</h3>
        <div className="results">
              {flights.map((flight) => (
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
                    <div className='flight-price'>{flight.cheapestSeatPrice}р.</div>
                  </div>
                  <div>
                    <button onClick={() => handleBuyTicket(flight.id)}>
                      Купить
                    </button>
                  </div>
                  </div>
              ))}
        </div>
        </div>
      )}
    </div>
  );
}

export default FlightSearch;
