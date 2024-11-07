import { createContext, useEffect, useState } from "react";
import { UserProfile } from "../Models/User";
import { useNavigate } from "react-router-dom";
import { login, register } from "../Services/AuthService";
import { toast } from "react-toastify";
import React from "react";
import axios from "axios";

type UserContextType = {
  user: UserProfile | null;
  token: string | null;
  registerUser: (email: string, username: string, password: string) => void;
  loginUser: (email: string, password: string) => void;
  logout: () => void;
  isLoggedIn: () => boolean;
};

type Props = { children: React.ReactNode };

const UserContext = createContext<UserContextType>({} as UserContextType);

export const UserProvider = ({ children }: Props) => {
  const navigate = useNavigate();
  const [token, setToken] = useState<string | null>(null);
  const [user, setUser] = useState<UserProfile | null>(null);
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const user = localStorage.getItem("user");
    const token = localStorage.getItem("token");
    if (user && token) {
      setUser(JSON.parse(user));
      setToken(token);
      axios.defaults.headers.common["Authorization"] = "Bearer" + token;
    }
    setIsReady(true);
  }, []);

  const registerUser = async (
    email: string,
    username: string,
    password: string
  ) => {
    await register(email, username, password)
      .then((res) => {
        if (res && res.data) {
          localStorage.setItem("token", res.data.token);
          const userObj = {
            userName: res.data.userName,
            email: res.data.email,
          };
          localStorage.setItem("user", JSON.stringify(userObj));
          setToken(res.data.token);
          setUser(userObj);
          toast.success("Register Success");
          navigate("/");
        } else {
          toast.error("Registration failed. Please try again.");
        }
      })
      .catch((error) => {
        if (
          error.response &&
          error.response.data &&
          error.response.data.errors
        ) {
          if (Array.isArray(error.response.data.errors)) {
            error.response.data.errors.forEach((err: string) => {
              toast.error(err);
            });
          } else {
            toast.error(error.response.data.errors);
          }
        } else {
          toast.error("An unexpected error occurred. Please try again.");
        }
      });
  };

  const loginUser = async (username: string, password: string) => {
    await login(username, password)
      .then((res) => {
        if (res) {
          localStorage.setItem("token", res?.token);
          const userObj = {
            userName: res?.userName,
            email: res?.email,
          };
          localStorage.setItem("user", JSON.stringify(userObj));
          setToken(res?.token!);
          setUser(userObj!);
          toast.success("Register Success");
          navigate("/");
        }
      })
      .catch((error) => {
        if (
          error.response &&
          error.response.data &&
          error.response.data.errors
        ) {
          if (Array.isArray(error.response.data.errors)) {
            error.response.data.errors.forEach((err: string) => {
              toast.error(err);
            });
          } else {
            toast.error(error.response.data.errors);
          }
        } else {
          toast.error("An unexpected error occurred. Please try again.");
        }
      });
  };

  const isLoggedIn = () => {
    return !!user; // if user be it will return true
  };

  const logout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
    setToken(null);
    navigate("/");
  };

  return (
    <UserContext.Provider
      value={{ loginUser, user, token, logout, isLoggedIn, registerUser }}
    >
      {isReady ? children : null}
    </UserContext.Provider>
  );
};

export const useAuth = () => React.useContext(UserContext);
