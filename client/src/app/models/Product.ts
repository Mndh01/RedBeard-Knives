import { Photo } from "./Photo";

export interface Product {
    id: number;
    type: string;
    price: number;
    inStock: number;
    soldItems: number;
    photo: Photo[];
}