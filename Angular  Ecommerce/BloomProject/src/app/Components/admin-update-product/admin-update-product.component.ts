import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ProductsService } from 'src/app/Services/products.service';

@Component({
  selector: 'app-admin-update-product',
  templateUrl: './admin-update-product.component.html',
  styleUrls: ['./admin-update-product.component.css']
})
export class AdminUpdateProductComponent implements OnInit {
  productData:any;
  productMessage:any;
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
