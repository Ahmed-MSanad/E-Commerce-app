import { Routes } from '@angular/router';
import { AuthLayotComponent } from './Layouts/auth-layot.component/auth-layot.component';
import { BlankLayoutComponent } from './Layouts/blank-layout.component/blank-layout.component';
import { authGuardGuard } from './Core/Guards/auth.guard-guard';
import { siteGuardGuard } from './Core/Guards/site.guard-guard';
import { NotFoundPageComponent } from './Components/not-found-page.component/not-found-page.component';

export const routes: Routes = [
    {path: "", component: AuthLayotComponent, canActivate:[authGuardGuard], children: [
        {path: '', redirectTo: 'Login', pathMatch:'full'},
        {path: "Register", loadComponent: () => import("./Components/register.component/register.component").then(tsFile => tsFile.RegisterComponent)},
        {path: "Login", loadComponent: () => import("./Components/login.component/login.component").then(tsFile => tsFile.LoginComponent)},
    ]},
    {path:"", component: BlankLayoutComponent, canActivate:[siteGuardGuard], children: [
        { path: '', redirectTo: 'Products', pathMatch:'full' },
        {path:"Products", loadComponent: () => import("./Components/products/products.component").then((tsFile) => tsFile.ProductsComponent)},
        {path:"ProductDetails/:id", loadComponent: () => import("./Components/product-details/product-details.component").then((tsFile) => tsFile.ProductDetailsComponent)},
    ]},
    { path: '**', component: NotFoundPageComponent },
];
