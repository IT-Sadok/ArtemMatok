import React from "react";
import logo from "./logo.svg";
import "./App.css";
import { Outlet, useLocation } from "react-router";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import {  UserProvider } from "./Context/useAuth";
import Header from "./Components/Header";

function App() {
  const location = useLocation();
  
  const hideHeaderRoutes = ["/register", "/login"]; // Додайте сюди всі шляхи, де не хочете показувати Header
  const shouldShowHeader = !hideHeaderRoutes.includes(location.pathname);

  return (
    <UserProvider>
      {shouldShowHeader && <Header />}
      <Outlet />
      <ToastContainer />
    </UserProvider>
  );
}

export default App;
