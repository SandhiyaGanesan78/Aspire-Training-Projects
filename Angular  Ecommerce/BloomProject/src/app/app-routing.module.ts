import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { SignupComponent } from './Components/signup/signup.component';
import { HomeComponent } from './Components/home/home.component';
import { AuthGuard } from './guards/auth.guard';
import { AdminHomeComponent } from './Components/Admin-home/Admin-home.component';
import { AdminAddProductComponent } from './Components/admin-add-product/admin-add-product.component';
import { AdminUpdateProductComponent } from './Components/admin-update-product/admin-update-product.component';
import { SearchComponent } from './Components/search/search.component';
import { ProductDetailsComponent } from './Components/product-details/product-details.component';
import { CategoryComponent } from './Components/category/category.component';

const routes: Routes = [
  {
    component:LoginComponent,
    path:'login'
  },
  {
    component:SignupComponent,
    path:'signup'
  },
  {
    component:HomeComponent,
    path:''
  },
  {
    component:AdminHomeComponent,
    path:'admin/home',
    canActivate:[AuthGuard]
  },
  {
    component:AdminAddProductComponent,
    path:'admin/add/product',
    canActivate:[AuthGuard]
  },
  {
    component:AdminUpdateProductComponent,
    path:'admin-update-product/:id',
    canActivate:[AuthGuard]
  },
  {
    component:SearchComponent,
    path:'search/:query'
  },
  {
    component: ProductDetailsComponent,
    path:'details/:productId'
  },
  {
    component:CategoryComponent,
    path:`category/:category`
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
