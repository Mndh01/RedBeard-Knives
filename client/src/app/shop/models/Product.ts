import { Photo } from "../../shared/models/Photo";
import { Review } from "../../shared/models/Review";

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
    reviews: Review[];
}

export interface Category {
    id: number;
    name: string;
}