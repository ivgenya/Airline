import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import '../Styles/RegistrationStyle.css';

function MyBookingDetails() {
  const navigate = useNavigate();
  const [booking, setBooking] = useState([]);
  const { bookingId } = useParams();
  const [error, setError] = useState(null);

  useEffect(() => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    axios
      .get(`https://localhost:7125/api/ticket/GetTicketDetailsByBooking/${bookingId}`, { headers })
      .then((response) => {
        setBooking(response.data);
      })
      .catch((error) => {
        setError('Ошибка получения данных');
      });
  }, [bookingId]);

  const handleBookings = () => {
    navigate(`/allbookings`);
  }


  return (
    <div className="container-register">
      <div className="container-register-info">
        <h2>Детали бронирования</h2>
        {booking.map((ticket, index) => (
          <div key={index}>
            <p><strong>Фамилия:</strong> {ticket.surname}</p>
            <p><strong>Имя:</strong> {ticket.name}</p>
            <p><strong>Номер документа:</strong> {ticket.documentNumber}</p>
            <p><strong>Дата рождения:</strong> {ticket.dateOfBirth}</p>
            <p><strong>Пол:</strong> {ticket.gender}</p>
            <p><strong>Email:</strong> {ticket.email}</p>
            <p><strong>Рейс:</strong>   {ticket.shortName}{ticket.number}   {ticket.depCity}-{ticket.arrCity}  {new Date(ticket.date).toLocaleDateString()}</p>
            <p><strong>Место:</strong> {ticket.seat}</p>
            <p><strong>Статус:</strong> {ticket.bookingStatus}</p>
          </div>
        ))}
      </div>
      {booking.length <= 0 && booking[0].bookingStatus === "paid" && (
        <div className="error-message">
          <p>{error}</p>
        </div>
      )}
      <button onClick={handleBookings}>Перейти к моим бронированиям</button>
    </div>
  );
}

export default MyBookingDetails;
