import { IProduct } from './../../Core/Interfaces/iproduct';
import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../Core/Services/product.service';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BasketService } from '../../Core/Services/basket.service';

@Component({
  selector: 'app-product-details.component',
  imports: [NgxSpinnerModule],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit{
  private readonly _activatedRoute = inject(ActivatedRoute);
  readonly _productService = inject(ProductService);
  readonly _basketService = inject(BasketService);
  product : WritableSignal<IProduct> = signal({IsLoved:signal(false), currentImageIndex:signal(0)} as IProduct);
  ngOnInit(){
    this._activatedRoute.paramMap.subscribe((routeParams) => {
      this.GetProductDetails(routeParams.get("id"));
    })
  }
  GetProductDetails(id : string | null){
    if(id !== null){
      this._productService.getProductDetails(+id).subscribe({
        next:(res) => {
          this.product.set(({...res, IsLoved: signal(false), currentImageIndex: signal(0)}));
          console.log(`Fetched Product Details: `, this.product());
          this.product.update((oldVal) => {oldVal.IsLoved = signal(false); return oldVal;})
          console.log(this.product());
        },
        error:(err) => {
          console.log(err.error);
        }
      });
    }
    else{
      console.log("No Id is there !!!");
    }
  }
  showPreviousImage() : void{
    this.product().currentImageIndex.update(oldIndex => oldIndex > 0 ? oldIndex - 1 : this.product().productImages.length - 1);
  }
  showNextImage() : void{
    this.product().currentImageIndex.update(oldIndex => oldIndex < this.product().productImages.length - 1 ? oldIndex + 1 : 0);
  }
}
