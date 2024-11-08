import React from "react";
import { useAuth } from "../Context/useAuth"; // Переконайтеся, що шлях до вашого хука useAuth правильний
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";

const Header = () => {
  const { user, logout } = useAuth(); // Передбачаємо, що useAuth має функцію logout
  const navigate = useNavigate();

  const handleLogout = () => {
    logout(); // Викликаємо функцію виходу з контексту
    navigate("/login"); // Перенаправлення на сторінку входу після виходу
  };

  return (
    <header className="flex items-center justify-between px-6 py-4 shadow-md bg-white">
      <div className="flex items-center space-x-2">
        <img
          src="https://1000logos.net/wp-content/uploads/2017/08/Airbnb-logo.jpg"
          alt="Airbnb logo"
          className="h-8"
        />
      </div>

      {/* Профіль та інші іконки */}
      <div className="flex items-center space-x-4">
        {user?.role === "Host" && (
          <Link to={"/create-apartament"} className="text-gray-600 hidden md:inline">
            Apartament for rent
          </Link>
        )}
        {/* Кнопка Вийти */}
        {user && (
          <button
            onClick={handleLogout}
            className="text-white bg-pink-500 px-4 py-2 rounded-full hover:bg-pink-600"
          >
            Exit 
          </button>
        )}
      </div>
    </header>
  );
};

export default Header;
