import { WritableSignal } from '@angular/core';
export interface IProduct {
    id: number;
    name: string;
    description: string;
    price: number;
    stockQuantity: number;
    productImages: {image:string}[];
    productBrand: string;
    productSubCategory: string;
    // rating: IRating,
    currentImageIndex: WritableSignal<number>;
    IsLoved: WritableSignal<boolean>;
}

export interface IRating{
    rate: number,
    count: number
}