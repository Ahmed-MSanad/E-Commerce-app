export interface IBasket {
    id : string,
    basket : IBasketItem[]
}
export interface IBasketItem{
    id : number,
    name : string,
    description : string,
    price : number,
    basketQuantity : number,
    brand : string,
    type : string,
    productImages : string[]
}