export interface product{
  name:string,
  price:number,
  category:string,
  description:string,
  image:string,
  id:string,
  quantity:string,
  productId:string
}
export interface cart{
  name:string,
  price:number,
  category:string,
  description:string,
  image:string,
  id:string,
  quantity:string,
   productId:string,
  userId:string
}
export interface priceSummary{
  price:number,
  discount:number,
  tax:number,
  delivery:number,
  total:number
}
export interface order{
  email:string,
  address:string,
  contact:string,
  totalPrice:number,
  userId:string,id:string
}
