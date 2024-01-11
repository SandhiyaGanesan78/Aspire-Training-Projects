import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './Components/login/login.component';
import { SignupComponent } from './Components/signup/signup.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {  HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './Components/home/home.component';
import{ NgToastModule} from 'ng-angular-popup';
import { HeaderComponent } from './Components/header/header.component';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import  { AdminHomeComponent } from './Components/Admin-home/Admin-home.component';
import { AdminAddProductComponent } from './Components/admin-add-product/admin-add-product.component';
import { AdminUpdateProductComponent } from './Components/admin-update-product/admin-update-product.component';
import '@angular/localize/init';
import { SearchComponent } from './Components/search/search.component';
import { ProductDetailsComponent } from './Components/product-details/product-details.component';
import { FooterComponent } from './Components/footer/footer.component';
import { CategoryComponent } from './Components/category/category.component';
import { TokenInterceptor } from 'token.interceptor';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,

    HeaderComponent,
    AdminHomeComponent,
    AdminAddProductComponent,
    AdminUpdateProductComponent,
    SearchComponent,
    ProductDetailsComponent,
    FooterComponent,

    CategoryComponent,




  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgToastModule,
    CommonModule,
    FontAwesomeModule,HomeComponent
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
