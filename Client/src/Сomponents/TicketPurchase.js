import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import '../Styles/TicketStyle.css';

function TicketPurchase() {
  const [passenger, setPassenger] = useState({
    Surname: '',
    Name: '',
    DocumentNumber: '',
    DateOfBirth: '',
    Gender: 'male',
    Email: '',
  });
  const [paymentInfo, setPaymentInfo] = useState({
    cardNumber: '',
    expirationDate: '',
    cvv: '',
  });
  const [availableSeats, setAvailableSeats] = useState([]);
  const [seatId, setSeatId] = useState('');
  const { flightId } = useParams();
  const [showPayment, setShowPayment] = useState(false);
  const [showBooking, setShowBooking] = useState(false);
  const [ticket, setTicket] = useState('');
  const [ticketId, setTicketId] = useState(null);
  const [isEditing, setIsEditing] = useState(true);
  const [isBooked, setIsBooked] = useState(false);
  const navigate = useNavigate();
  const [fileLink, setFileLink] = useState(null);
  const [error, setError] = useState(null);

  useEffect(() => {
    axios
      .get(`https://localhost:7125/api/ticket/seats/${flightId}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('jwtToken')}`,
        },
      })
      .then((response) => {
        setAvailableSeats(response.data);
      })
      .catch((error) => {
        console.error('Ошибка получения доступных мест:', error);
      });
  }, [flightId]);


  const createTicket = async (e) => {
    e.preventDefault();
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };
      const response = await axios.post(
        `https://localhost:7125/api/ticket/buy?flightId=${flightId}&seatId=${seatId}`,
        {
          Surname: passenger.Surname,
          Name: passenger.Name,
          DocumentNumber: passenger.DocumentNumber,
          DateOfBirth: passenger.DateOfBirth,
          Gender: passenger.Gender,
          Email: passenger.Email,
        },
        { headers }
      )      
      .then((response) => {
        setTicket(response.data);
        setTicketId(response.data.id);
        console.log(response.data.id);
        setIsBooked(response.data.status === 'booked');
        if (!isBooked) {
          setShowPayment(true);
        } else {
          navigate(`/book/${response.data.id}`);
        }
      })
      .catch((error) => {
        setError(error);
      });
  
  };

  const handlePayment = async () => {
    const jwtToken = localStorage.getItem('jwtToken');
    const headers = {
      Authorization: `Bearer ${jwtToken}`,
    };
    try {
      const response = await axios.post(
        `https://localhost:7125/api/ticket/pay/${ticketId}`,
        {
          creditCardNumber: paymentInfo.cardNumber,
          expiryDate: paymentInfo.expirationDate,
          cvc: paymentInfo.cvv,
        },
        { headers: headers, responseType: 'arraybuffer' }
      );

      if (response.status === 200) {
        const blob = new Blob([response.data], { type: 'application/pdf' });
        const fileURL = URL.createObjectURL(blob);
        setFileLink(fileURL);
        setIsEditing(false);
        setShowPayment(false);
      }
    } catch (error) {
      console.error('Ошибка:', error);
      if (
        error.response &&
        (error.response.status === 400 ||
          error.response.status === 404 ||
          error.response.status === 401)
      ) {
        setError(
          'Ошибка при регистрации. Пожалуйста, проверьте введенные данные или повторите попытку позже.'
        );
      }
    }
  };

  return (
    <div className="ticket-purchase">
      <h2>Покупка билета</h2>
      {!fileLink && (
        <form onSubmit={createTicket}>
          <div>
            <label className="input-label">Фамилия:</label>
            <input
              required
              disabled={!isEditing}
              className="input-field"
              type="text"
              value={passenger.Surname}
              onChange={(e) =>
                setPassenger({ ...passenger, Surname: e.target.value })
              }
            />
          </div>
          <div>
            <label className="input-label">Имя:</label>
            <input
              required
              disabled={!isEditing}
              className="input-field"
              type="text"
              value={passenger.Name}
              onChange={(e) =>
                setPassenger({ ...passenger, Name: e.target.value })
              }
            />
          </div>
          <div>
            <label className="input-label">Номер документа:</label>
            <input
              required
              disabled={!isEditing}
              className="input-field"
              type="text"
              value={passenger.DocumentNumber}
              onChange={(e) =>
                setPassenger({
                  ...passenger,
                  DocumentNumber: e.target.value,
                })
              }
            />
          </div>
          <div>
            <label className="input-label">Дата рождения:</label>
            <input
              required
              disabled={!isEditing}
              className="input-field"
              type="date"
              value={passenger.DateOfBirth}
              onChange={(e) =>
                setPassenger({ ...passenger, DateOfBirth: e.target.value })
              }
            />
          </div>
          <div>
            <label className="input-label">Пол:</label>
            <select
            required
              name="gender"
              value={passenger.Gender}
              disabled={!isEditing}
              className="select"
              onChange={(e) =>
                setPassenger({ ...passenger, Gender: e.target.value })
              }
            >
              <option value="male">Мужской</option>
              <option value="female">Женский</option>
            </select>
          </div>
          <div>
            <label className="input-label">Email:</label>
            <input
            required
              disabled={!isEditing}
              className="input-field"
              type="email"
              value={passenger.Email}
              onChange={(e) =>
                setPassenger({ ...passenger, Email: e.target.value })
              }
            />
          </div>
          <div>
            <label>Место:</label>
            <select
            required
              value={seatId}
              onChange={(e) => setSeatId(e.target.value)}
            >
              <option value={null}>Выберите место</option>
              {availableSeats.map((seat) => (
                <option key={seat.id} value={seat.id}>
                  {seat.number}
                </option>
              ))}
            </select>
          </div>
          {!showBooking && !showPayment ? (
            <div className='purchase-buttons'>
              <button className="purchase-button" type="submit">
                Оплатить
              </button>
              <button
                className="purchase-button"
                type="submit"
                onClick={(e) => setIsBooked(true)}
              >
                Забронировать
              </button>
            </div>
          ) : null}
        </form>
      )}
      {showPayment ? (
        <div>
          <h2>Оплата</h2>
          <div>
            <label className="input-label">Номер карты:</label>
            <input
            required
              className="input-field"
              type="text"
              value={paymentInfo.cardNumber}
              onChange={(e) =>
                setPaymentInfo({
                  ...paymentInfo,
                  cardNumber: e.target.value,
                })
              }
            />
          </div>
          <div>
            <label className="input-label">Срок действия:</label>
            <input
            required
              className="input-field"
              type="text"
              value={paymentInfo.expirationDate}
              onChange={(e) =>
                setPaymentInfo({
                  ...paymentInfo,
                  expirationDate: e.target.value,
                })
              }
            />
          </div>
          <div>
            <label className="input-label">CVV:</label>
            <input
            required
              className="input-field"
              type="text"
              value={paymentInfo.cvv}
              onChange={(e) =>
                setPaymentInfo({ ...paymentInfo, cvv: e.target.value })
              }
            />
          </div>
          <button className="purchase-button" onClick={handlePayment}>Оплатить</button>
        </div>
      ) : null}
      {fileLink && (
        <div>
          <h2>Билет успешно оплачен</h2>
          <p>Ссылка на ваш билет:</p>
          <a href={fileLink} download="ticket.pdf">
            Открыть билет
          </a>
        </div>
      )}
    </div>
  );
}

export default TicketPurchase;
