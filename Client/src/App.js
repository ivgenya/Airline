import React from 'react';
import { BrowserRouter } from "react-router-dom"
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Flights from './Сomponents/Flights';
import SearchFlight from './Сomponents/SearchFlight';
import Schedule from './Сomponents/Schedule';
import FlightDetails from './Сomponents/FlightDetails';
import CreateFlight from './Сomponents/CreateFlight';
import ScheduleDetails from './Сomponents/ScheduleDetails';
import CreateSchedule from './Сomponents/CreateSchedule';
import CreateUser from './Сomponents/CreateUser';
import Navigation from './Сomponents/Navigation';
import Users from './Сomponents/Users';
import UserDetails from './Сomponents/UserDetails';
import Login from './Сomponents/Login';
import Logout from './Сomponents/Logout';
import FlightsBoard from './Сomponents/FlightsBoard';
import TicketPurchase from './Сomponents/TicketPurchase';
import TicketBooking from './Сomponents/TicketBooking';
import { AuthProvider } from './Сomponents/AuthContext';
import FlightsRegistration from './Сomponents/FlightRegistration';
import MyBooking from './Сomponents/MyBooking';
import MyBookingDetails from './Сomponents/MyBookingDetails';
import AllBookings from './Сomponents/AllBookings';
import AllTickets from './Сomponents/AllTickets';
import CancelBooking from './Сomponents/CancelBooking';
import Payment from './Сomponents/Payment';
import Registration from './Сomponents/Registration';
import RegistrationDetails from './Сomponents/RegistrationDetails';
import Tabs from './Сomponents/Tabs';

function App() {
  return (
    <BrowserRouter>
    <AuthProvider>
      <Navigation/>
      <Routes>
        <Route path="/board" element={<FlightsBoard />} />
        <Route path="/search" element={<SearchFlight />} />
        <Route path="/flights" element={<Flights />} />
        <Route path="/users" element={<Users />} />
        <Route path="/flight/:id" element={<FlightDetails/>} />
        <Route path="/flight/create" element={<CreateFlight/>} />
        <Route path="/schedule" element={<Schedule />} />
        <Route path="/schedule/:id" element={<ScheduleDetails/>} />
        <Route path="/user/:username" element={<UserDetails/>} />
        <Route path="/schedule/create" element={<CreateSchedule/>} />
        <Route path="/user/create" element={<CreateUser/>} />
        <Route path="/register" element={<Registration/>} />
        <Route path="/login" element={<Login/>} />
        <Route path="/logout" element={<Logout/>} />
        <Route path="/buy/:flightId" element={<TicketPurchase />} />
        <Route path="/book/:ticketId" element={<TicketBooking />} />
        <Route path="/" element={<Tabs />} />
        <Route path="/register" element={<FlightsRegistration />} />
        <Route path="/register/:ticketId" element={<RegistrationDetails />} />
        <Route path="/booking" element={<MyBooking />} />
        <Route path="/allbookings" element={<AllBookings />} />
        <Route path="/alltickets" element={<AllTickets />} />
        <Route path="/pay/:ticketId" element={<Payment />} />
        <Route path="/cancel/:bookingId" element={<CancelBooking />} />
        <Route path="/booking/:bookingId" element={<MyBookingDetails />} />
      </Routes>
      </AuthProvider>
      </BrowserRouter>
  );
}

export default App;