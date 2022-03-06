import { Photo } from "./Photo";

export interface Product {
    id: number;
    category: string;
    photoUrl: string;
    price: number;
    inStock: number;
    soldItems: number;
    photos: Photo[];
}