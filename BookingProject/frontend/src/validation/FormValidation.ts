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