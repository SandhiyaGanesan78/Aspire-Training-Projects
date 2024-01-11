import { Component, OnInit } from '@angular/core';
import { Router,RouterModule } from '@angular/router';
import { ApiService } from 'src/app/Services/api.service';
import { AuthService } from 'src/app/Services/auth.service';
import { ProductsService } from 'src/app/Services/products.service';
import { UserStoreService } from 'src/app/Services/user-store.service';
import { NgbCarouselModule,NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgIf,NgFor } from '@angular/common';
import { CartService } from 'src/app/Services/cart.service';
@Component({
  selector: 'app-home',
  standalone: true,
	imports: [NgbCarouselModule, NgIf,NgFor,RouterModule,NgbModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  public users:any=[];
  public fullName: any="";
  public role: any="";
  popularProducts:undefined | any[];
  trendyProducts:undefined|any[];
  constructor(private auth:AuthService,private route:Router,
    private api: ApiService, private store:UserStoreService,
    private product:ProductsService,
    private cart:CartService)
  {

  }
ngOnInit(){

  this.api.getUsers().subscribe(result=>{
    this.users=result;
  });
  // Check if the user is already logged in
  if (this.auth.isLoggedIn()) {
    // Sync cart items with the server
    const cartItems = this.product.getCartItems();
    if (cartItems && cartItems.length > 0) {
      this.cart.getCartItems().subscribe(
        (serverCartItems) => {
          if (serverCartItems && serverCartItems.length > 0) {
            // Merge local cart items with the server cart items if necessary
            // You may need to handle duplicates and updates based on your server implementation
            const mergedCartItems = [...cartItems, ...serverCartItems];
            this.cart.updateCartItems(mergedCartItems).subscribe(
              () => {
                this.product.clearCart(); // Clear the local cart once synced
              },
              (error) => {
                console.error('Failed to update cart items on the server', error);
              }
            );
          }
        },
        (error) => {
          console.error('Failed to get cart items from the server', error);
        }
      );
    }
  }

  console.log(this.store.getFullNameFromStore());
  this.store.getFullNameFromStore().subscribe(value=>{
    let fullNameFromToken=this.auth.getFullNameFromToken();
    console.log(fullNameFromToken);
    this.fullName=value || fullNameFromToken
    console.log(this.fullName);
  });
  this.store.getRoleFromStore().subscribe(value=>{
    const rolefromtoken= this.auth.getRoleFromToken();
    this.role=value|| rolefromtoken;
  });
  this.product.popularProducts().subscribe((data)=>{
    this.popularProducts=data;
    console.log(this.popularProducts);
  });
  this.product.trendyProducts().subscribe((data)=>{
    this.trendyProducts=data;
  })
  }

logout() {
this.auth.signOut();

}
}
