import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { ApartamentPostDto } from "../Models/Apartament";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { ApartamentValidation } from "../validation/FormValidation";
import { createApartament } from "../Services/ApartamentService";
import { useAuth } from "../Context/useAuth";

type CreateApartamentInputs = {
  address: string;
  area: number;
  latitude: number;
  longtitude: number;
  bedrooms: number;
};

const CreateApartament = () => {
    const{token} = useAuth();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<CreateApartamentInputs>({
    resolver: yupResolver(ApartamentValidation),
  });

  const handleCreate = async(form: CreateApartamentInputs) => {
    await createApartament(form, token!);
  };

  return (
    <div className="max-w-md mx-auto mt-10 p-6 bg-white shadow-md rounded-lg">
      <h2 className="text-2xl font-bold mb-6 text-center">Create Apartament</h2>
      <form onSubmit={handleSubmit(handleCreate)}>
        <div className="mb-4">
          <label className="block text-gray-700 font-medium mb-2">
            Address
          </label>
          <input
            type="text"
            required
            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:border-pink-500"
            {...register("address")}
          />
          {errors.address ? (
            <p className="text-red-500">{errors.address.message}</p>
          ) : (
            ""
          )}
        </div>
        <div className="mb-4">
          <label className="block text-gray-700 font-medium mb-2">
            Area (m²)
          </label>
          <input
            type="number"
            required
            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:border-pink-500"
            {...register("area")}
          />
          {errors.area ? (
            <p className="text-red-500">{errors.area.message}</p>
          ) : (
            ""
          )}
        </div>
        <div className="mb-4">
          <label className="block text-gray-700 font-medium mb-2">
            Latitude
          </label>
          <input
            type="number"
            required
            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:border-pink-500"
            {...register("latitude")}
          />
          {errors.latitude ? (
            <p className="text-red-500">{errors.latitude.message}</p>
          ) : (
            ""
          )}
        </div>
        <div className="mb-4">
          <label className="block text-gray-700 font-medium mb-2">
            Longtitude
          </label>
          <input
            type="number"
            {...register("longtitude")}
            required
            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:border-pink-500"
          />
          {errors.longtitude ? (
            <p className="text-red-500">{errors.longtitude.message}</p>
          ) : (
            ""
          )}
        </div>
        <div className="mb-4">
          <label className="block text-gray-700 font-medium mb-2">
            Bedrooms
          </label>
          <input
            type="number"
            {...register("bedrooms")}
            required
            className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:border-pink-500"
          />
          {errors.bedrooms ? (
            <p className="text-red-500">{errors.bedrooms.message}</p>
          ) : (
            ""
          )}
        </div>
        <button
          type="submit"
          className="w-full bg-pink-500 text-white py-2 px-4 rounded-lg hover:bg-pink-600 focus:outline-none"
        >
          Create
        </button>
      </form>
    </div>
  );
};

export default CreateApartament;
