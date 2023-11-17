import React, { useState } from 'react';
import '../Styles/TabStyle.css';
import FlightsBoard from './FlightsBoard';
import Registration from './FlightRegistration';
import MyBooking from './MyBooking';
import SearchFlight from './SearchFlight';

const Tabs = () => {

    const [currentTab, setCurrentTab] = useState('1');
    const tabs = [
        {
            id: 1,
            tabTitle: 'Покупка', 
            title: 'Поиск рейсов',
            content: <FlightsBoard />
        },
        {
            id: 2,
            tabTitle: 'Регистрация на рейс',
            title: 'Регистрация на рейс',
            content: <Registration/>
        },
        {
            id: 3,
            tabTitle: 'Мои бронирования',
            title: 'Мои бронирования',
            content: <MyBooking/>
        },
        {
            id: 4,
            tabTitle: 'Статус рейса',
            title: 'Статус рейса',
            content: <SearchFlight/>
        },

    ];

    const handleTabClick = (e) => {
        setCurrentTab(e.target.id);
    }

    return (
        <div className='container-tab'>
            <div className='tabs'>
                {tabs.map((tab, i) =>
                    <button key={i} id={tab.id} disabled={currentTab === `${tab.id}`} onClick={(handleTabClick)}>{tab.tabTitle}</button>
                )}
            </div>
            <div className='content'>
                {tabs.map((tab, i) =>
                    <div key={i}>
                        {currentTab === `${tab.id}` && <div><p className='title'>{tab.title}</p><p>{tab.content}</p></div>}
                    </div>
                )}
            </div>
        </div>
    );
}

export default Tabs;