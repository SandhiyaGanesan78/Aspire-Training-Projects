import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = `http://localhost:5176/api/Cart`;
  constructor(private http: HttpClient) { }
  addToCart(item: any) {
    return this.http.post<any>(`${this.apiUrl}/Cart/addtocart`, item);
  }

  removeFromCart(itemId: number) {
    return this.http.delete<any>(`${this.apiUrl}/Cart/removefromcart/${itemId}`);
  }

  getCartItems() {
    return this.http.get<any[]>(`${this.apiUrl}/Cart/getcartitems`);
  }
  updateCartItems(cartItems: any[]) {
    return this.http.put<any[]>(`${this.apiUrl}/Cart/updatecartitems`, cartItems);
  }
}
