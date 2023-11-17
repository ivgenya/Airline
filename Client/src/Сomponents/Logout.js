import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from './AuthContext';
import '../Styles/LoginStyle.css';

function Logout() {
    const navigate = useNavigate();
    const { logout } = useAuth();
    const handleLogout = () => {
    localStorage.removeItem('jwtToken');
    localStorage.setItem('isUserLoggedIn', 'false');
    logout();
    navigate(`/login`);
  };

  return (
    <div className='logout-container'>
      <h2>Вы действительно хотите выйти из системы?</h2>
      <button onClick={handleLogout}>Выйти</button>
    </div>
  );
}

export default Logout;
