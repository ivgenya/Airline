import React, { useState } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import '../Styles/RegistrationStyle.css';

function CancelBooking() {
  const navigate = useNavigate();
  const { bookingId } = useParams();
  const [error, setError] = useState(null);
  const [isCancel, setIsCancel] = useState(false);

  const handleCancellation = async () => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };
    try {
      await axios.post(`https://localhost:7125/api/Ticket/cancel/${bookingId}`, null, { headers });
      setIsCancel(true);
    } catch (error) {
      if (error.response.status === 400 || error.response.status === 404 || error.response.status === 401) {
        setError('Ошибка при отмене бронирования. Пожалуйста, повторите попытку позже.');
      }
    }
  };

  return (
    <div className="container-register">
      {!isCancel && (<div>
        <p>Вы действительно хотите отменить бронирование?</p>
      <button onClick={handleCancellation}>Отменить бронирование</button>
      </div>)}
      {isCancel && (
        <p>Бронирование отменено</p>
      )}
      {error && <p className="error-message">{error}</p>}
    </div>
  );
}

export default CancelBooking;
