import React from 'react';
import NavItem from './NavItem';
import { NavLink } from "react-router-dom"
import { useState, useEffect } from 'react';
import '../Styles/style.css';
import { AuthProvider } from './AuthContext';
import { useAuth } from './AuthContext';
import axios from 'axios';


function Navigation() {
  const { isUserLoggedIn } = useAuth(AuthProvider);
  const [userRole, setUserRole] = useState('undefined'); 

  useEffect(() => {
    if (isUserLoggedIn) {
      const jwtToken = localStorage.getItem('jwtToken');
      const headers = {
        Authorization: `Bearer ${jwtToken}`,
      };

      axios
        .get('https://localhost:7125/api/User/GetUserRole', { headers })
        .then((response) => {
          const role = response.data;
          setUserRole(role); 
        })
        .catch((error) => {
          console.error('Ошибка получения роли пользователя:', error);
        });
    }
  }, [isUserLoggedIn]);

  return (
    <nav>
      <ul className='navigation'>
      <div className='logo'>
        <li>
          <NavLink to="/"><img src="/img/icon.png" alt="Логотип" /></NavLink>
        </li>
        </div>
        <div className='without-logo'>
        {isUserLoggedIn  ? (
          <NavItem to="/logout">Выйти</NavItem>
        ) : (
          <NavItem to="/login">Войти</NavItem>
        )}
        {!isUserLoggedIn && (
          <NavItem to="/register">Зарегистрироваться</NavItem>
        )} 
        {isUserLoggedIn && userRole === 'client' && (
          <NavItem to="/allbookings">Мои бронирования</NavItem>
        )}
        {isUserLoggedIn && userRole === 'client' && (
          <NavItem to="/alltickets">Мои билеты</NavItem>
        )}
        {isUserLoggedIn && userRole === 'dispatcher' && (
          <NavItem to="/flights">Рейсы</NavItem>
        )}
        {isUserLoggedIn && userRole === 'dispatcher' && (
          <NavItem to="/schedule">Расписание</NavItem>
        )}
        {isUserLoggedIn && userRole === 'admin' && (
          <NavItem to="/users">Пользователи</NavItem>
        )}
        </div>
        
      </ul>
    </nav>
  );
}

export default Navigation;
