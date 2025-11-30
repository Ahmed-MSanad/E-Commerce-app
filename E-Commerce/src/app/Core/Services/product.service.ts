import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { environment } from '../environments/environment';
import { map, Observable } from 'rxjs';
import { IProduct } from '../Interfaces/iproduct';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly _httpClient = inject(HttpClient);

  getAllProducts(searchTerm : string) : Observable<any[]>{
    return this._httpClient.get<any[]>(`${environment.apiBaseUrl}/Product/GetProducts`, {
      params:{
        search: searchTerm
      }
    }).pipe(map((products : any[]) => {
      return products.map((product: any) => {
        return {...product, IsLoved: signal(false), currentImageIndex: signal(0)};
      });}));
  }

  getProductDetails(id : number) : Observable<any>{
    return this._httpClient.get<any>(`${environment.apiBaseUrl}/Product/GetProductDetails/${id}`);
  }

  ToggleIsLoved(product : IProduct) : void{
    product.IsLoved.update((oldVal) => !oldVal);
  }

  HandleImageError(event : ErrorEvent) : void{
    (event?.target as HTMLImageElement).src = 'default-product-image.png';
  }
}
