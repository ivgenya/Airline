import React from 'react';
import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import '../Styles/BookStyle.css';
import axios from 'axios';

function TicketBooking() {
    const [bookingId, setBookingId] = useState(null);
    const [ticket, setTicket] = useState({});
    const { ticketId } = useParams();


    useEffect(() => {
        const jwtToken = localStorage.getItem('jwtToken');
        const headers = {
          Authorization: `Bearer ${jwtToken}`,
        };
    
        axios
          .get(`https://localhost:7125/api/ticket/GetTicketDetailsById/${ticketId}`, { headers })
          .then((response) => {
            setTicket(response.data);
          })
          .catch((error) => {
            console.error('Ошибка получения данных:', error);
          });
      }, [ticketId]);
    
    const handleBooking = async () => {
        const jwtToken = localStorage.getItem('jwtToken');
        const headers = {
          Authorization: `Bearer ${jwtToken}`,
        };
        try {
            const response = await axios.post(`https://localhost:7125/api/ticket/book/${ticketId}`,null, { headers });
            if (response.status === 200) {
              setBookingId(response.data); 
              
            } else {
              console.error('Ошибка при бронировании билета');
            }
          } catch (error) {
            console.error('Произошла ошибка при отправке запроса:', error);
          }
      };

  return (
    <div className="container-book">
      {!bookingId && (
        <div>
      <div className="container-book-info">
      <h2>Детали билета</h2>
      <p><strong>Фамилия:</strong> {ticket.surname}</p>
      <p><strong>Имя:</strong> {ticket.name}</p>
      <p><strong>Номер документа:</strong> {ticket.documentNumber}</p>
      <p><strong>Дата рождения:</strong> {ticket.dateOfBirth}</p>
      <p><strong>Пол:</strong> {ticket.gender}</p>
      <p><strong>Email:</strong> {ticket.email}</p>
      <p><strong>Место:</strong> {ticket.seat}</p>
      </div>
      <button onClick={() => handleBooking()}>Забронировать</button>
      </div>
      )}
      {bookingId && (
        <div className='container-book-info'>
          <h2>Билет успешно забронирован</h2>
          <p>У вас есть 1 час, чтобы оплатить билет. Для оплаты перейдите в раздел Мои бронирования.</p>
        </div>
      )}
    </div>

  );
}

export default TicketBooking;
