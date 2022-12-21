import { Address } from "./Address";

export interface User {
    id: number;
    firstName: string;
    sureName: string;
    email: string;
    gender: string;
    phoneNumber: string;
    token: string;
    dateOfBirth: Date;
    photoUrl: string;
    address: Address;
    roles: string[];
}