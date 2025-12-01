import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { environment } from '../environments/environment';
import { IBasket, IBasketItem } from '../Interfaces/ibasket';
import { Observable } from 'rxjs';
import { ProductService } from './product.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  private readonly _http = inject(HttpClient);
  readonly _productService = inject(ProductService);

  userBasketId : string = "Basket_01";
  userBasket : WritableSignal<IBasket> = signal({basket: [], id: this.userBasketId} as IBasket);
  toggleBasketSignal : WritableSignal<boolean> = signal(false)

  setBasket(basket : IBasket){
    return this._http.put(`${environment.apiBaseUrl}/Basket/Update`, basket);
  }
  getBasket(basketId : string) : Observable<IBasket>{
    return this._http.get<IBasket>(`${environment.apiBaseUrl}/Basket/Get/${basketId}`);
  }
  removeBasket(basketId : string){
    return this._http.delete(`${environment.apiBaseUrl}/Basket/Delete/${basketId}`);
  }

  setToBasket(){
    this.setBasket(this.userBasket()).subscribe({
      next: _ => { console.log("Basket is set successfully", _); },
      error: (error) => {console.log(`Error setting the basket: ${error}`);}
    });
  }
  AddToBasket(productId : number){
    this.userBasket.update(oldBasket => {
      const index = oldBasket.basket.findIndex(item => item.id == productId);
      if(index !== -1){
        let newBasket : IBasket = {id: oldBasket.id, basket: [
          ...oldBasket.basket.slice(0, index), 
          {...oldBasket.basket[index], basketQuantity: oldBasket.basket[index].basketQuantity + 1},
          ...oldBasket.basket.slice(index + 1)
        ]};
        return newBasket; // doing so will fire the signal to set the new total basket cost in the UI,, can be done also by changing the settings of the signal to allow it to change when a deep change happens
        // oldBasket.basket[index].basketQuantity++; // just simply returning the old basket won't fire the total basket cost as we change deeply in an item quantity
      }
      else{
        const product = this._productService.filteredList().find(p => p.id === productId);
        if (product) {
          let images : string[] = product.productImages.map(imageObj => imageObj.image);
          let newBasketItem : IBasketItem = {id: product.id, name: product.name, description: product.description, price: product.price, 
            basketQuantity: 1, brand: product.productBrand, type: product.productSubCategory, productImages: images};
          let newBasketList : IBasketItem[] = [...oldBasket.basket, newBasketItem];
          let newBasket : IBasket = {id: oldBasket.id, basket: newBasketList};
          return newBasket;
        }
      }
      return oldBasket; // just simply returning the old basket won't fire the total basket cost as we change deeply in an item quantity
    });
    this.toggleBasketSignal.set(true);
    this.setToBasket();
  }
}
