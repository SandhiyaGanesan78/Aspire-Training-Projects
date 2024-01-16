
import { Component, OnInit } from '@angular/core';
import { NgbCarouselModule } from '@ng-bootstrap/ng-bootstrap';
import { NgIf,NgFor } from '@angular/common';
import { ProductsService } from '../products.service';
import { product } from '../dataTypes';
import { Router, RouterModule } from '@angular/router';

@Component({
	selector: 'app-home',
	standalone: true,
	imports: [NgbCarouselModule, NgIf,NgFor,RouterModule],
	templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  popularProducts:undefined | product[];
  trendyProducts:undefined|product[];


  constructor(private product:ProductsService,private router:Router)
  {

  }
  ngOnInit(): void {
    this.product.popularProducts().subscribe((data)=>{
      this.popularProducts=data;
    });
    this.product.trendyProducts().subscribe((data)=>{
      this.trendyProducts=data;
    })
  }
  isBestSeller(item: any): boolean {
    return item.BestSeller;
  }



}
