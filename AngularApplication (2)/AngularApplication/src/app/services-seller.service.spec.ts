import { TestBed } from '@angular/core/testing';

import { ServicesSellerService } from './services-seller.service';

describe('ServicesSellerService', () => {
  let service: ServicesSellerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicesSellerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
