import * as Yup from "yup";

export const RegisterValidation = Yup.object().shape({
    userName: Yup.string().min(2).required("Username is required"),
    password: Yup.string().min(8).required("Password is required"),
    email: Yup.string().required("Email is required"),
})

export const LoginValidation = Yup.object().shape({
    password: Yup.string().min(8).required("Password is required"),
    email: Yup.string().required("Email is required"),
})

export const ApartamentValidation = Yup.object().shape({
    address: Yup.string()
      .required("Address is required")
      .min(5, "Address should be at least 5 characters"),
    area: Yup.number()
      .required("Area is required")
      .min(1, "Area must be at least 1 m²"),
    latitude: Yup.number()
      .required("Latitude is required")
      .min(-90, "Latitude must be between -90 and 90")
      .max(90, "Latitude must be between -90 and 90"),
    longtitude: Yup.number()
      .required("Longitude is required")
      .min(-180, "Longitude must be between -180 and 180")
      .max(180, "Longitude must be between -180 and 180"),
    bedrooms: Yup.number()
      .required("Number of bedrooms is required")
      .min(1, "There must be at least 1 bedroom")
      .max(10, "Number of bedrooms cannot exceed 10"),
  });