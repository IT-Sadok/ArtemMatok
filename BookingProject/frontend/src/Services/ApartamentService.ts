import axios from "axios";
import { ApartamentGetDto, ApartamentPostDto } from "../Models/Apartament";
import { useAuth } from "../Context/useAuth";
import { toast } from "react-toastify";
import { PageResultResponse } from "../Models/Result";

const api = "http://localhost:5274/api/Apartament/";

export const createApartament = async(apartamentDto:ApartamentPostDto, token:string) => {
    try {
        console.log("start");
        const response = await axios.post(api + "CreateApartament", apartamentDto, {
          headers: {
            "Authorization": `Bearer ${token}`, 
            "Content-Type": "application/json"
          }
        });
        if(response){
            toast.success("Created!");
            return response.data;

        }
        
      } catch (error: any) {
        console.log("Create Apartament error:", error);
        throw error;
      }
}

export const getApartaments = async (filterParams = {}) => {
  try {
    const response = await axios.get<PageResultResponse<ApartamentGetDto>>(api + "GetApartaments", {
      params: filterParams, 
    });
    return response.data; 
  } catch (error) {
    console.error("Error fetching apartments:", error);
    toast.error("Failed to fetch apartments.");
    throw error;
  }
};

