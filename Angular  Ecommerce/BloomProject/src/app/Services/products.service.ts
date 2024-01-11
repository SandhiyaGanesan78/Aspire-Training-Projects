import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  cartData=new EventEmitter<any[]|[]> ()
  private cartItemsKey = 'cartItems';

  constructor(private http:HttpClient) {

  }
  addProduct(data:any)
  {
    return this.http.post('http://localhost:5176/Products',data);
  }
  productlist()
  {
    return this.http.get<any[]>('http://localhost:5176/Products');
  }
  deleteProduct(id:any){
    return this.http.delete(`http://localhost:5176/Products/${id}`);
  }
  getProduct(id:any) {
    return this.http.get<any>(`http://localhost:5176/Products/${id}`);
  }
  updateProduct(product:any){
    return this.http.put<any>(`http://localhost:5176/Products/${product.id}`,product)
  }
  popularProducts(){
    return this.http.get<any[]>('http://localhost:5176/Products/carousel?_limit=3');
  }
  getProducts(category:any): Observable<any[]> {
    return this.http.get<any[]>(`http://localhost:5176/Products/category?category=${category}`)
  }

  trendyProducts()
  {
    return this.http.get<any[]>('http://localhost:5176/Products/trendy?_limit=8')
  }
  searchProduct(query:any)
  {
    return this.http.get<any[]>(`http://localhost:5176/Products/search?query=${query}`)
  }
  addToCart(item: any) {
    let cartItems = this.getCartItems() || [];
    cartItems.push(item);
    localStorage.setItem(this.cartItemsKey, JSON.stringify(cartItems));
  }

  removeFromCart(itemId: number) {
    let cartItems = this.getCartItems() || [];
    cartItems = cartItems.filter(item => item.id !== itemId);
    localStorage.setItem(this.cartItemsKey, JSON.stringify(cartItems));
  }

  getCartItems(): any[] {
    const cartItemsString = localStorage.getItem(this.cartItemsKey);
    if (cartItemsString) {
      return JSON.parse(cartItemsString) as any[];
    }
    return [];
  }

  clearCart() {
    localStorage.removeItem(this.cartItemsKey);
  }


}
