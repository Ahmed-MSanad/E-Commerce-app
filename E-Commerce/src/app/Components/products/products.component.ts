import { Component, effect, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { ProductService } from '../../Core/Services/product.service';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ICartProduct, IProduct } from '../../Core/Interfaces/iproduct';
import { NgxSpinnerModule } from 'ngx-spinner';
import { SortByEnum, sortDirectionEnum } from '../../Core/Enums/sort-by.enum';
import { CommonModule, CurrencyPipe, JsonPipe } from '@angular/common';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-products.component',
  imports: [FormsModule, NgxSpinnerModule, CommonModule, CurrencyPipe],
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent implements OnInit, OnDestroy{
  readonly _productService = inject(ProductService);
  private readonly _Router = inject(Router);
  search : WritableSignal<string> = signal("");
  productList : WritableSignal<IProduct[]> = signal([]);
  filteredList : WritableSignal<IProduct[]> = signal([]);
  sortMechanism : SortByEnum | undefined;
  SortByEnum = SortByEnum;
  sortDirection : WritableSignal<sortDirectionEnum> = signal(sortDirectionEnum.down);
  sortDirectionEnum = sortDirectionEnum;
  isLoading = false;
  errorMessage: string | null = null;
  toggleCartSignal : WritableSignal<boolean> = signal(false);
  getProductsSubscription ! : Subscription;
  userCartOfProducts : WritableSignal<ICartProduct[]> = signal([]);
  pagination : {totalItemsCount: WritableSignal<number>, pageIndex: WritableSignal<number>, pageSize: WritableSignal<number>} = {
    totalItemsCount: signal(0), pageIndex: signal(1), pageSize: signal(5)
  };

  toggleCart(){
    this.toggleCartSignal.update(oldVal => !oldVal);
  }

  constructor() {
    if(typeof(localStorage) !== 'undefined'){
      const savedCart = localStorage.getItem('userCart');
      if (savedCart) {
        let parsedCart = JSON.parse(savedCart);

        parsedCart = parsedCart.map((cartProduct : any) => ({
          productCount: cartProduct.productCount,
          product: {
            ...cartProduct.product,
            IsLoved: signal(cartProduct.product.IsLoved),
            currentImageIndex: signal(cartProduct.product.currentImageIndex)
          }
        }));
        this.userCartOfProducts.set(parsedCart);
      }
      
      effect(() => {
        let tempMappedSignals = this.userCartOfProducts().map(cartProduct => ({
            productCount: cartProduct.productCount,
            product: {...cartProduct.product, currentImageIndex: cartProduct.product.currentImageIndex(), IsLoved: cartProduct.product.IsLoved()}
          })
        );
        localStorage.setItem('userCart', JSON.stringify(tempMappedSignals));
      });
    }
  }

  ngOnInit(){
    this.GetProducts();
  }
  GetProducts(){
    this.getProductsSubscription = this._productService.getAllProducts(this.search(), this.pagination.pageIndex()).subscribe({
      next:(res : any) => {
        this.productList.set(res.itemsList);
        this.pagination.pageIndex.set(res.pageIndex);
        this.pagination.pageSize.set(res.pageSize);
        this.pagination.totalItemsCount.set(res.totalItemsCount);
        this.filteredList.set([...this.productList()]);
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
      this.filteredList.update(oldVal => oldVal.sort((p1, p2) => p1.name.toLowerCase() <= p2.name.toLowerCase() ? -1 : 1));
    }
    else if(this.sortMechanism == SortByEnum.Price){
      this.filteredList.update(oldVal => oldVal.sort((p1, p2) => p1.price <= p2.price ? -1 : 1));
    }
  }
  ChangeSortDirection(){
    this.filteredList.update(oldVal => oldVal.reverse());
    this.sortDirection.update(oldVal => oldVal == sortDirectionEnum.down ? sortDirectionEnum.up : sortDirectionEnum.down);
  }
  ResetSearchFilter(){
    this.search.set("");
    this.filteredList.set([...this.productList()]);
    this.reApplyOtherFilters();
  }
  ResetSortMechanismFilter(){
    this.sortMechanism = undefined;
    this.filteredList.set([...this.productList()]);
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
  
  AddToCart(productId : number){
    this.userCartOfProducts.update(oldList => {
      const index = oldList.findIndex(p => p.product.id == productId);
      if(index !== -1){
        oldList[index].productCount++;
      }
      else{
        const product = this.filteredList().find(p => p.id === productId);
        if (product) {
          return [{ product: product, productCount: 1 }, ...oldList];
        }
      }
      return oldList;
    });
    this.toggleCartSignal.set(true);
  }
  RemoveProductFromCart(productId: number) {
    this.userCartOfProducts.update(oldList => {
      const index = oldList.findIndex(product => product.product.id === productId);
      if (index !== -1) {
        if(oldList[index].productCount === 1){
          return [...oldList.slice(0, index), ...oldList.slice(index + 1)];
        }
        else{
          oldList[index].productCount--;
        }
      }
      return oldList;
    });
  }
  ClearFilters(){
    this.search.set("");
    this.sortMechanism = undefined;
    this.filteredList.set([...this.productList()]);
  }
  GoToProductDetailsPage(id : number) : void{
    this._Router.navigate(["/ProductDetails", id]);
  }

  showPreviousImage(productIndex : number) : void{
    this.filteredList()[productIndex].currentImageIndex.update(oldIndex => oldIndex > 0 ? oldIndex - 1 : this.productList()[productIndex].productImages.length - 1);
  }
  showNextImage(productIndex : number) : void{
    this.filteredList()[productIndex].currentImageIndex.update(oldIndex => oldIndex < this.productList()[productIndex].productImages.length - 1 ? oldIndex + 1 : 0);
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
