import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProductsService } from '../products.service';
import { product } from '../dataTypes';
import{faGifts,faSearch} from '@fortawesome/free-solid-svg-icons'
import { Subject } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit{
  menuType:string='default';
  sellerName:string="";
  userName:string="";
  searchResult:undefined|product[];
  cartItems=0;
  icon=faGifts;
  iconsearch=faSearch;
  categories: string[] = ['Flowers', 'Combos', 'Hampers', 'Cakes', 'Chocolates'];
  selectedCategory: string = '';
  products: product[] = [];

  constructor(private route: Router, private product: ProductsService){

  }
  ngOnInit(): void {
      this.route.events.subscribe((val:any)=>
      {
        if(val.url){
          if(localStorage.getItem('seller') && val.url.includes('seller'))
          {
            let sellerStore=localStorage.getItem('seller');
            let sellerData=sellerStore && JSON.parse(sellerStore)[0];
            this.sellerName=sellerData.name;
            this.menuType='seller';
          }
          else if(localStorage.getItem('user')){
            let userStore =localStorage.getItem('user');
            let userData=userStore && JSON.parse(userStore);
            this.userName=userData.name;
            this.menuType='user';
            this.product.getCartList(userData.id);
          }
          else{
            this.menuType='default';
          }
        }
      });
      let cartData=localStorage.getItem('localCart');
      if(cartData){
        this.cartItems=JSON.parse(cartData).length
      }
      this.product.cartData.subscribe((items)=>{
        this.cartItems=items.length;
      })
    }
    logout()
      {
        localStorage.removeItem('seller');
        this.route.navigate(['/'])
      }
      searchProduct(query:KeyboardEvent){
        if(query){
          const element=query.target as HTMLInputElement;
          this.product.searchProduct(element.value).subscribe((result)=>
          {
            if(result.length>10){
              result.length=length
            }
            this.searchResult=result;
          });
        }

      }
      hideSearch()
      {
        this.searchResult=undefined
      }
      submitSearch(value:string){
        console.warn(value)
        this.route.navigate([`search/${value}`]);

      }
      redirectToDeatils(id:number){
        this.route.navigate(['/details/'+id]);
      }
      userLogout(){
        localStorage.removeItem('user');
        this.route.navigate(['/user-auth'])
        this.product.cartData.emit([])
      }

        filterProducts(category: string) {
          this.selectedCategory = category;

          this.product.getProducts(category).subscribe(
            (products) => {
              this.navigateToProductListing(category, products);
            },
            (error) => {
              console.error(error);
            }
          );
        }

        navigateToProductListing(category: string, products: product[]) {
          this.route.navigate([`category/${category}`], { state: { products: products } });        }


}


