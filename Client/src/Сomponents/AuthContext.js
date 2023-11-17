import React, { createContext, useContext, useState } from 'react';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [isUserLoggedIn, setIsUserLoggedIn] = useState(!!localStorage.getItem('jwtToken'));

  const login = () => {
    setIsUserLoggedIn(true);
  };

  const logout = () => {
    setIsUserLoggedIn(false);
  };

  return (
    <AuthContext.Provider value={{ isUserLoggedIn, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};