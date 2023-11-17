import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../Styles/RegistrationStyle.css';

function FlightsRegistration() {
  const [ticketId, setTicketId] = useState('');
  const navigate = useNavigate();

  const handleRegistration = (e) => {
    e.preventDefault();
    navigate(`/register/${ticketId}`);
  };

  return (
    <div className='registration'>
      <form onSubmit={(e) => handleRegistration(e)}>
        <div className="input-group-registration">
          <input
            required
            type="text"
            value={ticketId}
            onChange={(e) => setTicketId(e.target.value)}
            placeholder="Номер билета"
          />
        </div>
        <button className='registration-btn' type="submit">
          Поиск
        </button>
      </form>
    </div>
  );
}

export default FlightsRegistration;
