import axios from "axios";
import { UserProfileToken } from "../Models/User";
import { toast } from "react-toastify";

const api = "http://localhost:5274/api/Account/";

export const login = async (email: string, password: string) => {
  try {
    const data = await axios.post<UserProfileToken>(api + "Login", {
      email,
      password,
    });
    if (data) {
      return data.data;
    }
  } catch (error:any) {
    console.log("Login error:",  error.response.data.errors);
    throw error;
  }
};

export const registerForUser = async (
  email: string,
  username: string,
  password: string,
  role:string
) => {
  try {
    const data = await axios.post<UserProfileToken>(api + "Register", {
      email,
      username,
      password,
      role
    });
    if (data) {
      return data
    }
  } catch (error: any) {
    console.log(error);
    console.log("Register error:", error);
    throw error;
  }
};
