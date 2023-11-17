import React from 'react';
import { Link } from 'react-router-dom';

function NavItem({ to, children }) {
  return (
    <li>
      <Link to={to}>{children}</Link>
    </li>
  );
}

export default NavItem;
