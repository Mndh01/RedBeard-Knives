import { Photo } from "./Photo";

export interface Product {
    id: number;
    name: string;
    category: Category;
    description: string;
    photoUrl: string;
    price: number;
    inStock: number;
    soldItems: number;
    photos: Photo[];
}

export interface Category {
    id: number;
    name: string;
}