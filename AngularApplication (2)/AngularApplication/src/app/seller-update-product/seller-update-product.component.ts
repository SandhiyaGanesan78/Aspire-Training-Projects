import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductsService } from '../products.service';
import { product } from '../dataTypes';
import { FormBuilder,FormGroup, Validators,FormGroupDirective } from '@angular/forms';


@Component({
  selector: 'app-seller-update-product',
  templateUrl: './seller-update-product.component.html',
  styleUrls: ['./seller-update-product.component.css']
})
export class SellerUpdateProductComponent implements OnInit {
  productData:undefined |product;
  productMessage:undefined|string;
  updateProduct:FormGroup;
constructor(private route:ActivatedRoute,private product:ProductsService, private formBuilder:FormBuilder )
{
  this.updateProduct=formBuilder.group({
    name: ["", [Validators.required, Validators.minLength(5), Validators.maxLength(25)]],
    price: ["", [Validators.required]],
    category: ["", [Validators.required]],
    description: ["", [Validators.required, Validators.maxLength(500)]],
    image: ["", [Validators.required]]
  });

}
  ngOnInit(): void {
    let productId=this.route.snapshot.paramMap.get('id');
    console.warn(productId)
    productId && this.product.getProduct(productId).subscribe((data)=>{
      console.warn(data);
      this.productData=data;
    });


  }
submit(data:any)
{
  console.warn(data);
  if(this.productData)
  {
    data.id=this.productData.id;
  }
  this.product.updateProduct(data).subscribe((result)=>{
    if(result){
      this.productMessage="Product details has Updated"
    }
  });
  setTimeout(()=>{
    this.productMessage=undefined;
  },3000)

  }

}

