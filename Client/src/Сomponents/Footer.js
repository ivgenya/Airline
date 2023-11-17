import React from 'react';

const Footer = () => {
  return (
    <footer style={{
      background: '#333',
      padding: '20px',
      textAlign: 'center',
      color: '#fff',
      position: 'fixed',
      marginTop:'50px',
      bottom: '0',
      width: '100%'
    }}>
      <p>&copy; 2023 Airland. Все права защищены.</p>
    </footer>
  );
}

export default Footer;
