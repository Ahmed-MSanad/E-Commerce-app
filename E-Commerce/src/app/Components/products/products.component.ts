import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { ProductService } from '../../Core/Services/product.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { IProduct } from '../../Core/Interfaces/iproduct';
import { NgxSpinnerModule } from 'ngx-spinner';
import { SortByEnum, sortDirectionEnum } from '../../Core/Enums/sort-by.enum';
import { CommonModule, JsonPipe } from '@angular/common';
import { Subscription } from 'rxjs';
import { BasketService } from '../../Core/Services/basket.service';
import { BasketComponent } from '../basket.component/basket.component';


@Component({
  selector: 'app-products.component',
  imports: [FormsModule, NgxSpinnerModule, CommonModule, BasketComponent, JsonPipe],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements OnInit, OnDestroy{
  readonly _productService = inject(ProductService);
  readonly _basketService = inject(BasketService);
  private readonly _Router = inject(Router);
  search : WritableSignal<string> = signal("");
  productList : WritableSignal<IProduct[]> = signal([]);
  sortMechanism : SortByEnum | undefined;
  SortByEnum = SortByEnum;
  sortDirection : WritableSignal<sortDirectionEnum> = signal(sortDirectionEnum.down);
  sortDirectionEnum = sortDirectionEnum;
  isLoading = false;
  errorMessage: string | null = null;
  getProductsSubscription ! : Subscription;
  pagination : {totalItemsCount: WritableSignal<number>, pageIndex: WritableSignal<number>, pageSize: WritableSignal<number>} = {
    totalItemsCount: signal(0), pageIndex: signal(1), pageSize: signal(5)
  };

  ngOnInit(){
    this.GetProducts();
  }
  GetProducts(){
    this.getProductsSubscription?.unsubscribe();
    this.getProductsSubscription = this._productService.getAllProducts(this.search(), this.pagination.pageIndex()).subscribe({
      next:(res : any) => {
        this.productList.set(res.itemsList);
        this.pagination.pageIndex.set(res.pageIndex);
        this.pagination.pageSize.set(res.pageSize);
        this.pagination.totalItemsCount.set(res.totalItemsCount);
        this._productService.filteredList.set([...this.productList()]);
        console.log("filtered List: ", this._productService.filteredList());
        console.log("product List: ", this.productList());
      },
      error:(err) => {
        console.log(err.error);
      }
    });
  }
  SearchForProducts() : void{
    this.pagination.pageIndex.set(1);
    this.pagination.pageSize.set(8);
    this.GetProducts();
  }
  SortProducts(){
    if(this.sortMechanism == SortByEnum.Name){
      this._productService.filteredList.update(oldVal => oldVal.sort((p1, p2) => p1.name.toLowerCase() <= p2.name.toLowerCase() ? -1 : 1));
    }
    else if(this.sortMechanism == SortByEnum.Price){
      this._productService.filteredList.update(oldVal => oldVal.sort((p1, p2) => p1.price <= p2.price ? -1 : 1));
    }
  }
  ChangeSortDirection(){
    this._productService.filteredList.update(oldVal => oldVal.reverse());
    this.sortDirection.update(oldVal => oldVal == sortDirectionEnum.down ? sortDirectionEnum.up : sortDirectionEnum.down);
  }
  ResetSearchFilter(){
    this.search.set("");
    this._productService.filteredList.set([...this.productList()]);
    this.reApplyOtherFilters();
  }
  ResetSortMechanismFilter(){
    this.sortMechanism = undefined;
    this._productService.filteredList.set([...this.productList()]);
    this.sortDirection.set(sortDirectionEnum.down);
    this.reApplyOtherFilters();
  }
  reApplyOtherFilters(){
    if(this.search()){
      this.SearchForProducts();
    }
    if(this.sortMechanism){
      this.SortProducts();
    }
    if(this.sortDirection() == sortDirectionEnum.up){
      this.ChangeSortDirection();
    }
  }

  ClearFilters(){
    this.search.set("");
    this.sortMechanism = undefined;
    this._productService.filteredList.set([...this.productList()]);
  }
  GoToProductDetailsPage(id : number) : void{
    this._Router.navigate(["/ProductDetails", id]);
  }

  showPreviousImage(productIndex: number): void {
      const product = this._productService.filteredList()[productIndex];
      if (!product?.productImages || product.productImages.length <= 1) return;  // Guard: No-op if invalid/single image
      product.currentImageIndex.update(oldIndex => oldIndex > 0 ? oldIndex - 1 : product.productImages.length - 1);
  }

  showNextImage(productIndex: number): void {
      const product = this._productService.filteredList()[productIndex];
      if (!product?.productImages || product.productImages.length <= 1) return;  // Guard
      product.currentImageIndex.update(oldIndex => oldIndex < product.productImages.length - 1 ? oldIndex + 1 : 0);
  }

  getPreviousPage() : void{
    this.pagination.pageIndex.update(oldIndex => oldIndex == 0 ? oldIndex : oldIndex - 1);
    console.log("new page index: ", this.pagination.pageIndex());
    this.GetProducts();
  }
  getNextPage() : void{
    this.pagination.pageIndex.update(oldIndex => oldIndex * this.pagination.pageSize() >= this.pagination.totalItemsCount() ? oldIndex : oldIndex + 1);
    console.log("new page index: ", this.pagination.pageIndex());
    this.GetProducts();
  }

  ngOnDestroy(){
    this.getProductsSubscription?.unsubscribe();
  }
}
