import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../Styles/AllTickets.css';

function AllTickets() {
  const [tickets, setTickets] = useState([]);

  useEffect(() => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    axios
      .get('https://localhost:7125/api/UserTickets/GetAllUserTickets', { headers })
      .then((response) => {
        setTickets(response.data);
      })
      .catch((error) => {
        console.error('Ошибка получения данных:', error);
      });
  }, []);

  return (
    <div className='all-tickets'>
      <h2>Мои билеты</h2>
      <div className='all-tickets-info'>
      {tickets.length > 0 ? (
      tickets.map((ticket, index) => (
        <div className='ticket-card'>
          <div>
          <div>Код: {ticket.ticketCode}</div>
          <div>Статус: {ticket.ticketStatus}</div>
          <div>Фамилия: {ticket.surname}</div>
            <div>Имя: {ticket.name}</div>
            <div>Документ: {ticket.documentNumber}</div>
            <div>Дата рождения: {new Date(ticket.dateOfBirth).toLocaleDateString()}</div>
            <div>Пол: {ticket.gender}</div>
            <div>Email: {ticket.email}</div>
            <div>Место: {ticket.seat}</div>
            </div>
            <div>{ticket.shortName}{ticket.number}</div>
                  <div className='ticket-flight-name'>
                  <div>{ticket.depShortName} ({ticket.depCity})</div>
                  <ion-icon name="airplane-outline"></ion-icon>
                  <div>{ticket.arrShortName} ({ticket.arrCity})</div>
                  <div>{new Date(ticket.date).toLocaleDateString()}</div>
                  <div><ion-icon name="calendar-outline"></ion-icon></div>
                  </div>
                  <div>
                  <div>{ticket.departureTime} - {ticket.arrivalTime}</div>
                  <div>В пути - {ticket.flightDuration}</div>
                  </div>
                  <div>
                    <div>Бронирование</div>
                    <div>Код:</div>
                    <div>{ticket.bookingCode}</div>
                    <div>Дата: {ticket.bookingDate != null ? new Date(ticket.bookingDate).toLocaleDateString() : null}</div>
            <div>Статус: {ticket.bookingStatus}</div>
            </div>
        </div>
      ))
    ) : (
      <p className='no-tickets-message'>Ничего не найдено</p>
    )}        
    </div>
    </div>
  );
}

export default AllTickets;
