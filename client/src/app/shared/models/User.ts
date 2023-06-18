import { Address } from "./Address";

export interface User {
    userName: string;
    sureName: string;
    email: string;
    gender: string;
    phoneNumber: string;
    token: string;
    dateOfBirth: Date;
    photoUrl: string;
    addresses: Address[];
    roles: string[];
}

// For future use
export interface UserFormValues {
    userName: string;
    sureName: string;
    password: string;
    phoneNumber: string;
}