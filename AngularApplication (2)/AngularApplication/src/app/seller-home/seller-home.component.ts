import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../products.service';
import { product } from '../dataTypes';
import{faTrash,faEdit} from '@fortawesome/free-solid-svg-icons'

@Component({
  selector: 'app-seller-home',
  templateUrl: './seller-home.component.html',
  styleUrls: ['./seller-home.component.css']
})
export class SellerHomeComponent implements OnInit {
  productList:undefined | product[];
  productMessage:undefined | string;
  icon=faTrash;
  iconEdit=faEdit;
  constructor(private product:ProductsService )
  {

  }
  ngOnInit(): void {
    this.list();
  }
deleteProduct(id:number){
this.product.deleteProduct(id).subscribe((result)=>{
  if(result){
  this.productMessage="Product is deleted";
  this.list();
}
});
setTimeout(()=>{
  this.productMessage=undefined;
},3000);
}

list(){
  this.product.productlist().subscribe((result)=>{
    if(result){

      this.productList=result;
      console.log(this.productList)
    }
  });
}
}
