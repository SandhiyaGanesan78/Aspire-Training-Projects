import { EventEmitter, Injectable, numberAttribute } from '@angular/core';
import { cart, order, product } from './dataTypes';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  private baseUrl:string=`http://localhost:5176/Products`;
  cartData=new EventEmitter<product[]|[]> ()
  constructor(private http:HttpClient) { }
  addProduct(data:product)
  {
    return this.http.post(this.baseUrl,data);
  }
  productlist()
  {
    return this.http.get<product[]>(this.baseUrl);
  }
  deleteProduct(id:number){
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
  getProduct(id: string) {
    return this.http.get<product>(`${this.baseUrl}/${id}`);
  }
  updateProduct(product:product){
    return this.http.put<product>(`${this.baseUrl}/${product.id}`,product)
  }
  popularProducts(){
    return this.http.get<product[]>(`${this.baseUrl}/carousel?_limit=3`);
  }
  getProducts(category: string): Observable<product[]> {
    return this.http.get<product[]>(`${this.baseUrl}/category?category=${category}`)
  }

  trendyProducts()
  {
    return this.http.get<product[]>(`${this.baseUrl}/trendy?_limit=8`)
  }
  searchProduct(query:string)
  {
    return this.http.get<product[]>(`${this.baseUrl}/search?query=${query}`)
  }
  localAddToCart(data:product){
    let cartData=[];
    let localCart=localStorage.getItem('localCart')
    if(!localCart){
      localStorage.setItem('localCart',JSON.stringify([data]));
      this.cartData.emit([data]);
      }
      else{
        cartData=JSON.parse(localCart);
        cartData.push(data);
        localStorage.setItem('localCart',JSON.stringify(cartData));
        this.cartData.emit(cartData);
      }
    }
  removeItemFromCart(productId:number){
      let cartData= localStorage.getItem('localCart');
      if(cartData){
        let items:product[] =JSON.parse(cartData);
        items= items.filter((item:product)=>
        productId!==item.id)
        localStorage.setItem('localCart',JSON.stringify(items));
        this.cartData.emit(items);
        }
      }
  addToCart(cartData:cart){
    return this.http.post(' http://localhost:3000/cart',cartData);
  }
  getCartList(userId:number){
    return this.http.get<product[]>('http://localhost:3000/cart?userId='+userId,{
      observe:'response'
      }).subscribe((result)=>{
          if(result &&result.body){
            this.cartData.emit(result.body);
          }
        })
  }
  removeToCart(cartId:number){
    return this.http.delete('http://localhost:3000/cart/'+cartId);
  }
  currentCart()
  {
    let userStore =localStorage.getItem('user');
    let userData=userStore && JSON.parse(userStore);
    return this.http.get<cart[]>('http://localhost:3000/cart?userId='+userData.id)
  }
  orderNow(data:order)
  {
    return this.http.post('http://localhost:3000/orders',data);
  }
  orderList(){
    let userStore =localStorage.getItem('user');
    let userData=userStore && JSON.parse(userStore);
    return this.http.get<order[]>('http://localhost:3000/orders?userId='+userData.id);
  }
  deleteCartItems(cartId:number){
    return this.http.delete('http://localhost:3000/cart/'+cartId,{observe:'response'}).subscribe((result)=>{
      if(result){
        this.cartData.emit([])
      }
    });

  }
  cancelOrder(orderId:number){
    return this.http.delete('http://localhost:3000/orders/'+orderId);
  }
}
