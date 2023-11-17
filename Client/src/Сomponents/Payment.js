import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../Styles/TicketStyle.css';
import { useParams } from 'react-router-dom';
import '../Styles/style.css';

function Payment() {
  const [ticket, setTicket] = useState({});
  const { ticketId } = useParams();
  const [fileLink, setFileLink] = useState(null);
  const [error, setError] = useState(null);

  const [paymentInfo, setPaymentInfo] = useState({
    cardNumber: '',
    expirationDate: '',
    cvv: '',
  });


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
    <div className='container'>
      {!fileLink && (
        <div>
        <div className='container-payment'>
      <h2>Оплата</h2>
      <div>
        <label className="input-label">Номер карты:</label>
        <input
          className="input-field"
          type="text"
          value={paymentInfo.cardNumber}
          onChange={(e) => setPaymentInfo({ ...paymentInfo, cardNumber: e.target.value })}
        />
      </div>
      <div>
        <label className="input-label">Дата выдачи:</label>
        <input
          className="input-field"
          type="text"
          value={paymentInfo.expirationDate}
          onChange={(e) => setPaymentInfo({ ...paymentInfo, expirationDate: e.target.value })}
        />
      </div>
      <div>
        <label className="input-label">CVV:</label>
        <input
          className="input-field"
          type="text"
          value={paymentInfo.cvv}
          onChange={(e) => setPaymentInfo({ ...paymentInfo, cvv: e.target.value })}
        />
      </div>
      </div>
      <button className='' onClick={handlePayment}>Оплатить</button>
      </div>
      )}
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

export default Payment;
