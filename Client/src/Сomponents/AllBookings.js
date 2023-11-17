import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import '../Styles/AllTickets.css';
import { AuthProvider } from './AuthContext';
import { useAuth } from './AuthContext';

function AllBookings() {
  const [bookings, setBookings] = useState([]);
  const navigate = useNavigate();
  const { isUserLoggedIn } = useAuth(AuthProvider);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const jwtToken = localStorage.getItem('jwtToken');
        const headers = {
          Authorization: `Bearer ${jwtToken}`,
        };
        const response = await axios.get('https://localhost:7125/api/UserTickets/GetAllUserBookings', { headers });
        setBookings(response.data);
      } catch (error) {
        console.error('Ошибка получения данных:', error);
      }
    };
    fetchData();
  }, []);

  const handleCancelBooking = (bookingId) => {
    if (isUserLoggedIn) {
      navigate(`/cancel/${bookingId}`);
    } else {
      navigate(`/login`);
    }
  };

  const handlePayBooking = (ticketId) => {
    if (isUserLoggedIn) {
      navigate(`/pay/${ticketId}`);
    } else {
      navigate(`/login`);
    }
  };

  return (
    <div className='all-tickets'>
      <h2>Мои бронирования</h2>
      <div className='all-tickets-info'> 
        {bookings.length > 0 ? (
          bookings.map((booking, index) => (
            <div key={index} className='ticket-card'> 
              <div>
          <div>Код: {booking.ticketCode}</div>
          <div>Статус: {booking.ticketStatus}</div>
          <div>Фамилия: {booking.surname}</div>
            <div>Имя: {booking.name}</div>
            <div>Документ: {booking.documentNumber}</div>
            <div>Дата рождения: {new Date(booking.dateOfBirth).toLocaleDateString()}</div>
            <div>Пол: {booking.gender}</div>
            <div>Email: {booking.email}</div>
            <div>Место: {booking.seat}</div>
            </div>
            <div>{booking.shortName}{booking.number}</div>
                  <div className='ticket-flight-name'>
                  <div>{booking.depShortName} ({booking.depCity})</div>
                  <ion-icon name="airplane-outline"></ion-icon>
                  <div>{booking.arrShortName} ({booking.arrCity})</div>
                  <div>{new Date(booking.date).toLocaleDateString()}</div>
                  <div><ion-icon name="calendar-outline"></ion-icon></div>
                  </div>
                  <div>
                  <div>{booking.departureTime} - {booking.arrivalTime}</div>
                  <div>В пути - {booking.flightDuration}</div>
                  </div>
                  <div>
                    <div>Бронирование</div>
                    <div>Код:</div>
                    <div>{booking.bookingCode}</div>
                    <div>Дата: {booking.bookingDate != null ? new Date(booking.bookingDate).toLocaleDateString() : null}</div>
            <div>Статус: {booking.bookingStatus}</div>
            </div>
            {booking.bookingStatus === 'confirmed' && (
                  <div className='booking-buttons'>
                    <button onClick={() => handleCancelBooking(booking.bookingId)}>Отменить бронирование</button>
                    <button onClick={() => handlePayBooking(booking.ticketId)}>Оплатить бронирование</button>
                  </div>
                )}
            
        </div>
          ))
        ) : (
          <p className='no-tickets-message'>Ничего не найдено</p>
        )}
      </div>
    </div>
  );
}

export default AllBookings;
