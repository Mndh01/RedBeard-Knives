import { Address } from "src/app/shared/models/Address";
import { OrderItem } from "src/app/shared/models/Order";
import { DeliveryMethod } from "src/app/shared/models/deliveryMethod";

export interface OrderForAdmin {
    id: number;
    buyerEmail: string;
    orderDate: Date;
    shipToAddress: Address;
    deliveryMethod: DeliveryMethod;
    shippingPrice: number;
    orderItems: OrderItem[];
    subtotal: number;
    total: number;
    status: string;
}