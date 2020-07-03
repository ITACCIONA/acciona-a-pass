import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { ConfigTestingModule } from '../commons';
import { OutsourcingService } from './outsourcing.service';

describe('OutsourcingService', () => {
  let service: OutsourcingService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule, ConfigTestingModule]
    });
    service = TestBed.inject(OutsourcingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
