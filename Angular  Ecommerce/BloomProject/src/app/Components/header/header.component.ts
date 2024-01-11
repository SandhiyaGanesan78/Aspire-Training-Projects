import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Services/auth.service';
import { UserStoreService } from 'src/app/Services/user-store.service';
import { Router } from '@angular/router';
import { faGifts, faSearch } from '@fortawesome/free-solid-svg-icons';
import { ProductsService } from 'src/app/Services/products.service';
import { CartService } from 'src/app/Services/cart.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  menuType:any='default';
  public fullName: any="";
  userName:any="";
  public role: any="";
  cartItems=0;
  searchResult:any[]|undefined;
  icon=faGifts;
  iconsearch=faSearch;
  products: any[] = [];
  categories: string[] = ['Flowers', 'Combos', 'Hampers', 'Cakes', 'Chocolates'];
  selectedCategory: any = '';
  constructor(private store:UserStoreService,
    private auth:AuthService,private router:Router, private product: ProductsService,
    private cart:CartService){

  }
  ngOnInit(){
    this.store.getFullNameFromStore().subscribe(value=>{
      let fullNameFromToken=this.auth.getFullNameFromToken();
      console.log(fullNameFromToken);
      this.fullName=value || fullNameFromToken
      console.log(this.fullName);
    });
    this.store.getRoleFromStore().subscribe(value=>{
      const rolefromtoken= this.auth.getRoleFromToken();
      this.role=value|| rolefromtoken;
      console.log(this.role);

      if(this.role==='Admin')
      {
        return this.menuType='Admin';
      }
      else if(this.role==='User')
      {
        
        return this.menuType='User';


      }
      else{
        return this.menuType='default'
      }
    });
    let cartData=localStorage.getItem('cartItems');
      if(cartData){
        this.cartItems=JSON.parse(cartData).length
      }
      this.product.cartData.subscribe((items)=>{
        this.cartItems=items.length;
      })

  }
  logout() {
    this.auth.signOut();
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
    submitSearch(value:any){
      console.warn(value)
      this.router.navigate([`search/${value}`]);

    }
    redirectToDeatils(id:any){
      this.router.navigate(['/details/'+id]);
    }
    userLogout(){
      this.router.navigate(['login'])
      this.product.cartData.emit([])
    }

      filterProducts(category: any) {
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

      navigateToProductListing(category: any, products: any[]) {
        this.router.navigate([`category/${category}`], { state: { products: products } });        }



}

