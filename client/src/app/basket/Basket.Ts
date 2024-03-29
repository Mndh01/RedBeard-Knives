import { v4 as uuidv4 } from 'uuid'
import { Category } from '../shop/models/Product';

export interface IBasket {
    id: string,
    items: IBasketItem[];
    clientSecret?: string;
    paymentIntentId?: string;
    deliveryMethodId?: number;
    shippingPrice: number;
}

export interface IBasketItem {
    id: number;
    productName: string;
    price: number;
    quantity: number;
    photoUrl: string;
    category: Category;
}

export class Basket implements IBasket {
    id = uuidv4();
    items: IBasketItem[] = [];
    shippingPrice = 0;
}

export interface IBasketTotals {
    shipping: number;
    subtotal: number;
    total: number;
}