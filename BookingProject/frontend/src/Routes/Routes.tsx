import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import HomePage from "../Pages/HomePage";
import RegisterPage from "../Pages/RegisterPage";
import ProtectedRoutes from "./ProtectedRoutes";
import LoginPage from "../Pages/LoginPage";
import CreateApartament from "../Pages/CreateApartament";

export const router = createBrowserRouter([
    {
        path:"/",
        element:<App />,
        children: [
            {path:"", element:<ProtectedRoutes><HomePage /></ProtectedRoutes>},
            {path:"/register", element:<RegisterPage />},
            {path:"/login", element:<LoginPage />},
            {path:"/create-apartament", element:<CreateApartament />}
        ]
    }
])