import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import '../Styles/RegistrationStyle.css';

function RegistationDetails() {

  const navigate = useNavigate();
  const [ticket, setTicket] = useState({});
  const [fileLink, setFileLink] = useState(null);
  const { ticketId } = useParams();
  const [error, setError] = useState(null);
  const [ticketExist, setTicketExist] = useState(false);

  useEffect(() => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };

    axios
      .get(`https://localhost:7125/api/ticket/GetTicketDetails/${ticketId}`, { headers })
      
      .then((response) => {
        console.log("dkdkdkdk"+response.status);
        setTicket(response.data);
        setTicketExist(true);
      })
      .catch((error) => {
          setError(error);
      });
  }, [ticketId]);
  
  const handleRegistration = async(e) => {
    e.preventDefault();
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };
    try {
      const response = await axios.post(`https://localhost:7125/api/ticket/register?ticketCode=${ticketId}`,null,
      { headers: headers, responseType: 'arraybuffer' });

      if (response.status === 200) {
        const blob = new Blob([response.data], { type: 'application/pdf' });
        const fileURL = URL.createObjectURL(blob);
        setFileLink(fileURL);
        setError(null);
      }
    } catch (error) {
      if (error.response && (error.response.status === 400 || error.response.status === 500)) {
        const decoder = new TextDecoder('utf-8');
        const errorMessage = decoder.decode(error.response.data);
        setError('Ошибка при регистрации. ' + errorMessage);
      } else if (error.response && error.response.status === 401) {
        navigate(`/login`);
      }
    }
  }

  return (
    <div className="container-register">
        {!fileLink && ticketExist && (
      <div className="container-register-info">
      <h2>Детали билета</h2>
      <p><strong>Фамилия:</strong> {ticket.surname}</p>
      <p><strong>Имя:</strong> {ticket.name}</p>
      <p><strong>Номер документа:</strong> {ticket.documentNumber}</p>
      <p><strong>Пол:</strong> {ticket.gender}</p>
      <p><strong>Место:</strong> {ticket.seat}</p>
      <p><strong>Статус билета:</strong> {ticket.ticketStatus}</p>
      <h2>Детали рейса</h2>
      <p><strong>Номер:</strong> {ticket.shortName + ticket.number}</p>
      <p><strong>Откуда-куда:</strong> {ticket.depCity + " " + ticket.arrCity}</p>
      <p><strong>Дата и время отправления:</strong> {new Date(ticket.date).toLocaleDateString() + " "+ ticket.departureTime}</p>
      <p><strong>Терминал:</strong> {ticket.terminal}</p>
      </div>)}
      {error && (
        <div className="error-message">
          <p>{error}</p>
        </div>
      )}
      {!fileLink && ticketExist && (
        <button onClick={(e) => handleRegistration(e)}>Зарегистрироваться</button>
              )}
              {!ticketExist && (<p>Билет не найден</p>)}
      {fileLink && (
        <div>
        <h2>Вы успешно зарегистрировались на рейс</h2>
          <p>Ссылка на ваш посадочный талон:</p>
          <a href={fileLink} download="boarding-pass.pdf">
                Открыть посадочный талон
            </a>
        </div>
      )}
    </div>
  );
}

export default RegistationDetails;

