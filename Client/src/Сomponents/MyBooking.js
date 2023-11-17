import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../Styles/RegistrationStyle.css';

function MyBooking() {
  const [bookingId, setTicketId] = useState('');
  const navigate = useNavigate();

  const handleBooking = (e) => {
    e.preventDefault();
    navigate(`/booking/${bookingId}`);
  };

  return (
    <div className='registration'>
      <form onSubmit={(e) => handleBooking(e)}>
        <div className="input-group-registration">
          <input
            required
            type="text"
            value={bookingId}
            onChange={(e) => setTicketId(e.target.value)}
            placeholder="Номер бронирования"
          />
        </div>
        <button className='registration-btn' type="submit">
          Поиск
        </button>
      </form>
    </div>
  );
}

export default MyBooking;
