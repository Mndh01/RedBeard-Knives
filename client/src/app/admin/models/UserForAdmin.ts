import { Address } from "src/app/shared/models/Address";
import { OrderForAdmin } from "./OrderForAdmin";

export interface UserForAdmin {
    id: number;
    userName: string;
    sureName: string;
    email: string;
    phoneNumber: string;
    gender: string;
    dateOfBirth: Date;
    // photoUrl: string; TODO: Consider adding this
    addresses: Address[];
    orders: OrderForAdmin[];
    roles: string[];
}