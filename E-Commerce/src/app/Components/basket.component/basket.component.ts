import { Component, computed, inject, OnInit, Signal } from '@angular/core';
import { BasketService } from '../../Core/Services/basket.service';
import { IBasket } from '../../Core/Interfaces/ibasket';
import { ProductService } from '../../Core/Services/product.service';
import { CommonModule, CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-basket',
  imports: [CurrencyPipe, CommonModule],
  templateUrl: './basket.component.html',
  styleUrl: './basket.component.scss'
})
export class BasketComponent implements OnInit{
  readonly _basketService = inject(BasketService);
  readonly _productService = inject(ProductService);

  totalCost : Signal<number> = computed(() => {
    let total = 0;
    this._basketService.userBasket().basket.map((item) => total += (item.price * item.basketQuantity))
    return total;
  });

  ngOnInit(){
    // this.GetBasket();
  }

  GetBasket(){
    try{
        this._basketService.getBasket(this._basketService.userBasketId).subscribe({
          next: (basket : IBasket) => {
            console.log("basket got successfully: ", basket);
            this._basketService.userBasket.set(basket);
            console.log("user basket:", this._basketService.userBasket().basket);
          },
          error: (error) => {console.error(`Error getting the basket: ${error}`);}
        });
    }
    catch(error){
      alert(error);
    }
  }
  RemoveFromBasket(productId: number) {
    this._basketService.userBasket.update(oldBasket => {
      const index = oldBasket.basket.findIndex(item => item.id == productId);
      if (index !== -1) {
        if(oldBasket.basket[index].basketQuantity == 1){
          let newBasket : IBasket = {id: oldBasket.id, basket: [...oldBasket.basket.slice(0, index), ...oldBasket.basket.slice(index + 1)]}
          return newBasket;
        }
        else{
          let newBasket : IBasket = {id: oldBasket.id, basket: [
            ...oldBasket.basket.slice(0, index), 
            {...oldBasket.basket[index], basketQuantity: oldBasket.basket[index].basketQuantity - 1},
            ...oldBasket.basket.slice(index + 1)
          ]};
          return newBasket; // doing so will fire the signal to set the new total basket cost in the UI,, can be done also by changing the settings of the signal to allow it to change when a deep change happens
          // oldBasket.basket[index].basketQuantity--; // just simply returning the old basket won't fire the total basket cost as we change deep in the signal at the item quantity
        }
      }
      return oldBasket;
    });
    this._basketService.setToBasket();
  }
  RemoveUserBasket(){
    this._basketService.removeBasket(this._basketService.userBasketId).subscribe({
      next: _ => {
        console.log(`Basket with id: ${this._basketService.userBasketId} is removed successfully`);
        this._basketService.userBasket.set({basket: [], id: this._basketService.userBasketId} as IBasket);
      },
      error: error => {console.log(`Error: removing the basket with id: ${this._basketService.userBasketId} -> ${error}`);}
    });
  }

  toggleBasket(){
    this._basketService.toggleBasketSignal.update(oldVal => !oldVal);
  }
}
