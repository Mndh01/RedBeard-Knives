import { Photo } from "./Photo";

export interface Product {
    id: number;
    name: string;
    category: string;
    description: string;
    photoUrl: string;
    price: number;
    inStock: number;
    soldItems: number;
    photos: Photo[];
}